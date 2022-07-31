using System.Runtime;
using System.Runtime.InteropServices;
using Internal.Runtime.CompilerServices;
using MOOS;
using MOOS.Driver;
using MOOS.Misc;
using static Internal.Runtime.CompilerHelpers.InteropHelpers;

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
    public static IDTDescriptor idtr;


    public static bool Initialized { get; private set; }


    public static unsafe bool Initialize()
    {
        idt = new IDTEntry[256];

        set_idt_entries(Unsafe.AsPointer(ref idt[0]));

        fixed (IDTEntry* _idt = idt)
        {
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

    //interrupts_asm.asm line 39
    [RuntimeExport("exception_handler")]
    public static unsafe void ExceptionHandler(int code, IDTStackGeneric* stack)
    {
        Panic.Error($"Kernel panic on CPU {SMP.ThisCPU}", true);
        InterruptReturnStack* irs = code switch
        {
            8 or 10 or 11 or 12 or 13 or 14 or 17 or 21 or 29 or 30 => (InterruptReturnStack*)(((byte*)stack) + sizeof(RegistersStack)),
            _ => (InterruptReturnStack*)(((byte*)stack) + sizeof(RegistersStack) + sizeof(ulong)),
        };
        Console.WriteLine($"RIP: 0x{stack->irs.rip.ToString("x2")}");
        Console.WriteLine($"Code Segment: 0x{stack->irs.cs.ToString("x2")}");
        Console.WriteLine($"RFlags: 0x{stack->irs.rflags.ToString("x2")}");
        Console.WriteLine($"RSP: 0x{stack->irs.rsp.ToString("x2")}");
        Console.WriteLine($"Stack Segment: 0x{stack->irs.ss.ToString("x2")}");
        string description = code switch
        {
            0 => "DIVIDE BY ZERO",
            1 => "SINGLE STEP",
            2 => "NMI",
            3 => "BREAKPOINT",
            4 => "OVERFLOW",
            5 => "BOUNDS CHECK",
            6 => "INVALID OPCODE",
            7 => "COPR UNAVAILABLE",
            8 => "DOUBLE FAULT",
            9 => "COPR SEGMENT OVERRUN",
            10 => "INVALID TSS",
            11 => "SEGMENT NOT FOUND",
            12 => "STACK EXCEPTION",
            13 => "GENERAL PROTECTION",
            14 => (Native.ReadCR2() >> 5) < 0x1000 ? "NULL POINTER" : "PAGE FAULT",
            16 => "COPR ERROR",
            _ => "UNKNOWN"
        };
        Console.WriteLine($"Cause: {description}");
        Framebuffer.Update();
        for (; ; )
        {

        }
    }

    public struct RegistersStack
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
    }

    //https://os.phil-opp.com/returning-from-exceptions/
    public struct InterruptReturnStack
    {
        public ulong rip;
        public ulong cs;
        public ulong rflags;
        public ulong rsp;
        public ulong ss;
    }

    public struct IDTStackGeneric
    {
        public RegistersStack rs;
        public ulong errorCode;
        public InterruptReturnStack irs;
    }

    [RuntimeExport("irq_handler")]
    public static unsafe void IRQHandler(int irq, IDTStackGeneric* stack)
    {
        //DEAD
        if (irq == 0xFD)
        {
            Native.Cli();
            Native.Hlt();
            for (; ; )
            {
                Native.Hlt();
            }
        }

        //For main processor
        if (SMP.ThisCPU == 0)
        {
            //System calls
            if (irq == 0x80)
            {
                MethodFixupCell* pCell = (MethodFixupCell*)stack->rs.rcx;
                string name = string.FromASCII(pCell->Module->ModuleName, strings.strlen((byte*)pCell->Module->ModuleName));
                stack->rs.rax = (ulong)API.HandleSystemCall(name);
                name.Dispose();
            }
            switch (irq)
            {
                case 0x20:
                    //misc.asm Schedule_Next
                    if (stack->rs.rdx != 0x61666E6166696E)
                    {
                        Timer.OnInterrupt();
                    }

                    break;
            }
            Interrupts.HandleInterrupt(irq);
        }

        if (irq == 0x20)
        {
            ThreadPool.Schedule(stack);
        }

        Interrupts.EndOfInterrupt((byte)irq);
    }
}