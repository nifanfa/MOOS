using Kernel.Misc;
using static IDT;
using static Internal.Runtime.CompilerHelpers.InteropHelpers;

namespace Kernel
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
                    return (delegate*<char*, void>)&API_WriteLine;
            }
            Panic.Error($"System call \"{name}\" is not found");
            return null;
        }

        //String will become char* if we use DllImport
        public static void API_WriteLine(char* c) 
        {
            int i = 0;
            while (c[i] != 0)
            {
                Console.Write(c[i]);
                i++;
            }

            Console.WriteLine();
        }

        public static void SayHello()
        {
            Console.WriteLine("Hello from exe!");
        }
    }
}
