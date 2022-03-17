using OS_Sharp;
using OS_Sharp.Driver;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Kernel
{
    public unsafe class Thread
    {
        public IDT.IDTStack* stack;

        public Thread()
        {
            ulong size = 1048576;
            byte* ptr = (byte*)Allocator.Allocate(size);
            Native.Stosb(ptr, 0, size);
            ptr -= sizeof(IDT.IDTStack);
            stack = (IDT.IDTStack*)ptr;

            stack->cs = 0x08;
            stack->rsp = (ulong)ptr;
            stack->rflags = 0x202;
        }
    }

    internal static unsafe class ThreadPool
    {
        public static List<Thread> Ts;
        public static bool Ready = false;

        public static void Initialize()
        {
            Ready = false;
            Ts = new();
            Add(&IdleThread);
            //dd(&A);
            //Add(&B);
            Add(&Program.KMain);
            Ready = true;
            //Make sure the irq wont be triggered during _iretq
            Native.Hlt();
            //_iretq((ulong)Ts[0].stack + 8);
            //_int20h();
            //for (; ; ) Native.Hlt();
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

        [DllImport("*")]
        public static extern void _iretq(ulong rsp);

        [DllImport("*")]
        public static extern void _int20h();

        public static void Add(delegate* <void> method)
        {
            Thread t = new();
            t.stack->rip = (ulong)method;
            Ts.Add(t);
        }

        public static int Index = 0;

        public static void Schedule(IDT.IDTStack* stack)
        {
            if (!Ready) return;

            //Native.Movsb(Ts[Index].stack, stack, 14 * 8);
            Native.Movsb(Ts[Index].stack, stack, (ulong)sizeof(IDT.IDTStack));
            Index = (Index + 1) % Ts.Count;

            Native.Movsb(stack, Ts[Index].stack, (ulong)sizeof(IDT.IDTStack));
        }
    }
}
