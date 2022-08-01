using System;
using System.Runtime.InteropServices;
using MOOS.Misc;

namespace MOOS
{
	public static unsafe class AVX
	{
		public static void init_avx()
		{
			CPUID* cpuid = Native.CPUID(1);
			if (!BitHelpers.IsBitSet(cpuid->ECX, 28))
			{
				Console.WriteInfo("AVX", "Warning: this CPU doesn't support AVX!");
				return;
			}
			enable_avx();
		}

		[DllImport("*")]
		public static extern void enable_avx();

		[DllImport("*")]
		public static extern void avx_memcpy(void* pvDest, void* pvSrc, ulong nBytes);
	}
}