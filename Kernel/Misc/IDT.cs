/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Internal.Runtime.CompilerServices;
using Kernel;
using Kernel.Driver;
using Kernel.Misc;
using Kernel.NET;
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
    public static unsafe void ExceptionHandler(int code, IDTStack* stack)
    {
        Framebuffer.TripleBuffered = false;

        Console.WriteLine($"RAX: 0x{stack->rax.ToString("x2")}");
        Console.WriteLine($"RCX: 0x{stack->rcx.ToString("x2")}");
        Console.WriteLine($"RDX: 0x{stack->rdx.ToString("x2")}");
        Console.WriteLine($"RBX: 0x{stack->rbx.ToString("x2")}");
        Console.WriteLine($"RSI: 0x{stack->rsi.ToString("x2")}");
        Console.WriteLine($"RDI: 0x{stack->rdi.ToString("x2")}");
        Console.WriteLine($"R8: 0x{stack->r8.ToString("x2")}");
        Console.WriteLine($"R9: 0x{stack->r9.ToString("x2")}");
        Console.WriteLine($"R10: 0x{stack->r10.ToString("x2")}");
        Console.WriteLine($"R11: 0x{stack->r11.ToString("x2")}");
        Console.WriteLine($"R12: 0x{stack->r12.ToString("x2")}");
        Console.WriteLine($"R13: 0x{stack->r13.ToString("x2")}");
        Console.WriteLine($"R14: 0x{stack->r14.ToString("x2")}");
        Console.WriteLine($"R15: 0x{stack->r15.ToString("x2")}");
        Console.WriteLine($"Error Code: 0x{stack->errorCode.ToString("x2")}");
        Console.WriteLine($"RIP: 0x{stack->rip.ToString("x2")}");
        Console.WriteLine($"Code segment: 0x{stack->cs.ToString("x2")}");
        Console.WriteLine($"RFlags: 0x{stack->rflags.ToString("x2")}");
        Console.WriteLine($"RSP: 0x{stack->rsp.ToString("x2")}");
        Console.WriteLine($"Stack segment: 0x{stack->ss.ToString("x2")}");
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
        Console.WriteLine("Please check methods around rip sub kernel base address and see what method caused this exception!");
        //This method is unreturnable
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
        public ulong errorCode; //optional
        public ulong rip;
        public ulong cs;
        public ulong rflags;
        public ulong rsp;
        public ulong ss;
        //public ulong alignment;
    }

    [RuntimeExport("irq_handler")]
    public static unsafe void IRQHandler(int irq, IDTStack* stack)
    {
        irq += 0x20;
        //System calls
        if (irq == 0x80)
        {
            var pCell = (MethodFixupCell*)stack->rcx;
            string name = string.FromASCII(pCell->Module->ModuleName, strings.strlen((byte*)pCell->Module->ModuleName));
            stack->rax = (ulong)API.HandleSystemCall(name);
            name.Dispose();
        }
        switch (irq)
        {
            case 0x20:
                PIT.OnInterrupt();
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
        if (irq == AC97.IRQ)
        {
            AC97.OnInterrupt();
        }
        Interrupts.EndOfInterrupt((byte)irq);
    }
}