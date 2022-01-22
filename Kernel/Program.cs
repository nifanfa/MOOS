using ConsoleApp1;
using Kernel;
using System.Runtime.InteropServices;

unsafe class Program
{
    static void Main() { }

    [UnmanagedCallersOnly(EntryPoint = "kernel_main", CallingConvention = CallingConvention.StdCall)]
    static void kernel_main()
    {
        //You don't need to build. Just save and run Launcher
        Console.Setup();

        //                 10MiB                 512MiB                1MiB
        for (uint i = 1024 * 1024 * 10; i < 1024 * 1024 * 512; i += 1024 * 1024)
        {
            //                                      1MiB / 4KiB
            Allocator.AddFreePages((System.IntPtr)(i), 256);
        }
        Serial.WriteLine("Hello World");
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        /*
        IDT.Disable();
        GDT.Initialise();
        IDT.Initialise();
        IDT.Enable();
        Serial.Initialise();
        PageTable.Initialise();
        */

        /*
        BGA.Setup();
        BGA.SetVideoMode(640, 480);
        BGA.Clear(0xFFFF0000);
        */

        for (; ; );
    }

    public class TestClass
    {
        public string TestString;
    }
}
