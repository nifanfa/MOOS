/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
#if Kernel
using Kernel;
using Kernel.Misc;
#endif
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Internal.Runtime.CompilerHelpers
{
    internal static class InteropHelpers
    {
        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct MethodFixupCell
        {
            public IntPtr Target;
            public IntPtr MethodName;
            public ModuleFixupCell* Module;
            public CharSet CharSetMangling;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct ModuleFixupCell
        {
            public IntPtr Handle;
            public IntPtr ModuleName;
            public EETypePtr CallingAssemblyType;
            public uint DllImportSearchPathAndCookie;
        }

#if Kernel
#else
        [DllImport("*")]
        private static extern ulong _int80h(ulong p1);
#endif

        internal static unsafe IntPtr ResolvePInvoke(MethodFixupCell* pCell)
        {
#if Kernel
            Console.Write("Method Name: ");
            Console.WriteLine(string.FromASCII(pCell->Module->ModuleName, strings.strlen((byte*)pCell->Module->ModuleName)));
            //Return the pointer of method
            return (IntPtr)(delegate*<void>)&Hello;
#else
            return (IntPtr)_int80h((ulong)pCell);
#endif
        }

#if Kernel
        internal static void Hello() 
        {
            Panic.Error("Not implemented, check out Internal.Runtime.CompilerHelpers.InteropHelpers");
        }
#endif

        internal static unsafe byte* StringToAnsiString(string str, bool bestFit, bool throwOnUnmappableChar) 
        {
            //String will become char* if we use DllImport
            //No Ansi support, Return unicode
            fixed (char* ptr = str) return (byte*)ptr;
        }

        internal unsafe static void CoTaskMemFree(void* p)
        {
            //TO-DO
        }
    }
}
