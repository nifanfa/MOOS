using System.Runtime;
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

        IDT.Disable();
        GDT.Initialise();
        IDT.Initialise();
        IDT.Enable();
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");

        TestClass[] tcs = new TestClass[2];
        tcs[0] = new TestClass();
        Console.WriteLine(tcs[0].ToString());

        for (; ; );
    }

    public class TestClass 
    {
        public string TestString;
    }
}
