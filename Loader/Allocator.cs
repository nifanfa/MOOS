using System;

namespace Kernel 
{
	internal static unsafe class Allocator
	{
		public static IntPtr Allocate(ulong size)
		{
			IntPtr ptr = default;
			EFI.EFI.ST->BootServices->AllocatePool(EFI.MemoryType.LoaderData, size, &ptr);

			return ptr;
		}

		public static void Free(IntPtr buf)
		{
			EFI.EFI.ST->BootServices->FreePool(buf);
		}

		public static unsafe void ZeroFill(IntPtr ptr, ulong len)
		{
			EFI.EFI.ST->BootServices->SetMem(ptr, len, 0);
		}

		public static unsafe void MemoryCopy(IntPtr dst, IntPtr src, ulong len)
		{
			EFI.EFI.ST->BootServices->CopyMem(dst, src, len);
		}
	}
}