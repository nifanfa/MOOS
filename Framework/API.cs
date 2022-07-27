using System.Runtime;
using System.Threading;
using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;

namespace MOOS
{
    public static unsafe class API
    {
        public static unsafe void* HandleSystemCall(string name)
        {
            switch (name)
            {
                case "WriteLine":
                    return (delegate*<string, void>)&Console.WriteLine;
                case "DebugWriteLine":
                    return (delegate*<string, void>)&API_DebugWriteLine;
                case "Allocate":
                    return (delegate*<ulong, nint>)&Allocator.Allocate;
                case "Reallocate":
                    return (delegate*<nint, ulong, nint>)&Allocator.Reallocate;
                case "Free":
                    return (delegate*<nint, ulong>)&Allocator.Free;
                case "Sleep":
                    return (delegate*<ulong, void>)&Thread.Sleep;
                case "GetTick":
                    return (delegate*<ulong>)&GetTick;
                case "ReadAllBytes":
                    return (delegate*<string, ulong*, byte**, void>)&ReadAllBytes;
                case "Write":
                    return (delegate*<string, void>)&Console.Write;
                case "DebugWrite":
                    return (delegate*<char, void>)&API_DebugWrite;
                case "SwitchToConsoleMode":
                    return (delegate*<void>)&SwitchToConsoleMode;
                case "DrawPoint":
                    return (delegate*<int, int, uint, bool, void>)&DrawPoint;
                case "Lock":
                    return (delegate*<object, void>)&Monitor.Enter;
                case "Unlock":
                    return (delegate*<object, void>)&Monitor.Exit;
            }
            Panic.Error($"System call \"{name}\" is not found");
            return null;
        }

        [RuntimeExport("Panic")]
        public static void API_Panic(string message)
        {
            Panic.Error(message);
        }
        [RuntimeExport("DebugWrite")]
        public static void API_DebugWrite(char c)
        {
            Serial.Write(c);
        }

        [RuntimeExport("DebugWriteLine")]
        public static void API_DebugWriteLine(string s)
        {
            Serial.WriteLine(s);
        }

        public static void DrawPoint(int x, int y, uint color, bool alphaBlending = false)
        {
            Framebuffer.Graphics.DrawPoint(x, y, color, alphaBlending);
        }

        public static void SwitchToConsoleMode()
        {
            Framebuffer.DoubleBuffered = false;
        }

        public static void ReadAllBytes(string name, ulong* length, byte** data)
        {
            byte[] buffer = File.Instance.ReadAllBytes(name);

            *data = (byte*)Allocator.Allocate((ulong)buffer.Length);
            *length = (ulong)buffer.Length;
            fixed (byte* p = buffer)
            {
                Native.Movsb(*data, p, *length);
            }

            buffer.Dispose();
        }

        public static ulong GetTick()
        {
            return Timer.Ticks;
        }
    }
}