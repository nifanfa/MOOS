using Internal.Runtime.CompilerServices;
using Kernel;
#if PLATFORM_KERNEL
using Kernel.Driver;
using Kernel.NET;
#endif
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
        while (true) ;
    }

    [RuntimeExport("irq_handler")]
    public static void IRQHandler(int irq) 
    {
        irq += 0x20;
#if PLATFORM_KERNEL
        switch (irq)
        {
            case 0x20:
                PIT.OnInterrupt();
                break;
            case 0x21:
                byte b = Native.In8(0x60);
                char c = PS2Keyboard.ProcessKey(b);
                PS2Keyboard.Key = c;
                if (c == '\n') Console.WriteLine();
                else if (c == '\b') Console.Back();
                else if (c != '?') Console.Write(c);
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
        PIC.EndOfInterrupt(irq);
#endif
    }
}