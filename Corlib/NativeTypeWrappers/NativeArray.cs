using System;
using System.Runtime.InteropServices;

namespace NativeTypeWrappers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NativeArray<T> where T : unmanaged
    {
        readonly IntPtr _pointer;

        public unsafe T this[int index]
        {
            get => ((T*)_pointer)[index];
            set => ((T*)_pointer)[index] = value;
        }
    }
}