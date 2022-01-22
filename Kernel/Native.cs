using System;
using System.Runtime.InteropServices;


static unsafe class Native
{
    [DllImport("*")]
    public static extern void outb(ushort port, byte value);

    [DllImport("*")]
    public static extern void outw(ushort port, ushort value);

    [DllImport("*")]
    public static extern byte inb(ushort port);

    [DllImport("*")]
    public static extern IntPtr kmalloc(ulong size);

    [DllImport("*")]
    public extern static unsafe void Stosb(void* p, byte value, ulong count);

    [DllImport("*")]
    public static extern IntPtr krealloc(IntPtr ptr, ulong newSize);

    [DllImport("*")]
    public static extern IntPtr kcalloc(ushort num, ushort size);

    [DllImport("*")]
    public static extern void kfree(IntPtr ptr);

    [DllImport("*")]
    public static extern void load_gdt(ref GDT.GDTDescriptor gdtr);

    [DllImport("*")]
    public static extern void load_idt(ref IDT.IDTDescriptor idtr);

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