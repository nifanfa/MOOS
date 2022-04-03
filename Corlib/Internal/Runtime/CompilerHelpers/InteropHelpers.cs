using Kernel;
using Kernel.Misc;
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

        internal static unsafe IntPtr ResolvePInvoke(MethodFixupCell* pCell)
        {
            if (pCell->Target != IntPtr.Zero)
                return pCell->Target;

            Console.Write("Method Name: ");
            Console.WriteLine(string.FromASCII(pCell->MethodName, strings.strlen((byte*)pCell->MethodName)));
            //Return the pointer of method
            return (IntPtr)(delegate*<void>)&Hello;
        }

        internal static void Hello() 
        {
            Panic.Error("Not implemented, check out Internal.Runtime.CompilerHelpers.InteropHelpers");
        }
    }
}
