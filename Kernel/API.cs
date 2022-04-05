using static IDT;
using static Internal.Runtime.CompilerHelpers.InteropHelpers;

namespace Kernel
{
    public static unsafe class API
    {
        public static unsafe void HandleSystemCall(IDTStack* stack)
        {
            var pCell = (MethodFixupCell*)stack->rcx;
            string name = string.FromASCII(pCell->MethodName, strings.strlen((byte*)pCell->MethodName));
            switch (name)
            {
                case "SayHello":
                    stack->rax = (ulong)(delegate*<void>)&SayHello;
                    break;
            }
            name.Dispose();
        }

        public static void SayHello()
        {
            Console.WriteLine("Hello from exe!");
        }
    }
}
