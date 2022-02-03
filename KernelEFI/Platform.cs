using KernelEFI;
using System;

namespace Kernel
{
    internal unsafe class Platform
    {
        public static IntPtr Allocate(ulong size) 
        {
            Efi.ST->BootServices->AllocatePool(EfiMemoryType.EfiLoaderData, size, out var ptr);
            return (IntPtr)ptr;
        }

        public static void Free(IntPtr ptr)
        {
            Efi.ST->BootServices->FreePool((void*)ptr);
        }

        public static void ZeroMemory(IntPtr ptr,ulong size) 
        {
            Efi.ST->BootServices->SetMem((void*)ptr, size, 0);
        }

        public static void MemoryCopy(IntPtr dst, IntPtr src,ulong size)
        {
            Efi.ST->BootServices->CopyMem((void*)dst, (void*)src, size);
        }
    }
}
