/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */

using Internal.Runtime.CompilerServices;
using MOOS.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MOOS.Misc
{
    public unsafe class Thread
    {
        public bool Terminated;
        public IDT.IDTStackGeneric* Stack;
        public ulong SleepingTime;
        public int RunOnWhichCPU;
        public bool IsIdleThread;

        public Thread(delegate*<void> method)
        {
            Stack = (IDT.IDTStackGeneric*)Allocator.Allocate((ulong)sizeof(IDT.IDTStackGeneric));

            Stack->irs.cs = 0x08;
            Stack->irs.ss = 0x10;
            const int Size = 16384;
            Stack->irs.rsp = ((ulong)Allocator.Allocate(Size)) + (Size);

            Stack->irs.rsp -= 8;
            *(ulong*)(Stack->irs.rsp) = (ulong)(delegate*<void>)&ThreadPool.Terminate;

            Stack->irs.rflags = 0x202;

            Stack->irs.rip = (ulong)method;

            Terminated = false;

            SleepingTime = 0;
        }

        public Thread Start(bool is_idle_thread = false) 
        {
            //Bootstrap CPU
            this.RunOnWhichCPU = 0;
            this.IsIdleThread = is_idle_thread;
            ThreadPool.Threads.Add(this);
            return this;
        }

        public Thread Start(int run_on_which_cpu, bool is_idle_thread = false)
        {
            bool hasThatCPU = false;
            for(int i = 0; i < ACPI.LocalAPIC_CPUIDs.Count; i++) 
            {
                if (ACPI.LocalAPIC_CPUIDs[i] == run_on_which_cpu) hasThatCPU = true;
            }
            if (!hasThatCPU)
            {
                //Didn't find that CPU. run on bootstrap CPU
                run_on_which_cpu = 0;
            }

            this.RunOnWhichCPU = run_on_which_cpu;
            this.IsIdleThread = is_idle_thread;
            ThreadPool.Threads.Add(this);
            return this;
        }

        public static void Sleep(ulong Millionseconds) 
        {
            ThreadPool.Threads[ThreadPool.Index].SleepingTime = Millionseconds;
        }

        public static void Sleep(int Index,ulong Millionseconds)
        {
            ThreadPool.Threads[Index].SleepingTime = Millionseconds;
        }
    }

    internal static unsafe class ThreadPool
    {
        public static List<Thread> Threads;
        public static bool Initialized = false;

        public static bool Locked;
        public static volatile uint Locker;


        internal static int Index
        {
            get 
            {
                return Indexs[SMP.ThisCPU];
            }
            set 
            {
                Indexs[SMP.ThisCPU] = value;
            }
        }

        public static void Initialize()
        {
            Native.Cli();
            //Bootstrap CPU
            if(SMP.ThisCPU == 0)
            {
                byte size = 0;
                for (int i = 0; i < ACPI.LocalAPIC_CPUIDs.Count; i++)
                    if (ACPI.LocalAPIC_CPUIDs[i] > size) size = ACPI.LocalAPIC_CPUIDs[i];
                Indexs = new int[size + 1];

                UnLock();

                Initialized = false;
                Threads = new();
                new Thread(&IdleThread).Start(true);
                Initialized = true;
            }
            //Application CPU
            else
            {
                new Thread(&IdleThread).Start((int)SMP.ThisCPU, true);
            }
            Native.Sti();
            _int20h(); //start scheduling
        }

        private static void Terminate()
        {
            Console.Write("Thread ");
            Console.Write(Index.ToString());
            Console.WriteLine(" Has Exited");
            Threads[Index].Terminated = true;
            _int20h();
            Panic.Error("Termination Failed!");
        }

        [DllImport("*")]
        private static extern void _int20h();

        public static void Lock() 
        {
            if(!ThreadPool.Locked)
            {
                Locker = SMP.ThisCPU;
                LocalAPIC.SendAllInterrupt(0x20);
                ThreadPool.Locked = true;
            }
        }

        public static void UnLock()
        {
            Locker = 0xFFFFFFFF;
            ThreadPool.Locked = false;
        }

        public static bool CanLock => Unsafe.As<bool, ulong>(ref ThreadPool.Locked);

        private static void TestThread()
        {
            Console.WriteLine("Non-Loop Thread Test!");
            return;
        }

        private static void A()
        {
            for (; ; ) Console.WriteLine("Thread A");
        }

        private static void B()
        {
            for (; ; ) Console.WriteLine("Thread B");
        }

        public static void IdleThread()
        {
            for (; ; ) Native.Hlt();
        }

        private static int[] Indexs;

        private static ulong TickInSec;
        private static ulong TickIdle;
        private static ulong LastSec;
        public static ulong CPUUsage;

        internal static void Schedule(IDT.IDTStackGeneric* stack)
        {
            if (!Initialized  || Threads.Count == 0) return;
            while (Locked && Locker != SMP.ThisCPU) Native.Nop();

            if (
                !Threads[Index].Terminated &&
                Threads[Index].RunOnWhichCPU == SMP.ThisCPU
                )
            {
                Native.Movsb(Threads[Index].Stack, stack, (ulong)sizeof(IDT.IDTStackGeneric));
            }

            for(int i = 0; i < Threads.Count; i++) 
            {
                if (
                    Threads[i].SleepingTime > 0 &&
                    Threads[i].RunOnWhichCPU == SMP.ThisCPU
                    ) 
                    Threads[i].SleepingTime--;
            }

            do
            {
                Index = (Index + 1) % Threads.Count;
            } while 
            (
                Threads[Index].Terminated ||
                (Threads[Index].SleepingTime > 0) ||
                Threads[Index].RunOnWhichCPU != SMP.ThisCPU
            );

            #region CPU Usage
            if (LastSec != RTC.Second)
            {
                if (TickInSec != 0 && TickIdle != 0)
                    CPUUsage = 100 - ((TickIdle * 100) / TickInSec);
                TickIdle = 0;
                TickInSec = 0;
                LastSec = RTC.Second;
            }
            if (Threads[Index].IsIdleThread)
            {
                TickIdle++;
            }
            TickInSec++;
            #endregion

            Native.Movsb(stack, Threads[Index].Stack, (ulong)sizeof(IDT.IDTStackGeneric));
        }
    }
}
