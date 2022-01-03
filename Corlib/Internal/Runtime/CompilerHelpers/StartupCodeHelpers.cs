using System;
using System.Runtime;
using System.Runtime.InteropServices;
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerHelpers {
	//[McgIntrinsics]
	class StartupCodeHelpers {
		[RuntimeExport("RhpReversePInvoke2")]
		static void RhpReversePInvoke2(IntPtr frame) { }

		[RuntimeExport("RhpReversePInvokeReturn2")]
		static void RhpReversePInvokeReturn2(IntPtr frame) { }

		[RuntimeExport("RhpPInvoke")]
		static void RhpPinvoke(IntPtr frame) { }

		[RuntimeExport("RhpPInvokeReturn")]
		static void RhpPinvokeReturn(IntPtr frame) { }

		[RuntimeExport("RhpNewFast")]
		static unsafe object RhpNewFast(EEType* pEEType) {
			var size = pEEType->BaseSize;

			// Round to next power of 8
			if (size % 8 > 0)
				size = ((size / 8) + 1) * 8;

			var data = Platform.Allocate(size);
			var obj = Unsafe.As<IntPtr, object>(ref data);
			Platform.ZeroMemory(data, size);
			SetEEType(data, pEEType);

			return obj;
		}

		[RuntimeExport("RhpNewArray")]
		internal static unsafe object RhpNewArray(EEType* pEEType, int length) {
			var size = pEEType->BaseSize + (ulong)length * pEEType->ComponentSize;

			// Round to next power of 8
			if (size % 8 > 0)
				size = ((size / 8) + 1) * 8;

			var data = Platform.Allocate(size);
			var obj = Unsafe.As<IntPtr, object>(ref data);
			Platform.ZeroMemory(data, size);
			SetEEType(data, pEEType);

			var b = (byte*)data;
			b += sizeof(IntPtr);
			Platform.CopyMemory((IntPtr)b, (IntPtr)(&length), sizeof(int));

			return obj;
		}

		//[RuntimeExport("RhpAssignRef")]
		//static unsafe void RhpAssignRef(ref object address, object obj) {
		//	var pAddr = (void**)Unsafe.AsPointer(ref address);
		//	var pObj = (void*)Unsafe.As<object, IntPtr>(ref obj);
		//	*pAddr = pObj;
		//	//address = obj;
		//}

		[RuntimeExport("RhpAssignRef")]
		static unsafe void RhpAssignRef(void** address, void* obj) {
			*address = obj;
		}

		[RuntimeExport("RhpByRefAssignRef")]
		static unsafe void RhpByRefAssignRef(void** address, void* obj) {
			*address = obj;
		}

		[RuntimeExport("RhpCheckedAssignRef")]
		static unsafe void RhpCheckedAssignRef(void** address, void* obj) {
			*address = obj;
		}

		[RuntimeExport("RhpStelemRef")]
		static unsafe void RhpStelemRef(Array array, int index, object obj) {
			fixed (int* n = &array._numComponents) {
				var ptr = (byte*)n;
				ptr += 8;   // Array length is padded to 8 bytes on 64-bit
				ptr += index * array.m_pEEType->ComponentSize;  // Component size should always be 8, seeing as it's a pointer...
				var pp = (IntPtr*)ptr;
				*pp = Unsafe.As<object, IntPtr>(ref obj);
			}
		}

		[RuntimeExport("RhTypeCast_IsInstanceOfClass")]
		static unsafe object RhTypeCast_IsInstanceOfClass(EEType* pTargetType, object obj) {
			if (obj == null)
				return null;

			if (pTargetType == obj.m_pEEType)
				return obj;

			var bt = obj.m_pEEType->RawBaseType;

			while (true) {
				if (bt == null)
					return null;

				if (pTargetType == bt)
					return obj;

				bt = bt->RawBaseType;
			}
		}

		internal static unsafe void SetEEType(IntPtr obj, EEType* type) {
			Platform.CopyMemory(obj, (IntPtr)(&type), (ulong)sizeof(IntPtr));
		}

		public static unsafe void InitialiseRuntime(IntPtr modulesSeg) {
			var modules = (IntPtr*)modulesSeg;

			for (int i = 0; ; i++) {
				var addr = modules[i];

				if (addr.Equals(IntPtr.Zero))
					break;

				InitialiseModule(addr, i);
			}
		}

		static unsafe void InitialiseModule(IntPtr addr, int index) {
			var header = (ReadyToRunHeader*)addr;
			var sections = (ModuleInfoRow*)(header + 1);

			for (int i = 0; i < header->NumberOfSections; i++) {
				if (sections[i].SectionId != 201)	// We only care about GCStaticRegion right now
					continue;

				InitialiseStatics(sections[i].Start, sections[i].End);
				break;
			}
		}

		static unsafe void InitialiseStatics(IntPtr rgnStart, IntPtr rgnEnd) {
			for (var block = (IntPtr*)rgnStart; block < (IntPtr*)rgnEnd; block++) {
				var pBlock = (IntPtr*)*block;
				var blockAddr = (long)(*pBlock);

				if ((blockAddr & 1) == 1) { // GCStaticRegionConstants.Uninitialized
					var obj = RhpNewFast((EEType*)new IntPtr(blockAddr & ~(1 | 2)));
					var handle = Platform.Allocate((ulong)sizeof(IntPtr));
					*(IntPtr*)handle = Unsafe.As<object, IntPtr>(ref obj);
					*pBlock = handle;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		struct ReadyToRunHeader {
			public uint Signature;  // "RTR"
			public ushort MajorVersion;
			public ushort MinorVersion;
			public uint Flags;
			public ushort NumberOfSections;
			public byte EntrySize;
			public byte EntryType;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct ModuleInfoRow {
			public int SectionId;
			public int Flags;
			public IntPtr Start;
			public IntPtr End;

			public bool HasEndPointer => !End.Equals(IntPtr.Zero);
			public int Length => (int)((ulong)End - (ulong)Start);
		}
	}
}