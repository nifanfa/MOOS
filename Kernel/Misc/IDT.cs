using Internal.Runtime.CompilerServices;
using Kernel;
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
            case 0: Console.WriteLine("Divide-by-zero Error"); break;
            case 1: Console.WriteLine("Debug"); break;
            case 2: Console.WriteLine("Non-maskable Interrupt"); break;
            case 3: Console.WriteLine("Breakpoint"); break;
            case 4: Console.WriteLine("Overflow"); break;
            case 5: Console.WriteLine("Bound Range Exceeded"); break;
            case 6: Console.WriteLine("Invalid Opcode"); break;
            case 7: Console.WriteLine("Device Not Available"); break;
            case 8: Console.WriteLine("Double Fault"); break;
            case 9: Console.WriteLine("Coprocessor Segment Overrun"); break;
            case 10: Console.WriteLine("Invalid TSS"); break;
            case 11: Console.WriteLine("Segment Not Present"); break;
            case 12: Console.WriteLine("Stack-Segment Fault"); break;
            case 13: Console.WriteLine("Stack-Segment Fault"); break;
            case 14:
                ulong CR2 = Native.ReadCR2();
                if ((CR2 >> 5) < 0x1000)
                {
                    Console.WriteLine("Null Pointer");
                }
                else
                {
                    Console.WriteLine("Page Fault");
                }
                break;
            case 16: Console.WriteLine("x87 Floating-Point Exception"); break;
            case 17: Console.WriteLine("Alignment Check"); break;
            case 18: Console.WriteLine("Machine Check"); break;
            case 19: Console.WriteLine("SIMD Floating-Point Exception"); break;
            case 20: Console.WriteLine("Virtualization Exception"); break;
            case 21: Console.WriteLine("Control Protection Exception"); break;
            case 28: Console.WriteLine("Hypervisor Injection Exception"); break;
            case 29: Console.WriteLine("VMM Communication Exception"); break;
            case 30: Console.WriteLine("Security Exception"); break;
            default: Console.WriteLine("Unknown Exception"); break;
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
                char c = PS2Keyboard.ProcessKey(b);
                if (c == '\n') Console.WriteLine();
                else if (c == '\b') Console.Back();
                else if (c != '?') Console.Write(c);
                break;
            case 0x2B:
                if (RTL8139.IRQ == 0x2B)
                {
                    RTL8139.OnInterrupt();
                }
                break;
            case 0x2C:
                PS2Mouse.OnInterrupt();
                break;
        }
        PIC.EndOfInterrupt(irq);
    }
}