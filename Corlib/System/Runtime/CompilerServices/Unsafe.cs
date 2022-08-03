using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;

namespace System.Runtime.CompilerServices
{
    public static unsafe class Unsafe
    {
      
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
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T As<T>(object value) where T : class
        {
            throw new PlatformNotSupportedException();
        }

        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TTo As<TFrom, TTo>(ref TFrom source)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Returns the size of an object of the given type parameter.
        /// </summary>
        [Intrinsic]
        [NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>()
        {
            throw new PlatformNotSupportedException();
        }


        [Intrinsic]
        public static extern void* AsPointer<T>(ref T value);

        //[Intrinsic]
        //[NonVersionable]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static ref T AsRef<T>(void* source)
        //{
        //	return ref Unsafe.As<byte, T>(ref *(byte*)source);
        //}
    }
}
