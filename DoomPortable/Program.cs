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

        [DllImport("GetTick")]
        public static extern uint GetTick();

        [DllImport("Reallocate")]
        public static extern nint Reallocate(nint intPtr, ulong size);

        [DllImport("Calloc")]
        public static extern void* Calloc(ulong num, ulong size);

        [DllImport("Write")]
        public static extern void Write(char c);

        [DllImport("Sleep")]
        public static extern void Sleep(ulong ms);

        [RuntimeExport("Main")]
        public static void Main()
        {
            var handle = CreateWindow(200, 300, 640, 400, "Doom Portable");
            var screenBufHandle = GetWindowScreenBuf(handle);
            ScreenBuf = Unsafe.As<IntPtr, Image>(ref screenBufHandle);

            ReadAllBytes("DOOM1.WAD", out var size, out var data);
            new Doom(data, (long)size);
        }
    }

    internal unsafe class Doom
    {
        #region Doom

        [RuntimeExport("_putchar")]
        public static void _putchar(char c) => Program.Write(c);

        [RuntimeExport("kcalloc")]
        public static void kcalloc(ulong num, ulong size) => Program.Calloc(num, size);

        [RuntimeExport("krealloc")]
        public static nint krealloc(nint intPtr, ulong size) => Program.Reallocate(intPtr, size);

        [RuntimeExport("kmalloc")]
        public static nint kmalloc(ulong size) => Program.Allocate(size);

        [RuntimeExport("kfree")]
        public static ulong kfree(nint ptr) => Program.AFree(ptr);

        [DllImport("*")]
        public static extern int doommain(byte* gb, long gl);

        [RuntimeExport("GetTickCount")]
        public static uint GetTickCount()
        {
            return Program.GetTick();
        }

        [RuntimeExport("Sleep")]
        public static void Sleep(uint ms)
        {
            Program.Sleep(ms);
        }

        [RuntimeExport("DrawPoint")]
        public static void DrawPoint(int x, int y, uint color)
        {
            Program.ScreenBuf.RawData[Program.ScreenBuf.Width * y + x] = (int)color;
        }

        [DllImport("SndWrite")]
        public static extern int SndWrite(byte* buffer, int len);

        [RuntimeExport("snd_write")]
        public static int snd_write(byte* buffer, int len) => SndWrite(buffer, len);

        [DllImport("*")]
        public static extern void addKeyToQueue(int pressed, byte keyCode);

        #endregion
        public static Image aScreenBuf;

        public Doom(void* ptr,long length)
        {
            aScreenBuf = new Image(320, 200);

            var handler = (EventHandler<ConsoleKeyInfo>)PS2Keyboard_OnKeyChanged;

            Program.BindOnKeyChangedHandler(handler);

            doommain((byte*)ptr,length);
        }

        private void PS2Keyboard_OnKeyChanged(object sender, ConsoleKeyInfo key)
        {
            addKeyToQueue(key.KeyState != ConsoleKeyState.Released ? 1 : 0, (byte)key.Key);
        }
    }
}