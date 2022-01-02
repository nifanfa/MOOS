using System.Runtime;

unsafe class Program
{
    [RuntimeExport("Main")]
    static void Main()
    {
        //You don't need to build. Just save and run Launcher
        Console.Setup();
        Console.Write('H');
        Console.Write('e');
        Console.Write('l');
        Console.Write('l');
        Console.Write('o');
        Console.Write(',');
        Console.Write(' ');
        Console.Write('W');
        Console.Write('o');
        Console.Write('r');
        Console.Write('l');
        Console.Write('d');
        Console.Write('!');
        for (; ; );
    }
}
