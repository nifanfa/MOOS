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
        [DllImport("WriteLine")]
        public static extern void WriteLine();

        [DllImport("Allocate")]
        public static extern nint Allocate(ulong size);

        [DllImport("Free")]
        public static extern ulong Free(nint ptr);

        [DllImport("GetTick")]
        public static extern uint GetTick();

        [DllImport("Sleep")]
        public static extern void Sleep(ulong ms);

        [DllImport("ReadAllBytes")]
        public static extern void ReadAllBytes(string name, out ulong size, out byte* data);

        [DllImport("Write")]
        public static extern void Write(char c);

        [DllImport("SwitchToConsoleMode")]
        public static extern void SwitchToConsoleMode();

        [RuntimeExport("Main")]
        public static void Main()
        {
            SwitchToConsoleMode();

            Write('C');
            Write('o');
            Write('n');
            Write('t');
            Write('e');
            Write('n');
            Write('t');
            Write(' ');
            Write('o');
            Write('f');
            Write(' ');
            Write('T');
            Write('e');
            Write('x');
            Write('t');
            Write('.');
            Write('t');
            Write('x');
            Write('t');
            Write(':');

            ReadAllBytes("Text.txt", out var size, out var data);
            for(ulong i = 0; i < size; i++) 
            {
                Write((char)data[i]);
            }
            WriteLine();

            for (; ; );
        }
    }
}