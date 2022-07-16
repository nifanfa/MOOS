using MOOS.Misc;
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
            }
            Panic.Error($"System call \"{name}\" is not found");
            return null;
        }

        public static void API_WriteLine(string s) 
        {
            Console.WriteLine(s);
        }

        public static void SayHello()
        {
            Console.WriteLine("Hello from exe!");
        }
    }
}