using MOOS.Driver;
using MOOS.Misc;
using System.Diagnostics;
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
                case "Console.WriteLine":
                    return (delegate*<string, void>)&API_WriteLine;
                case "Allocate":
                    return (delegate*<ulong, nint>)&API_Allocate;
                case "Free":
                    return (delegate*<nint, ulong>)&API_Free;
                case "Sleep":
                    return (delegate*<ulong, void>)&API_Sleep;
                case "GetTick":
                    return (delegate*<ulong>)&API_GetTick;
            }
            Panic.Error($"System call \"{name}\" is not found");
            return null;
        }

        public static void API_Sleep(ulong ms) 
        {
            Thread.Sleep(ms);
        }

        public static ulong API_GetTick() 
        {
            return Timer.Ticks;
        }

        public static void API_WriteLine(string s) 
        {
            Console.WriteLine(s);
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

        public static void SayHello()
        {
            Console.WriteLine("Hello from exe!");
        }
    }
}