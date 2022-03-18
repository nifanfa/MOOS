using OS_Sharp;
using OS_Sharp.Driver;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Kernel
{
    public unsafe class Thread
    {
        public IDT.IDTStack* stack;

        public Thread(delegate* <void> method)
        {
            ulong size = 1048576;
            byte* ptr = (byte*)Allocator.Allocate(size);
            Native.Stosb(ptr, 0, size);
            ptr -= sizeof(IDT.IDTStack);
            stack = (IDT.IDTStack*)ptr;

            stack->cs = 0x08;
            stack->rsp = (ulong)ptr;
            stack->rflags = 0x202;

            stack->rip = (ulong)method;

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
            new Thread(&IdleThread);
            //new Thread(&A);
            //new Thread(&B);
            new Thread(&Program.KMain);
            Ready = true;
            //Make sure the irq wont be triggered during _iretq
            Native.Hlt();
            IdleThread();
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

            Native.Movsb(Threads[Index].stack, stack, (ulong)sizeof(IDT.IDTStack));
            Index = (Index + 1) % Threads.Count;

            if(LastSec != RTC.Second)
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
            if (Index == 0)
            {
                TickIdle++;
            }
            TickInSec++;

            Native.Movsb(stack, Threads[Index].stack, (ulong)sizeof(IDT.IDTStack));
        }
    }
}
