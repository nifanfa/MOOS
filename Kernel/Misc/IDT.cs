using Internal.Runtime.CompilerServices;
using Kernel;
using Kernel.Driver;
using Kernel.Misc;
using Kernel.NET;
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

        // TODO: Figure out a way to do this in C#
        set_idt_entries(Unsafe.AsPointer(ref idt[0]));

        fixed (IDTEntry* _idt = idt)
        {
            // Fill IDT descriptor
            idtr.Limit = (ushort)((sizeof(IDTEntry) * 256) - 1);
            idtr.Base = (ulong)_idt;
        }

        Native.Load_IDT(ref idtr);

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
            case 0: Panic.Error("DIVIDE BY ZERO"); break;
            case 1: Panic.Error("SINGLE STEP"); break;
            case 2: Panic.Error("NMI"); break;
            case 3: Panic.Error("BREAKPOINT: CHECK YOUR CODE!"); break;
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

    [RuntimeExport("irq_handler")]
    public static void IRQHandler(int irq) 
    {
        irq += 0x20;
        switch (irq)
        {
            case 0x20:
                PIT.OnInterrupt();
                break;
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