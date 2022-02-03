using Kernel;
using System.Runtime;

namespace KernelEFI 
{
    public static unsafe class Program 
    {
        public static void Main() { }

        [RuntimeExport("EfiMain")]
        public static EfiStatus EfiMain(EfiHandle handle, EfiSystemTable* st) 
        {
            Efi.ST = st;
            Console.WriteLine("Hello World");
            for (; ; );
            return EfiStatus.EfiSuccess;
        }
    }
}