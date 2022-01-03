using System;
using System.Runtime;


abstract class Allocator {
	unsafe struct BitMap {
		public fixed byte Data[BITMAP_LENGTH];
	}


	const int BITMAP_LENGTH = 1024 * 16; // 16kb can hold enough bits to represent 131,072 pages, allowing up to 512mb to be allocated (assuming a page size of 4kb)


	static BitMap bitMap;
	static int totalPages = 0;
	static uint allocations = 0;


	public static unsafe bool AddFreePages(IntPtr address, uint pages) {
		var startPage = (int)((ulong)address / 4096);

		for (int i = 0; i < pages; i++) {
			if (totalPages < startPage + i)
				totalPages = startPage + i;

			var index = (startPage + i) / 8;
			var bit = (startPage + i) % 8;

			bitMap.Data[index] |= (byte)(1 << bit);
		}

		return true;
	}


	[RuntimeExport("liballoc_lock")]
	public static int Lock() {
		if (IDT.Initialised)
			IDT.Disable();

		return 0;
	}

	[RuntimeExport("liballoc_unlock")]
	public static int Unlock() {
		if (IDT.Initialised)
			IDT.Enable();

		return 0;
	}

	[RuntimeExport("liballoc_alloc")]
	public static unsafe IntPtr Alloc(ulong pages) {
		var count = 0;
		var index = 0;

		for (; ; ) {
			if (index >= totalPages)
				return IntPtr.Zero;

			if (count == (int)pages) {
				for (int i = 0; i < (int)pages; i++) {
					var _off = (index + i) / 8;
					var _bit = (index + i) % 8;
					bitMap.Data[_off] &= (byte)~(1 << _bit);
				}

				allocations++;

				return (IntPtr)(index * 4096);
			}

			if (count == 0) {
				for (; ; ) {
					var _off = index / 8;
					var _bit = index % 8;

					if ((bitMap.Data[_off] & (1 << _bit)) == (1 << _bit))
						break;

					index++;
				}
			}

			var off = (index + count) / 8;
			var bit = (index + count) % 8;

			if ((bitMap.Data[off] & (1 << bit)) != (1 << bit))
				count = 0;

			count++;
		}
	}

	[RuntimeExport("liballoc_free")]
	public static unsafe int Free(IntPtr ptr, ulong pages) {
		AddFreePages(ptr, (uint)pages);
		allocations--;

		return 0;
	}

	public static ulong TotalMemory
		=> (ulong)totalPages * 4096;

	public static unsafe ulong UsedMemory {
		get {
			var r = 0UL;

			for (int i = 0; i < totalPages / 8; i++) {
				for (int j = 0; j < 8; j++) {
					if ((bitMap.Data[i] & (1 << j)) != (1 << j))
						r++;
				}
			}

			return r * 4096;
		}
	}

	public static ulong FreeMemory
		=> TotalMemory - UsedMemory;

	public static ulong Allocations
		=> (ulong)allocations;
}