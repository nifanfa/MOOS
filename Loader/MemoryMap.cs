using Kernel;
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
		var bs = EFI.EFI.ST->BootServices;

		var res = bs->GetMemoryMap(ref size, _pointer, out var key, out descriptorSize, out var descripterVersion);

		if (res == EFI.Status.BufferTooSmall) {
			fixed (IntPtr* p = &_pointer)
				res = bs->AllocatePool(EFI.MemoryType.LoaderData, size, p);

			size += descriptorSize;
			res = bs->GetMemoryMap(ref size, _pointer, out key, out descriptorSize, out descripterVersion);
		}

		Length = (int)(size / descriptorSize);
		Key = key;
	}

	public unsafe void Free() {
		EFI.EFI.ST->BootServices->FreePool(_pointer);
	}
}