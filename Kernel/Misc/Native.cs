using System;
using System.Runtime.InteropServices;

/// <summary>
/// Warning: Try your best to not use(implement) c/c++ libs because it may cause triple fault like __movsb
/// </summary>
static unsafe class Native
{
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

    public static void Stosd(void* p, uint value, ulong count)
    {
        uint* pp = (uint*)p;
        for (ulong u = 0; u < count; u++) pp[u] = value;
    }

    public static unsafe void Stosb(void* p, byte value, ulong count) 
    {
        byte* pp = (byte*)p;
        for (ulong u = 0; u < count; u++) pp[u] = value;
    }

    public static void Movsb(void* dest, void* source, ulong count)
    {
        byte* pd = (byte*)dest;
        byte* ps = (byte*)source;
        for (ulong c = 0; c < count; c++) pd[c] = ps[c];
    }

    public static void Movsd(uint* dest, uint* source, ulong count)
    {
        uint* pd = (uint*)dest;
        uint* ps = (uint*)source;
        for (ulong c = 0; c < count; c++) pd[c] = ps[c];
    }

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
    public extern static void Nop();
}