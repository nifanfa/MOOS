using Internal.Runtime.CompilerServices;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime;
using static IDT;
using static Internal.Runtime.CompilerHelpers.InteropHelpers;

#if HasGUI
using MOOS.GUI;
#endif

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
                case "Clear":
                    return (delegate*<uint, void>)&API_Clear;
                case "Update":
                    return (delegate*<void>)&API_Update;
                case "Width":
                    return (delegate*<uint>)&API_Width;
                case "Height":
                    return (delegate*<uint>)&API_Height;
                case "WriteString":
                    return (delegate*<string, void>)&API_WriteString;
                case "GetTime":
                    return (delegate*<ulong>)&API_GetTime;
                case "DrawImage":
                    return (delegate*<int,int,Image,void>)&API_DrawImage;
                case "Error":
                    return (delegate*<string, bool, void>)&API_Error;
                case "StartThread":
                    return (delegate*<delegate*<void>, void>)&API_StartThread;
#if Kernel && HasGUI
                case "CreateWindow":
                    return (delegate*<int, int, int, int, string, IntPtr>)&API_CreateWindow;
                case "GetWindowScreenBuf":
                    return (delegate*<IntPtr, IntPtr>)&API_GetWindowScreenBuf;
                case "BindOnKeyChangedHandler":
                    return (delegate*<EventHandler<ConsoleKeyInfo>, void>)&API_BindOnKeyChangedHandler;
#endif
                case "Calloc":
                    return (delegate*<ulong, ulong, void*>)&API_Calloc;
                case "SndWrite":
                    return (delegate*<byte*, int, int>)&API_SndWrite;
            }
            Panic.Error($"System call \"{name}\" is not found");
            return null;
        }

#if Kernel && HasGUI
        public static IntPtr API_CreateWindow(int X, int Y, int Width, int Height, string Title)
        {
            PortableApp papp = new PortableApp(X, Y, Width, Height);
            papp.Title = Title;
            return papp;
        }

        public static IntPtr API_GetWindowScreenBuf(IntPtr handle)
        {
            PortableApp papp = Unsafe.As<IntPtr, PortableApp>(ref handle);
            return papp.ScreenBuf;
        }
#endif

        public static int API_SndWrite(byte* buffer, int len)
        {
            return Audio.snd_write(buffer, len);
        }

        public static void* API_Calloc(ulong num, ulong size)
        {
            return stdlib.calloc(num, size);
        }

        public static void API_BindOnKeyChangedHandler(EventHandler<ConsoleKeyInfo> handler)
        {
            Keyboard.OnKeyChanged = handler;

        }

        public static void API_StartThread(delegate* <void> func)
        {
            new Thread(func).Start();
        }

        public static void API_Error(string s,bool skippable)
        {
            Panic.Error(s,skippable);
        }

        public static ulong API_GetTime()
        {
            ulong century = RTC.Century;
            ulong year = RTC.Year;
            ulong month = RTC.Month;
            ulong day = RTC.Day;
            ulong hour = RTC.Hour;
            ulong minute = RTC.Minute;
            ulong second = RTC.Second;

            ulong time = 0;

            time |= century << 56;
            time |= year << 48;
            time |= month << 40;
            time |= day << 32;
            time |= hour << 24;
            time |= minute << 16;
            time |= second << 8;

            return time;
        }

        public static void API_DrawImage(int X, int Y, Image image)
        {
            Framebuffer.Graphics.DrawImage(X, Y, image, false);
        }

        public static void API_WriteString(string s) 
        {
            Console.Write(s);
            s.Dispose();
        }

        public static uint API_Width() => Framebuffer.Width;

        public static uint API_Height() => Framebuffer.Height;

        public static void API_Update()
        {
            Framebuffer.Update();
        }

        public static void API_Clear(uint color) => Framebuffer.Graphics.Clear(color);

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
            Framebuffer.Graphics.DrawPoint(x, y, color);
        }

        public static void API_SwitchToConsoleMode() 
        {
            Framebuffer.TripleBuffered = false;
        }

        public static void API_ReadAllBytes(string name,ulong* length,byte** data) 
        {
            byte[] buffer = File.ReadAllBytes(name);

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