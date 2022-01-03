using System.Runtime;

unsafe class Program
{
    [RuntimeExport("Main")]
    static void Main()
    {
        //You don't need to build. Just save and run Launcher
        Console.Setup();
        Console.Write('o');

        for (uint i = 512; i < 0x2000; i ++)
        {
            Allocator.AddFreePages((System.IntPtr)(i*4096), i-512);
        }

        Console.Write('k');

        IDT.Disable();
        //GDT.Initialise();
        IDT.Initialise();
        IDT.Enable();
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");
        for (; ; );
    }
}
