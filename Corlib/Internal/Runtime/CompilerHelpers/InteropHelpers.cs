/*
 * Copyright(c) 2022 nifanfa, This code is part of the Solution1 licensed under the MIT licence.
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

        internal static unsafe IntPtr ResolvePInvoke(MethodFixupCell* pCell)
        {
            if (pCell->Target != IntPtr.Zero)
                return pCell->Target;

#if Kernel
            Console.Write("Method Name: ");
            Console.WriteLine(string.FromASCII(pCell->MethodName, strings.strlen((byte*)pCell->MethodName)));
            //Return the pointer of method
            return (IntPtr)(delegate*<void>)&Hello;
#endif
            return IntPtr.Zero;
        }

#if Kernel
        internal static void Hello() 
        {
            Panic.Error("Not implemented, check out Internal.Runtime.CompilerHelpers.InteropHelpers");
        }
#endif

        internal static unsafe int StringToAnsiString(string s, byte* buffer, int bufferLength, bool bestFit = false, bool throwOnUnmappableChar = false)
        {
            int convertedBytes = 0;

            fixed (char* pChar = s)
            {
                while (pChar[convertedBytes] != '\0' && convertedBytes < bufferLength) 
                {
                    //No UTF8 support! To ASCII
                    buffer[convertedBytes] = (byte)s[convertedBytes];
                }
                //convertedBytes = Encoding.UTF8.GetBytes(pChar, s.Length, buffer, bufferLength);
            }

            buffer[convertedBytes] = 0;

            return convertedBytes;
        }

        public static void CoTaskMemFree(IntPtr allocatedMemory)
        {
#if Kernel
            Allocator.Free(allocatedMemory);
#endif
        }
    }
}
