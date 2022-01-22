using System;
using System.Runtime.InteropServices;

namespace NativeTypeWrappers
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ReadonlyNativeString
    {
        readonly IntPtr _pointer;

        public ReadonlyNativeString(IntPtr ptr)
        {
            _pointer = ptr;
        }

        public static unsafe implicit operator ReadonlyNativeString(char* ptr)
            => new ReadonlyNativeString((IntPtr)ptr);

        public override string ToString()
            => new string(_pointer);
    }
}