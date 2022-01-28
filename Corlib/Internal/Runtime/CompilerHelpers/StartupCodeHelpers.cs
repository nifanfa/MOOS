using Internal.Runtime.CompilerServices;
using Kernel;
using System;
using System.Runtime;
using System.Runtime.InteropServices;

namespace Internal.Runtime.CompilerHelpers
{
    [McgIntrinsics]
    class StartupCodeHelpers
    {
        [RuntimeExport("RhpFallbackFailFast")]
        static void RhpFallbackFailFast() { while (true) ; }

        [RuntimeExport("RhpReversePInvoke2")]
        static void RhpReversePInvoke2(IntPtr frame) { }

        [RuntimeExport("RhpReversePInvokeReturn2")]
        static void RhpReversePInvokeReturn2(IntPtr frame) { }

        [RuntimeExport("RhpReversePInvoke")]
        static void RhpReversePInvoke(IntPtr frame) { }

        [RuntimeExport("RhpReversePInvokeReturn")]
        static void RhpReversePInvokeReturn(IntPtr frame) { }

        [RuntimeExport("RhpPInvoke")]
        static void RhpPinvoke(IntPtr frame) { }

        [RuntimeExport("RhpPInvokeReturn")]
        static void RhpPinvokeReturn(IntPtr frame) { }

        [RuntimeExport("RhpNewFast")]
        static unsafe object RhpNewFast(EEType* pEEType)
        {
            var size = pEEType->BaseSize;

            // Round to next power of 8
            if (size % 8 > 0)
                size = ((size / 8) + 1) * 8;

            var data = Heap.Allocate(size);
            var obj = Unsafe.As<IntPtr, object>(ref data);
            Heap.ZeroMemory(data, size);
            SetEEType(data, pEEType);

            return obj;
        }

        [RuntimeExport("RhpNewArray")]
        internal static unsafe object RhpNewArray(EEType* pEEType, int length)
        {
            var size = pEEType->BaseSize + (ulong)length * pEEType->ComponentSize;

            // Round to next power of 8
            if (size % 8 > 0)
                size = ((size / 8) + 1) * 8;

            var data = Heap.Allocate(size);
            var obj = Unsafe.As<IntPtr, object>(ref data);
            Heap.ZeroMemory(data, size);
            SetEEType(data, pEEType);

            var b = (byte*)data;
            b += sizeof(IntPtr);
            Heap.MemoryCopy((IntPtr)b, (IntPtr)(&length), sizeof(int));

            return obj;
        }

        [RuntimeExport("RhpAssignRef")]
        static unsafe void RhpAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpByRefAssignRef")]
        static unsafe void RhpByRefAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpCheckedAssignRef")]
        static unsafe void RhpCheckedAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpStelemRef")]
        static unsafe void RhpStelemRef(Array array, int index, object obj)
        {
            fixed (int* n = &array._numComponents)
            {
                var ptr = (byte*)n;
                ptr += 8;   // Array length is padded to 8 bytes on 64-bit
                ptr += index * array.m_pEEType->ComponentSize;  // Component size should always be 8, seeing as it's a pointer...
                var pp = (IntPtr*)ptr;
                *pp = Unsafe.As<object, IntPtr>(ref obj);
            }
        }

        [RuntimeExport("RhTypeCast_IsInstanceOfClass")]
        public static unsafe object RhTypeCast_IsInstanceOfClass(EEType* pTargetType, object obj)
        {
            if (obj == null)
                return null;

            if (pTargetType == obj.m_pEEType)
                return obj;

            var bt = obj.m_pEEType->RawBaseType;

            while (true)
            {
                if (bt == null)
                    return null;

                if (pTargetType == bt)
                    return obj;

                bt = bt->RawBaseType;
            }
        }

        internal static unsafe void SetEEType(IntPtr obj, EEType* type)
        {
            Heap.MemoryCopy(obj, (IntPtr)(&type), (ulong)sizeof(IntPtr));
        }

        public static unsafe void InitializeRuntime(IntPtr modulesSeg)
        {
            var modules = (IntPtr*)modulesSeg;

            for (int i = 0; ; i++)
            {
                var addr = modules[i];

                if (addr.Equals(IntPtr.Zero))
                    break;

                InitializeModules(addr, i);
            }
        }

        static unsafe void InitializeModules(IntPtr addr, int index)
        {
            var header = (ReadyToRunHeader*)addr;
            var sections = (ModuleInfoRow*)(header + 1);

            for (int i = 0; i < header->NumberOfSections; i++)
            {
                if (sections[i].SectionId == (int)ReadyToRunSectionType.GCStaticRegion)
                    InitializeStatics(sections[i].Start, sections[i].End);

                if (sections[i].SectionId == (int)ReadyToRunSectionType.EagerCctor)
                    InitializeEagerClassConstructorsForModule(sections[i].Start, sections[i].End);
            }
        }

        static unsafe void InitializeEagerClassConstructorsForModule(IntPtr rgnStart, IntPtr rgnEnd) 
        {
            RunEagerClassConstructors(rgnStart, rgnEnd);
        }

        private static unsafe void RunEagerClassConstructors(IntPtr cctorTableStart, IntPtr cctorTableEnd)
        {
            for (IntPtr* tab = (IntPtr*)cctorTableStart; tab < (IntPtr*)cctorTableEnd; tab++)
            {
                ((delegate*<void>)*tab)();
            }
        }

        static unsafe void InitializeStatics(IntPtr rgnStart, IntPtr rgnEnd)
        {
            for (var block = (IntPtr*)rgnStart; block < (IntPtr*)rgnEnd; block++)
            {
                var pBlock = (IntPtr*)*block;
                var blockAddr = (long)(*pBlock);

                if ((blockAddr & GCStaticRegionConstants.Uninitialized) == GCStaticRegionConstants.Uninitialized)
                { // GCStaticRegionConstants.Uninitialized
                    var obj = RhpNewFast((EEType*)new IntPtr(blockAddr & ~(GCStaticRegionConstants.Uninitialized | GCStaticRegionConstants.HasPreInitializedData)));
                    var handle = Heap.Allocate((ulong)sizeof(IntPtr));
                    *(IntPtr*)handle = Unsafe.As<object, IntPtr>(ref obj);
                    *pBlock = handle;
                }
            }
        }
    }
}