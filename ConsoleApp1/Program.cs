using System.Runtime;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    static unsafe class Program
    {
        [DllImport("SayHello")]
        public static extern void SayHello();

        //Check out
        //Kernel.API
        //Internal.Runtime.CompilerHelpers.InteropHelpers
        [DllImport("Console.WriteLine")]
        public static extern void WriteLine(string s);

        [RuntimeExport("Main")]
        public static void Main()
        {
            WriteLine("Hello From ConsoleApp1.exe");
        }
    }
}