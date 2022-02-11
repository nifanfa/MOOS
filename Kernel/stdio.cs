using System.Runtime;

namespace Kernel
{
    internal unsafe class stdio
    {
        [RuntimeExport("printf")]
        public static void printf(byte* msg) 
        {
            byte b;
            while((b = *msg++) != 0) 
            {
                if (b == '\n') Console.WriteLine();
                else Console.Write((char)b);
            }
        }
    }
}
