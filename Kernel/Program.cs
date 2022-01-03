using System.Runtime;

unsafe class Program
{
    [RuntimeExport("Main")]
    static void Main()
    {
        for (uint i = 0; i < 0x40000; i ++)
        {
            Allocator.AddFreePages((System.IntPtr)((1024*1024)+(i*4096)), i);
        }

        IDT.Disable();
        //GDT.Initialise();
        IDT.Initialise();
        IDT.Enable();

        //You don't need to build. Just save and run Launcher
        Console.Setup();
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");
        for (; ; );
    }
}
