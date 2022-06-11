/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Internal.Runtime.CompilerServices;
using MOOS;
using MOOS.Driver;
using MOOS.Misc;
using MOOS.NET;
using System.Runtime;
using System.Runtime.InteropServices;
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

    //interrupts_asm.asm line 39
    [RuntimeExport("exception_handler")]
    public static unsafe void ExceptionHandler(int code, IDTStackGeneric* stack)
    {
        Panic.Error($"CPU{SMP.ThisCPU} KERNEL PANIC!!!", true);
        InterruptReturnStack* irs;
        switch (code) 
        {
            case 0x00:
            case 0x01:
            case 0x02:
            case 0x03:
            case 0x04:
            case 0x05:
            case 0x06:
            case 0x07:
            case 0x09:
            case 0x0F:
            case 0x10:
            case 0x12:
            case 0x13:
            case 0x14:
            case 0x16:
            case 0x17:
            case 0x18:
            case 0x19:
            case 0x1A:
            case 0x1B:
            case 0x1C:
            case 0x1F:
                irs = (InterruptReturnStack*)(((byte*)stack) + sizeof(RegistersStack));
                break;

            default:
                irs = (InterruptReturnStack*)(((byte*)stack) + sizeof(RegistersStack) + sizeof(ulong));
                break;
        }
        Console.WriteLine($"RIP: 0x{stack->irs.rip.ToString("x2")}");
        Console.WriteLine($"Code Segment: 0x{stack->irs.cs.ToString("x2")}");
        Console.WriteLine($"RFlags: 0x{stack->irs.rflags.ToString("x2")}");
        Console.WriteLine($"RSP: 0x{stack->irs.rsp.ToString("x2")}");
        Console.WriteLine($"Stack Segment: 0x{stack->irs.ss.ToString("x2")}");
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
            default: Console.WriteLine("UNKNOWN EXCEPTION"); break;
        }
        Framebuffer.Update();
        //This method is unreturnable
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
        if(irq == 0xFD) 
        {
            Native.Cli();
            Native.Hlt();
            for (; ; ) Native.Hlt();
        }

        //System.Threading.Monitor.Enter(object obj)
        if (irq == 0xFE)
        {
            while (ThreadPool.Locked) Native._nop();
        }

        //For main processor
        if (SMP.ThisCPU == 0)
        {
            //System calls
            if (irq == 0x80)
            {
                var pCell = (MethodFixupCell*)stack->rs.rcx;
                string name = string.FromASCII(pCell->Module->ModuleName, strings.strlen((byte*)pCell->Module->ModuleName));
                stack->rs.rax = (ulong)API.HandleSystemCall(name);
                name.Dispose();
            }
            switch (irq)
            {
                case 0x20:
                    Timer.OnInterrupt();
                    ThreadPool.Schedule(stack);
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
            Interrupts.HandleInterrupt(irq);
        }
        Interrupts.EndOfInterrupt((byte)irq);
    }
}