/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;
using Internal.Runtime.CompilerServices;
using OS_Sharp;
using OS_Sharp.Driver;
using OS_Sharp.Misc;
using OS_Sharp.Networking;

public static class IDT
{
    [DllImport("*")]
    private static extern unsafe void set_idt_entries(void* idt);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct IDTEntry
    {
        public ushort BaseLow;
        public ushort Selector;
        public byte Reserved0;
        public byte Type_Attributes;
        public ushort BaseMid;
        public uint BaseHigh;
        public uint Reserved1;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IDTDescriptor
    {
        public ushort Limit;
        public ulong Base;
    }

    private static IDTEntry[] idt;
    private static IDTDescriptor idtr;


    public static bool Initialized { get; private set; }


    public static unsafe bool Initialize()
    {
        idt = new IDTEntry[256];

        // TODO: Figure out a way to do this in C#
        set_idt_entries(Unsafe.AsPointer(ref idt[0]));

        fixed (IDTEntry* _idt = idt)
        {
            // Fill IDT descriptor
            idtr.Limit = (ushort)((sizeof(IDTEntry) * 256) - 1);
            idtr.Base = (ulong)_idt;
        }

        Native.Load_IDT(ref idtr);

        Initialized = true;
        return true;
    }

    public static void Enable()
    {
        Native.Sti();
    }

    public static void Disable()
    {
        Native.Cli();
    }


    [RuntimeExport("exception_handler")]
    public static void ExceptionHandler(int code)
    {
        switch (code)
        {
            case 0: Panic.Error("DIVIDE BY ZERO"); break;
            case 1: Panic.Error("SINGLE STEP"); break;
            case 2: Panic.Error("NMI"); break;
            case 3: Panic.Error("BREAKPOINT"); break;
            case 4: Panic.Error("OVERFLOW"); break;
            case 5: Panic.Error("BOUNDS CHECK"); break;
            case 6: Panic.Error("INVALID OPCODE"); break;
            case 7: Panic.Error("COPR UNAVAILABLE"); break;
            case 8: Panic.Error("DOUBLE FAULT"); break;
            case 9: Panic.Error("COPR SEGMENT OVERRUN"); break;
            case 10: Panic.Error("INVALID TSS"); break;
            case 11: Panic.Error("SEGMENT NOT FOUND"); break;
            case 12: Panic.Error("STACK EXCEPTION"); break;
            case 13: Panic.Error("GENERAL PROTECTION"); break;
            case 14:
                ulong CR2 = Native.ReadCR2();
                if ((CR2 >> 5) < 0x1000)
                {
                    Panic.Error("NULL POINTER");
                }
                else
                {
                    Panic.Error("PAGE FAULT");
                }
                break;
            case 16: Panic.Error("COPR ERROR"); break;
            default: Panic.Error(" UNKNOWN EXCEPTION"); break;
        }
    }

    public struct IDTStack
    {
        public ulong rax;
        public ulong rcx;
        public ulong rdx;
        public ulong rbx;
        public ulong rsi;
        public ulong rdi;
        public ulong r8;
        public ulong r9;
        public ulong r10;
        public ulong r11;
        public ulong r12;
        public ulong r13;
        public ulong r14;
        public ulong r15;

        //https://os.phil-opp.com/returning-from-exceptions/
        public ulong errorCode;
        public ulong rip;
        public ulong cs;
        public ulong rflags;
        public ulong rsp;
        public ulong ss;
        //public ulong alignment;
    }

    [RuntimeExport("irq_handler")]
    public static unsafe void IRQHandler(int irq, ulong rsp)
    {
        irq += 0x20;
        IDTStack* stack = (IDTStack*)rsp;
        if (irq == 0x80)
        {
            Console.Write("rax: 0x");
            Console.WriteLine(stack->rax.ToString("x2"));
            Console.Write("rip: 0x");
            Console.WriteLine(stack->rip.ToString("x2"));
            Console.Write("ss: 0x");
            Console.WriteLine(stack->ss.ToString("x2"));
            while (true)
            {
                ;
            }
        }
        switch (irq)
        {
            case 0x20:
                PIT.OnInterrupt();
                ThreadPool.Schedule(stack);
                LocalAPIC.EndOfInterrupt();
                return;
            case 0x21:
                byte b = Native.In8(0x60);
                PS2Keyboard.ProcessKey(b);
                break;
            case 0x2C:
                PS2Mouse.OnInterrupt();
                break;
        }
        if (irq == RTL8139.IRQ)
        {
            RTL8139.OnInterrupt();
        }
        if (irq == Intel8254X.IRQ)
        {
            Intel8254X.OnInterrupt();
        }
        if (irq == AC97.IRQ)
        {
            AC97.OnInterrupt();
        }
        LocalAPIC.EndOfInterrupt();
    }
}
