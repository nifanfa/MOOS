using System;
using System.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerServices
{
    public static unsafe class Unsafe
    {
        [Intrinsic]
        public static extern ref T Add<T>(ref T source, int elementOffset);

        [Intrinsic]
        public static extern ref TTo As<TFrom, TTo>(ref TFrom source);

        [Intrinsic]
        public static extern T As<T>(object value) where T : class;

        [Intrinsic]
        public static extern void* AsPointer<T>(ref T value);

        [Intrinsic]
        public static extern ref T AsRef<T>(void* pointer);

        public static ref T AsRef<T>(IntPtr pointer)
            => ref AsRef<T>((void*)pointer);

        [Intrinsic]
        public static extern int SizeOf<T>();

        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
        {
            for (; ; );
        }

        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref T AddByteOffset<T>(ref T source, nuint byteOffset)
        {
            return ref AddByteOffset(ref source, (IntPtr)(void*)byteOffset);
        }
    }
}