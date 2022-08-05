global using NES;
using Internal.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;

namespace NES
{
    public static unsafe class Program
    {
        [RuntimeExport("malloc")]
        public static nint malloc(ulong size) => Allocate(size);

        [DllImport("Allocate")]
        public static extern nint Allocate(ulong size);

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

        [DllImport("CreateWindow")]
        public static extern IntPtr CreateWindow(int X, int Y, int Width, int Height, string Title);

        [DllImport("GetWindowScreenBuf")]
        public static extern IntPtr GetWindowScreenBuf(IntPtr handle);

        [DllImport("BindOnKeyChangedHandler")]
        public static extern void BindOnKeyChangedHandler(IntPtr handler);

        public static Image ScreenBuf;

        [RuntimeExport("Main")]
        public static void Main()
        {
            var handle = CreateWindow(300, 350, 256, 240, "NES Portable");
            var screenBufHandle = GetWindowScreenBuf(handle);
            ScreenBuf = Unsafe.As<IntPtr, Image>(ref screenBufHandle);

            ReadAllBytes("Super Mario.nes", out var size, out var data);
            NES nes = new NES();
            nes.openROM(data);
            for(; ; ) 
            {
                nes.runGame();
            }
        }
    }
}