global using NES;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;

namespace NES
{
    public static unsafe class Program
    {
        //Warning: This program doesn't have input support!

        [RuntimeExport("malloc")]
        public static nint malloc(ulong size) => Allocate(size);

        [DllImport("Allocate")]
        public static extern nint Allocate(ulong size);

        [DllImport("SwitchToConsoleMode")]
        public static extern void SwitchToConsoleMode();

        [DllImport("DrawPoint")]
        public static extern void DrawPoint(int x, int y, uint color);

        [DllImport("ReadAllBytes")]
        public static extern void ReadAllBytes(string name, out ulong size, out byte* data);

        [DllImport("Lock")]
        public static extern void ALock();

        [DllImport("Unlock")]
        public static extern void AUnlock();

        [RuntimeExport("Lock")]
        public static void Lock() => ALock();

        [RuntimeExport("Unlock")]
        public static void Unlock() => AUnlock();

        [DllImport("DebugWrite")]
        public static extern void ADebugWrite(char c);

        [DllImport("DebugWriteLine")]
        public static extern void ADebugWriteLine();

        [RuntimeExport("DebugWrite")]
        public static void DebugWrite(char c) => ADebugWrite(c);

        [RuntimeExport("DebugWriteLine")]
        public static void DebugWriteLine() => ADebugWriteLine();

        [DllImport("Free")]
        public static extern ulong AFree(nint ptr);

        [RuntimeExport("free")]
        public static ulong free(nint ptr) => AFree(ptr);

        [DllImport("Clear")]
        public static extern void Clear(uint color);

        [DllImport("Update")]
        public static extern void Update();

        [RuntimeExport("Main")]
        public static void Main()
        {
            ReadAllBytes("Super Mario.nes", out var size, out var data);
            NES nes = new NES();
            nes.openROM(data);
            for(; ; ) 
            {
                //Debug.WriteLine("Running");
                nes.runGame();
            }
        }
    }
}