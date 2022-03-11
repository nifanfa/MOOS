// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System.Runtime.InteropServices;

internal static unsafe class Native
{
    [DllImport("*")]
    public static extern void Insb(ushort port, byte* data, ulong count);

    [DllImport("*")]
    public static extern void Insw(ushort port, ushort* data, ulong count);

    [DllImport("*")]
    public static extern void Insd(ushort port, uint* data, ulong count);

    [DllImport("*")]
    public static extern void Outsb(ushort port, byte* data, ulong count);

    [DllImport("*")]
    public static extern void Outsw(ushort port, ushort* data, ulong count);

    [DllImport("*")]
    public static extern void Outsd(ushort port, uint* data, ulong count);

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
    public static extern unsafe void Stosb(void* p, byte value, ulong count);

    [DllImport("*")]
    public static extern void Movsb(void* dest, void* source, ulong count);

    [DllImport("*")]
    public static extern void Movsd(uint* dest, uint* source, ulong count);

    [DllImport("*")]
    public static extern void Load_GDT(ref GDT.GDTDescriptor gdtr);

    [DllImport("*")]
    public static extern void Load_IDT(ref IDT.IDTDescriptor idtr);

    [DllImport("*")]
    public static extern unsafe void WriteCR3(ulong value);

    [DllImport("*")]
    public static extern void Hlt();

    [DllImport("*")]
    public static extern void Cli();

    [DllImport("*")]
    public static extern void Sti();

    [DllImport("*")]
    public static extern void Invlpg(ulong physicalAddress);

    [DllImport("*")]
    public static extern void Nop();
}
