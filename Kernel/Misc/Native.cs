using System;
using System.Runtime.InteropServices;


static unsafe class Native
{
    [DllImport("*")]
    public static extern void Insd(ushort Port, uint* Data, ulong Count);

    [DllImport("*")]
    public static extern void Outsd(ushort Port, uint* Data, ulong Count);

    [DllImport("*")]
    public static extern void Insw(ushort Port, ushort* Data, ulong Count);

    [DllImport("*")]
    public static extern void Outsw(ushort Port, ushort* Data, ulong Count);

    [DllImport("*")]
    public static extern void Insb(ushort Port, byte* Data, ulong Count);

    [DllImport("*")]
    public static extern void Outsb(ushort Port, byte* Data, ulong Count);

    [DllImport("*")]
    public static extern ulong ReadCR2();

    [DllImport("*")]
    public static extern void Out8(ushort port, byte value);

    [DllImport("*")]
    public static extern void Out16(ushort port, ushort value);

    [DllImport("*")]
    public static extern void Out32(ushort port, uint value);

    [DllImport("*")]
    public static extern byte In8(ushort port);

    [DllImport("*")]
    public static extern ushort In16(ushort port);

    [DllImport("*")]
    public static extern uint In32(ushort port);

    [DllImport("*")]
    public static extern void Stosd(void* p, uint value, ulong count);

    [DllImport("*")]
    public extern static unsafe void Stosb(void* p, byte value, ulong count);

    [DllImport("*")]
    public extern static void Movsd(uint* dest, uint* source, ulong count);

    [DllImport("*")]
    public static extern void Load_GDT(ref GDT.GDTDescriptor gdtr);

    [DllImport("*")]
    public static extern void Load_IDT(ref IDT.IDTDescriptor idtr);

    [DllImport("*")]
    public extern static unsafe void WriteCR3(ulong value);

    [DllImport("*")]
    public static extern void Hlt();

    [DllImport("*")]
    public static extern void Cli();

    [DllImport("*")]
    public static extern void Sti();

    [DllImport("*")]
    public extern static void Invlpg(ulong physicalAddress);

    [DllImport("*")]
    public extern static void Movsb(void* dest, void* source, ulong count);
}