using System.Runtime;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    static unsafe class Program
    {
        [RuntimeExport("Main")]
        public static void Main() 
        {
            byte* ptr = (byte*)0xb8000;
            ptr[0] = (byte)'S';
            ptr[1] = 0x0f;
            ptr[2] = (byte)'u';
            ptr[3] = 0x0f;
            ptr[4] = (byte)'c';
            ptr[5] = 0x0f;
            ptr[6] = (byte)'c';
            ptr[7] = 0x0f;
            ptr[8] = (byte)'e';
            ptr[9] = 0x0f;
            ptr[10] = (byte)'s';
            ptr[11] = 0x0f;
            ptr[12] = (byte)'s';
            ptr[13] = 0x0f;
        }
    }
}
