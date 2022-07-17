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

        [DllImport("Allocate")]
        public static extern nint Allocate(ulong size);

        [DllImport("Free")]
        public static extern ulong Free(nint ptr);

        [DllImport("GetTick")]
        public static extern uint GetTick();

        [DllImport("Sleep")]
        public static extern void Sleep(ulong ms);

        [RuntimeExport("Main")]
        public static void Main()
        {
            WriteLine("Hello From ConsoleApp1.exe");
        }
    }
}