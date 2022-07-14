using MOOS;
using MOOS.Misc;
using System.Runtime;

unsafe class Program
{
    internal static PNG Wallpaper;

    static void Main() { }

    /*
     * Minimum system requirement:
     * 1024MiB of RAM
     * Memory Map:
     * 256 MiB - 512MiB   -> System
     * 512 MiB - ∞     -> Free to use
     */
    //Check out Kernel/Misc/EntryPoint.cs
    [RuntimeExport("KMain")]
    static void KMain() 
    {
        Console.Clear();
        Console.WriteLine("Now you are in MOOS-ConsoleOS!");
        for(; ; ) 
        {
            string s = Console.ReadLine();
            Console.WriteLine(s);
        }
    }
}