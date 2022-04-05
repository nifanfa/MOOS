using System.Runtime;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    static unsafe class Program
    {
        [RuntimeExport("Main")]
        public static void Main() 
        {
            ulong* ptr = (ulong*)0xb8000;
            *ptr = 0x2f592f412f4b2f4f;
        }
    }
}
