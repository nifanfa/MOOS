using System.Runtime;

unsafe class Program
{
    [RuntimeExport("Main")]
    static void Main()
    {
        //You don't need to build. Just save and run Launcher
        Console.Setup();
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Use Native AOT (Core RT) Technology.");
        for (; ; );
    }
}
