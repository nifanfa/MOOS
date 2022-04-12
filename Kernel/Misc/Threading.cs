/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Kernel.Misc
{
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public unsafe struct FxsaveArea 
    {
        fixed byte Raw[512];
    }

    public unsafe class Thread
    {
        public bool Terminated;
        public IDT.IDTStack* stack;
        public FxsaveArea* fxsaveArea;

        public Thread(delegate*<void> method)
        {
            stack = (IDT.IDTStack*)Allocator.Allocate((ulong)sizeof(IDT.IDTStack));
            fxsaveArea = (FxsaveArea*)Allocator.Allocate((ulong)sizeof(FxsaveArea));
            Native.Movsb(fxsaveArea, ThreadPool.Fxdefault, 32);

            stack->cs = 0x08;
            stack->ss = 0x10;
            const int Size = 16384;
            stack->rsp = ((ulong)Allocator.Allocate(Size)) + (Size);

            stack->rsp -= 8;
            *(ulong*)(stack->rsp) = (ulong)(delegate*<void>)&ThreadPool.Terminate;

            stack->rflags = 0x202;

            stack->rip = (ulong)method;

            Terminated = false;

            ThreadPool.Threads.Add(this);
        }
    }

    internal static unsafe class ThreadPool
    {
        public static List<Thread> Threads;
        public static bool Ready = false;
        public static FxsaveArea* Fxdefault;

        public static void Initialize()
        {
            Fxdefault = (FxsaveArea*)Allocator.Allocate((ulong)sizeof(FxsaveArea));
            Native.Fxsave64(Fxdefault);

            Ready = false;
            Threads = new();
            new Thread(&IdleThread);
            new Thread(&TestThread);
            //new Thread(&A);
            //new Thread(&B);
            new Thread(&Program.KMain);
            Ready = true;
            _int20h(); //start scheduling
            IdleThread();
        }

        public static void Terminate()
        {
            Console.Write("Thread ");
            Console.Write(Index.ToString());
            Console.WriteLine(" Has Exited");
            Threads[Index].Terminated = true;
            _int20h();
            Panic.Error("Termination Failed!");
        }

        [DllImport("*")]
        public static extern void _int20h();

        public static void TestThread()
        {
            Console.WriteLine("Non-Loop Thread Test!");
            return;
        }

        public static void A()
        {
            for (; ; ) Console.WriteLine("Thread A");
        }

        public static void B()
        {
            for (; ; ) Console.WriteLine("Thread B");
        }

        public static void IdleThread()
        {
            for (; ; ) Native.Hlt();
        }

        public static int Index = 0;

        private static ulong TickInSec;
        private static ulong TickIdle;
        private static ulong LastSec;
        public static ulong CPUUsage;

        public static void Schedule(IDT.IDTStack* stack)
        {
            if (!Ready) return;

            if (!Threads[Index].Terminated)
            {
                Native.Movsb(Threads[Index].stack, stack, (ulong)sizeof(IDT.IDTStack));
                Native.Fxsave64(Threads[Index].fxsaveArea);
            }

            do
            {
                Index = (Index + 1) % Threads.Count;
            } while (Threads[Index].Terminated);

            #region CPU Usage
            if (LastSec != RTC.Second)
            {
                if (TickInSec != 0 && TickIdle != 0)
                    CPUUsage = 100 - ((TickIdle * 100) / TickInSec);
                TickIdle = 0;
                TickInSec = 0;
                LastSec = RTC.Second;
#if false
                Console.Write("CPU Usage: ");
                Console.Write(CPUUsage.ToString());
                Console.WriteLine("%");
#endif
            }
            //Make sure the index 0 is idle thread
            if (Index == 0)
            {
                TickIdle++;
            }
            TickInSec++;
            #endregion

            Native.Movsb(stack, Threads[Index].stack, (ulong)sizeof(IDT.IDTStack));
            Native.Fxrstor64(Threads[Index].fxsaveArea);
        }
    }
}
