using System.Runtime;
using System.Runtime.InteropServices;
using System.Solution1;

namespace ConsoleApp1
{
    static unsafe class Program
    {
        public static void Main() { }

        [RuntimeExport("Main")]
        public static void Main(SystemTable* st) 
        {
            st->Console_WriteLine("Hello World From 1.EXE!\0");
        }
    }
}
