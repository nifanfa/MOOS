﻿using Kernel;
using System;

struct MemoryMap {
	IntPtr _pointer;
	ulong descriptorSize;

	public int Length { get; private set; }
	public ulong Key { get; private set; }

	public unsafe EFI.MemoryDescriptor this[int index]
		=> *(EFI.MemoryDescriptor*)(_pointer + (uint)index * (uint)descriptorSize);

	public unsafe void Retrieve() {
		_pointer = IntPtr.Zero;
		ulong size = 0;
		ref var bs = ref EFI.EFI.ST.Ref.BootServices.Ref;

		var res = bs.GetMemoryMap(ref size, _pointer, out var key, out descriptorSize, out var descripterVersion);

		if (res == EFI.Status.BufferTooSmall) {
			fixed (IntPtr* p = &_pointer)
				res = bs.AllocatePool(EFI.MemoryType.LoaderData, size, p);

			if (res != EFI.Status.Success)
				Allocator.PrintLine("WHOOPS");

			size += descriptorSize;
			res = bs.GetMemoryMap(ref size, _pointer, out key, out descriptorSize, out descripterVersion);

			if (res != EFI.Status.Success)
				Allocator.PrintLine("oh no");
		}
		else
			Allocator.PrintLine("WTF?");

		Length = (int)(size / descriptorSize);
		Key = key;
	}

	public void Free() {
		EFI.EFI.ST.Ref.BootServices.Ref.FreePool(_pointer);
	}
}