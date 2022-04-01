﻿using System.Collections.Generic;
using System.Runtime.InteropServices;
using OS_Sharp;
using OS_Sharp.Misc;

namespace System.Threading
{
    public unsafe class Thread
    {
        public bool Terminated;
        public IDT.IDTStack* stack;

        public Thread(delegate*<void> method)
        {
            stack = (IDT.IDTStack*)Allocator.Allocate((ulong)sizeof(IDT.IDTStack));

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

        public static void Initialize()
        {
            Ready = false;
            Threads = new();
            _ = new Thread(&IdleThread);
            _ = new Thread(&Program.KMain);
            Ready = true;
            Native.Hlt();
        }

        public static void Terminate()
        {
            Threads[Index].Terminated = true;
            _int20h();
            Panic.Error("Termination Failed!");
        }

        [DllImport("*")]
        public static extern void _int20h();

        public static void IdleThread()
        {
            for (; ; )
            {
                Native.Hlt();
            }
        }

        public static int Index = 0;

        private static ulong TickInSec;
        private static ulong TickIdle;
        private static ulong LastSec;
        public static ulong CPUUsage;

        public static void Schedule(IDT.IDTStack* stack)
        {
            if (!Ready)
            {
                return;
            }

            if (!Threads[Index].Terminated)
            {
                Native.Movsb(Threads[Index].stack, stack, (ulong)sizeof(IDT.IDTStack));
            }

            do
            {
                Index = (Index + 1) % Threads.Count;
            } while (Threads[Index].Terminated);

            if (LastSec != RTC.Second)
            {
                if (TickInSec != 0 && TickIdle != 0)
                {
                    CPUUsage = 100 - ((TickIdle * 100) / TickInSec);
                }

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

            Native.Movsb(stack, Threads[Index].stack, (ulong)sizeof(IDT.IDTStack));
        }
    }
}
