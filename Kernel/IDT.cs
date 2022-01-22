using Internal.Runtime.CompilerServices;
using Kernel;
using System.Runtime;
using System.Runtime.InteropServices;

public static class IDT
{
    [DllImport("*")]
    static unsafe extern void set_idt_entries(void* idt);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct IDTEntry
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


    static IDTEntry[] idt;
    static IDTDescriptor idtr;


    public static bool Initialised { get; private set; }


    public unsafe static void Initialise()
    {
        idt = new IDTEntry[256];

        PIC.Enable();

        // TODO: Figure out a way to do this in C#
        set_idt_entries(Unsafe.AsPointer(ref idt[0]));

        fixed (IDTEntry* _idt = idt)
        {
            // Fill IDT descriptor
            idtr.Limit = (ushort)((sizeof(IDTEntry) * 256) - 1);
            idtr.Base = (ulong)_idt;
        }

        Native.Load_IDT(ref idtr);

        //Enable keyboard interrupts
        PIC.ClearMask(0x21);

        Initialised = true;
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
            case 0: Console.WriteLine("DIVIDE BY ZERO"); break;
            case 1: Console.WriteLine("SINGLE STEP"); break;
            case 2: Console.WriteLine("NMI"); break;
            case 3: Console.WriteLine("BREAKPOINT"); break;
            case 4: Console.WriteLine("OVERFLOW"); break;
            case 5: Console.WriteLine("BOUNDS CHECK"); break;
            case 6: Console.WriteLine("INVALID OPCODE"); break;
            case 7: Console.WriteLine("COPR UNAVAILABLE"); break;
            case 8: Console.WriteLine("DOUBLE FAULT"); break;
            case 9: Console.WriteLine("COPR SEGMENT OVERRUN"); break;
            case 10: Console.WriteLine("INVALID TSS"); break;
            case 11: Console.WriteLine("SEGMENT NOT FOUND"); break;
            case 12: Console.WriteLine("STACK EXCEPTION"); break;
            case 13: Console.WriteLine("GENERAL PROTECTION"); break;
            case 14:
                ulong CR2 = Native.ReadCR2();
                if ((CR2 >> 5) < 0x1000)
                {
                    Console.WriteLine("NULL POINTER");
                }
                else
                {
                    Console.WriteLine("PAGE FAULT");
                }
                break;
            case 16: Console.WriteLine("COPR ERROR"); break;
            default: Console.WriteLine(" UNKNOWN EXCEPTION"); break;
        }
    }

    [RuntimeExport("irq0_handler")]
    public static void IRQ0Handler()
    {
        PIT.OnInterrupt();
        PIC.EndOfInterrupt(0x20);
    }

    [RuntimeExport("irq1_handler")]
    public static void IRQ1Handler()
    {
        byte b = Native.In8(0x60);
        char c = PS2Keyboard.ProcessKey(b);
        if (c != '?') Console.Write(c);
        PIC.EndOfInterrupt(0x21);
    }

    [RuntimeExport("irq2_handler")]
    public static void IRQ2Handler()
    {
        Console.WriteLine("IRQ 2");
        PIC.EndOfInterrupt(0x22);
    }

    [RuntimeExport("irq3_handler")]
    public static void IRQ3Handler()
    {
        Console.WriteLine("IRQ 3");
        PIC.EndOfInterrupt(0x23);
    }

    [RuntimeExport("irq4_handler")]
    public static void IRQ4Handler()
    {
        Console.WriteLine("IRQ 4");
        PIC.EndOfInterrupt(0x24);
    }

    [RuntimeExport("irq5_handler")]
    public static void IRQ5Handler()
    {
        Console.WriteLine("IRQ 5");
        PIC.EndOfInterrupt(0x25);
    }

    [RuntimeExport("irq6_handler")]
    public static void IRQ6Handler()
    {
        Console.WriteLine("IRQ 6");
        PIC.EndOfInterrupt(0x26);
    }

    [RuntimeExport("irq7_handler")]
    public static void IRQ7Handler()
    {
        Console.WriteLine("IRQ 7");
        PIC.EndOfInterrupt(0x27);
    }

    [RuntimeExport("irq8_handler")]
    public static void IRQ8Handler()
    {
        Console.WriteLine("IRQ 8");
        PIC.EndOfInterrupt(0x28);
    }

    [RuntimeExport("irq9_handler")]
    public static void IRQ9Handler()
    {
        Console.WriteLine("IRQ 9");
        PIC.EndOfInterrupt(0x29);
    }

    [RuntimeExport("irq10_handler")]
    public static void IRQ10Handler()
    {
        Console.WriteLine("IRQ 10");
        PIC.EndOfInterrupt(0x2A);
    }

    [RuntimeExport("irq11_handler")]
    public static void IRQ11Handler()
    {
        Console.WriteLine("IRQ 11");
        PIC.EndOfInterrupt(0x2B);
    }

    [RuntimeExport("irq12_handler")]
    public static void IRQ12Handler()
    {
        PS2Mouse.OnInterrupt();
        PIC.EndOfInterrupt(0x2C);
    }

    [RuntimeExport("irq13_handler")]
    public static void IRQ13Handler()
    {
        Console.WriteLine("IRQ 13");
        PIC.EndOfInterrupt(0x2D);
    }

    [RuntimeExport("irq14_handler")]
    public static void IRQ14Handler()
    {
        Console.WriteLine("IRQ 14");
        PIC.EndOfInterrupt(0x2E);
    }

    [RuntimeExport("irq15_handler")]
    public static void IRQ15Handler()
    {
        Console.WriteLine("IRQ 15");
        PIC.EndOfInterrupt(0x2F);
    }
}