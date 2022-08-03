using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Internal.Runtime.CompilerHelpers
{
    unsafe class StartupCodeHelpers
    {


        [RuntimeExport("__current_exception_context")]
        public static void __current_exception_context()
        {

        }

        [RuntimeExport("__current_exception")]
        public static void __current_exception()
        {

        }

        [RuntimeExport("terminate")]
        public static void Terminate()
        {

        }

        [RuntimeExport("__C_specific_handler")]
        public static void __C_specific_handler()
        {

        }

        [RuntimeExport("_cexit")]
        public static void _cexit()
        {

        }

        [RuntimeExport("__acrt_thread_detach")]
        public static void __acrt_thread_detach()
        {

        }

        [RuntimeExport("__acrt_thread_attach")]
        public static void __acrt_thread_attach()
        {

        }

        [RuntimeExport("__acrt_uninitialize_critical")]
        public static void __acrt_uninitialize_critical()
        {

        }


        [RuntimeExport("__acrt_uninitialize")]
        public static void __acrt_uninitialize()
        {

        }

        [RuntimeExport("__acrt_initialize")]
        public static void __acrt_initialize()
        {

        }

        [RuntimeExport("_crt_at_quick_exit")]
        public static void _crt_at_quick_exit()
        {

        }

        [RuntimeExport("_crt_atexit")]
        public static void _crt_atexit()
        {

        }

        [RuntimeExport("_execute_onexit_table")]
        public static void _execute_onexit_table()
        {

        }

        [RuntimeExport("_register_onexit_function")]
        public static void _register_onexit_function()
        {

        }

        [RuntimeExport("_configure_narrow_argv")]
        public static void _configure_narrow_argv()
        {

        }

        [RuntimeExport("_initialize_onexit_table")]
        public static void _initialize_onexit_table()
        {

        }

        [RuntimeExport("_initialize_narrow_environment")]
        public static void _initialize_narrow_environment()
        {

        }

        [RuntimeExport("_is_c_termination_complete")]
        public static void _is_c_termination_complete()
        {

        }

        [RuntimeExport("_seh_filter_dll")]
        public static void _seh_filter_dll()
        {

        }

        [RuntimeExport("__vcrt_thread_detach")]
        public static void __vcrt_thread_detach()
        {

        }

        [RuntimeExport("__vcrt_thread_attach")]
        public static void __vcrt_thread_attach()
        {

        }

        [RuntimeExport("__vcrt_uninitialize_critical")]
        public static void __vcrt_uninitialize_critical()
        {

        }

        [RuntimeExport("__vcrt_uninitialize")]
        public static void __vcrt_uninitialize()
        {

        }

        [RuntimeExport("__vcrt_initialize")]
        public static void __vcrt_initialize()
        {

        }

        [RuntimeExport("_CxxThrowException")]
        public static void _CxxThrowException()
        {

        }

        [RuntimeExport("__std_exception_destroy")]
        public static void __std_exception_destroy()
        {

        }

        [RuntimeExport("__std_exception_copy")]
        public static int __std_exception_copy() => 0;

        [RuntimeExport("_free_dbg")]
        public static void _free_dbg()
        {
        }

        [RuntimeExport("_CrtDbgReportW")]
        public static int _CrtDbgReportW() => 0;

        [RuntimeExport("_callnewh")]
        public static void _callnewh()
        { 
        }

        [RuntimeExport("__CxxFrameHandler4")]
        public static int __CxxFrameHandler4() => 0;

        [RuntimeExport("RhpFailFastForPInvokeExceptionCoop")]
        public static int RhpFailFastForPInvokeExceptionCoop() => 0;

        [RuntimeExport("RhpFailFastForPInvokeExceptionPreemp")]
        public static int RhpFailFastForPInvokeExceptionPreemp() => 0;

        [RuntimeExport("RhpCidResolve")]
        public static int RhpCidResolve() => 0;

        [RuntimeExport("strtoul")]
        public static int strtoul() => 0;

        [RuntimeExport("ProcessFinalizers")]
        public static int ProcessFinalizers() => 0;

        [RuntimeExport("_wcsicmp")]
        public static int _wcsicmp() => 0;

        [RuntimeExport("_purecall")]
        public static int _purecall() => 0;

        [RuntimeExport("RhpCalculateStackTraceWorker")]
        public static int RhpCalculateStackTraceWorker() => 0;

        [RuntimeExport("memcmp")]
        public static int MemCmp() => 0;

        [RuntimeExport("RhRethrow")]
        public static int RhRethrow() => 0;

        [RuntimeExport("RhThrowEx")]
        public static int RhThrowEx() => 0;

        [RuntimeExport("RhThrowHwEx")]
        public static int RhThrowHwEx() => 0;

        [RuntimeExport("RhExceptionHandling_FailedAllocation")]
        public static int RhExceptionHandling_FailedAllocation() => 0;

        [RuntimeExport("RhpStelemRef")]
        public static int RhpStelemRef() => 0;

        [RuntimeExport("RhUnbox2")]
        public static int RhUnbox2() => 0;

        //-------------------------------------------------------------------
        //[RuntimeExport("__imp_GetCurrentThreadId")]
        //public static int __imp_GetCurrentThreadId() => 0;

        [RuntimeExport("__CheckForDebuggerJustMyCode")]
        public static int __CheckForDebuggerJustMyCode() => 0;

        [RuntimeExport("__fail_fast")]
        static void FailFast() { while (true) ; }

        [RuntimeExport("memset")]
        static unsafe void MemSet(byte* ptr, int c, int count)
        {
            for (byte* p = ptr; p < ptr + count; p++)
                *p = (byte)c;
        }

        [RuntimeExport("memcpy")]
        static unsafe void MemCpy(byte* dest, byte* src, ulong count)
        {
            for (ulong i = 0; i < count; i++) dest[i] = src[i];
        }

        //[RuntimeExport("RhpFallbackFailFast")]
        //static void RhpFallbackFailFast() { while (true) ; }

        [RuntimeExport("RhpReversePInvoke2")]
        static void RhpReversePInvoke2(IntPtr frame) { }

        [RuntimeExport("RhpReversePInvokeReturn2")]
        static void RhpReversePInvokeReturn2(IntPtr frame) { }

        //[RuntimeExport("RhpReversePInvoke")]
        //static void RhpReversePInvoke(IntPtr frame) { }

       // [RuntimeExport("RhpReversePInvokeReturn")]
       // static void RhpReversePInvokeReturn(IntPtr frame) { }

        //[RuntimeExport("RhpPInvoke")]
        //static void RhpPinvoke(IntPtr frame) { }

        //[RuntimeExport("RhpPInvokeReturn")]
        //static void RhpPinvokeReturn(IntPtr frame) { }

        [RuntimeExport("RhpNewFast")]
        static unsafe object RhpNewFast(EEType* pEEType)
        {
            var size = pEEType->BaseSize;

            // Round to next power of 8
            if (size % 8 > 0)
                size = ((size / 8) + 1) * 8;

            var data = malloc(size);
            var obj = Unsafe.As<IntPtr, object>(ref data);
            MemSet((byte*)data,0, (int)size);
            *(IntPtr*)data = (IntPtr)pEEType;

            return obj;
        }
        
        [DllImport("*")]
        public static extern nint malloc(ulong size);

        [RuntimeExport("RhpNewArray")]
        internal static unsafe object RhpNewArray(EEType* pEEType, int length)
        {
            var size = pEEType->BaseSize + (ulong)length * pEEType->ComponentSize;

            // Round to next power of 8
            if (size % 8 > 0)
                size = ((size / 8) + 1) * 8;

            var data = malloc(size);
            var obj = Unsafe.As<IntPtr, object>(ref data);
            MemSet((byte*)data,0, (int)size);
            *(IntPtr*)data = (IntPtr)pEEType;

            var b = (byte*)data;
            b += sizeof(IntPtr);
            MemCpy(b, (byte*)(&length), sizeof(int));

            return obj;
        }

        /*
        [RuntimeExport("RhpAssignRef")]
        static unsafe void RhpAssignRef(void** address, void* obj)
        {
            *address = obj;
        }

        [RuntimeExport("RhpByRefAssignRef")]
        static unsafe void RhpByRefAssignRef(void** address, void* obj)
        {
            *address = obj;
        }*/
        /*
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
                ptr += sizeof(void*);   // Array length is padded to 8 bytes on 64-bit
                ptr += index * array.m_pEEType->ComponentSize;  // Component size should always be 8, seeing as it's a pointer...
                var pp = (IntPtr*)ptr;
                *pp = Unsafe.As<object, IntPtr>(ref obj);
            }
        }
        */
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

        public static void InitializeModules(IntPtr Modules) 
        {
            for (int i = 0; ; i++)
            {
                if (((IntPtr*)Modules)[i].Equals(IntPtr.Zero))
                    break;

                var header = (ReadyToRunHeader*)((IntPtr*)Modules)[i];
                var sections = (ModuleInfoRow*)(header + 1);

                if(header->Signature != ReadyToRunHeaderConstants.Signature) 
                {
                    FailFast();
                }

                for (int k = 0; k < header->NumberOfSections; k++)
                {
                    if (sections[k].SectionId == ReadyToRunSectionType.GCStaticRegion)
                        InitializeStatics(sections[k].Start, sections[k].End);

                    if (sections[k].SectionId == ReadyToRunSectionType.EagerCctor)
                        RunEagerClassConstructors(sections[k].Start, sections[k].End);
                }
            }

            DateTime.s_daysToMonth365 = new int[]{
                0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
            DateTime.s_daysToMonth366 = new int[]{
                0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };
        }


        private static unsafe void RunEagerClassConstructors(IntPtr cctorTableStart, IntPtr cctorTableEnd)
        {
            for (IntPtr* tab = (IntPtr*)cctorTableStart; tab < (IntPtr*)cctorTableEnd; tab++)
            {
                ((delegate*<void>)(*tab))();
            }
        }

        static unsafe void InitializeStatics(IntPtr rgnStart, IntPtr rgnEnd)
        {
            for (IntPtr* block = (IntPtr*)rgnStart; block < (IntPtr*)rgnEnd; block++)
            {
                var pBlock = (IntPtr*)*block;
                var blockAddr = (long)(*pBlock);

                if ((blockAddr & GCStaticRegionConstants.Uninitialized) == GCStaticRegionConstants.Uninitialized)
                {
                    var obj = RhpNewFast((EEType*)(blockAddr & ~GCStaticRegionConstants.Mask));

                    if ((blockAddr & GCStaticRegionConstants.HasPreInitializedData) == GCStaticRegionConstants.HasPreInitializedData)
                    {
                        IntPtr pPreInitDataAddr = *(pBlock + 1);
                        fixed(byte* p = &obj.GetRawData())
                        {
                            MemCpy(p, (byte*)pPreInitDataAddr, obj.GetRawDataSize());
                        }
                    }

                    var handle = malloc((ulong)sizeof(IntPtr));
                    *(IntPtr*)handle = Unsafe.As<object, IntPtr>(ref obj);
                    *pBlock = handle;
                }
            }
        }
    }
    
}
