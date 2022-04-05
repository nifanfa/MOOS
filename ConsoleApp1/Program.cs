using System.Runtime;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    static unsafe class Program
    {
        [DllImport("SayHello")]
        public static extern void SayHello();

        [DllImport("*")]
        public static extern ulong _int80h(ulong p1);

        [RuntimeExport("Main")]
        public static void Main()
        {
            SayHello();
        }
    }
}
