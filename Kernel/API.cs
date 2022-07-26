using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;
using System;
using System.Diagnostics;
using System.Runtime;
using static IDT;
using static Internal.Runtime.CompilerHelpers.InteropHelpers;

namespace MOOS
{
    public static unsafe class API
    {
        public static unsafe void* HandleSystemCall(string name)
        {
            switch (name)
            {
                case "SayHello":
                    return (delegate*<void>)&SayHello;
                case "WriteLine":
                    return (delegate*<void>)&API_WriteLine;
                case "DebugWriteLine":
                    return (delegate*<void>)&API_DebugWriteLine;
                case "Allocate":
                    return (delegate*<ulong, nint>)&API_Allocate;
                case "Reallocate":
                    return (delegate*<nint, ulong, nint>)&API_Reallocate;
                case "Free":
                    return (delegate*<nint, ulong>)&API_Free;
                case "Sleep":
                    return (delegate*<ulong, void>)&API_Sleep;
                case "GetTick":
                    return (delegate*<ulong>)&API_GetTick;
                case "ReadAllBytes":
                    return (delegate*<string, ulong*, byte**, void>)&API_ReadAllBytes;
                case "Write":
                    return (delegate*<char, void>)&API_Write;
                case "DebugWrite":
                    return (delegate*<char, void>)&API_DebugWrite;
                case "SwitchToConsoleMode":
                    return (delegate*<void>)&API_SwitchToConsoleMode;
                case "DrawPoint":
                    return (delegate*<int, int, uint, void>)&API_DrawPoint;
                case "Lock":
                    return (delegate*<void>)&API_Lock;
                case "Unlock":
                    return (delegate*<void>)&API_Unlock;
            }
            Panic.Error($"System call \"{name}\" is not found");
            return null;
        }

        [RuntimeExport("DebugWrite")]
        public static void API_DebugWrite(char c)
        {
            Serial.Write(c);
        }

        [RuntimeExport("DebugWriteLine")]
        public static void API_DebugWriteLine()
        {
            Serial.WriteLine();
        }

        [RuntimeExport("Lock")]
        public static void API_Lock() 
        {
            if (ThreadPool.CanLock)
            {
                if (!ThreadPool.Locked)
                {
                    ThreadPool.Lock();
                }
            }
        }

        [RuntimeExport("Unlock")]
        public static void API_Unlock()
        {
            if (ThreadPool.CanLock)
            {
                if (ThreadPool.Locked)
                {
                    if (ThreadPool.Locker == SMP.ThisCPU)
                    {
                        ThreadPool.UnLock();
                    }
                }
            }
        }

        public static void API_DrawPoint(int x, int y, uint color)
        {
            if (!Framebuffer.TripleBuffered) 
            {
                Framebuffer.Graphics.DrawPoint(x, y, color);
            }
        }

        public static void API_SwitchToConsoleMode() 
        {
            Framebuffer.TripleBuffered = false;
        }

        public static void API_ReadAllBytes(string name,ulong* length,byte** data) 
        {
            byte[] buffer = File.Instance.ReadAllBytes(name);

            *data = (byte*)Allocator.Allocate((ulong)buffer.Length);
            *length = (ulong)buffer.Length;
            fixed (byte* p = buffer) Native.Movsb(*data, p, *length);

            buffer.Dispose();
        }

        public static void API_Sleep(ulong ms) 
        {
            Thread.Sleep(ms);
        }

        public static ulong API_GetTick() 
        {
            return Timer.Ticks;
        }

        public static void API_Write(char c) 
        {
            Console.Write(c);
        }

        public static void API_WriteLine() 
        {
            Console.WriteLine();
        }

        public static nint API_Allocate(ulong size)
        {
            //Debug.WriteLine($"API_Allocate {size}");
            return Allocator.Allocate(size);
        }

        public static ulong API_Free(nint ptr)
        {
            //Debug.WriteLine($"API_Free 0x{((ulong)ptr).ToString("x2")}");
            return Allocator.Free(ptr);
        }

        public static nint API_Reallocate(nint intPtr, ulong size) 
        {
            return Allocator.Reallocate(intPtr, size);
        }

        public static void SayHello()
        {
            Console.WriteLine("Hello from exe!");
        }
    }
}