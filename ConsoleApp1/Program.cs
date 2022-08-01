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

        [DllImport("Reallocate")]
        public static extern nint Reallocate(nint intPtr, ulong size);

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

        [DllImport("DrawPoint")]
        public static extern void DrawPoint(int x, int y, uint color);

        [RuntimeExport("malloc")]
        public static nint malloc(ulong size) => Allocate(size);

        [DllImport("WriteString")]
        public static extern void WriteString(string s);

        [RuntimeExport("Main")]
        public static void Main()
        {
            WriteString("Content of Text.txt is: ");

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