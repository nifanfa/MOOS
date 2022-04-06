using Kernel.Misc;
using static IDT;
using static Internal.Runtime.CompilerHelpers.InteropHelpers;

namespace Kernel
{
    public static unsafe class API
    {
        public static unsafe void HandleSystemCall(IDTStack* stack)
        {
            var pCell = (MethodFixupCell*)stack->rcx;
            string name = string.FromASCII(pCell->Module->ModuleName, strings.strlen((byte*)pCell->Module->ModuleName));
            switch (name)
            {
                case "SayHello":
                    stack->rax = (ulong)(delegate*<void>)&SayHello;
                    break;
                case "Console.WriteLine":
                    stack->rax = (ulong)(delegate*<char*, void>)&API_WriteLine;
                    break;
                default:
                    Panic.Error($"System call \"{name}\" is not found");
                    break;
            }
            name.Dispose();
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
