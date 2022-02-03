using KernelEFI;

namespace Kernel
{
    public unsafe class Console
    {
        internal static void WriteLine(string v)
        {
            fixed (char* p = v)
                Efi.ST->ConOut->OutputString(Efi.ST->ConOut, p);
        }
    }
}
