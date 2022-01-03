using System.Runtime.InteropServices;

public static class Native
{
    [DllImport("*")]
    public static extern void outb(ushort port, byte value);

    [DllImport("*")]
    public static extern void load_idt(ref IDT.IDTDescriptor idtr);

    [DllImport("*")]
    public static extern void _sti();

    [DllImport("*")]
    public static extern void _cli();
}