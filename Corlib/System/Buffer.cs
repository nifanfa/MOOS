// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Internal.Runtime.CompilerServices;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using nuint = System.UInt64;
using nint = System.Int64;

namespace System
{
    public partial class Buffer
    {
        // Non-inlinable wrapper around the QCall that avoids polluting the fast path
        // with P/Invoke prolog/epilog.
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe void _ZeroMemory(ref byte b, nuint byteLength)
        {
            fixed (byte* bytePointer = &b)
            {
                memset(bytePointer, 0, (int)byteLength);
            }
        }

        //public static void BulkMoveWithWriteBarrier(ref byte dmem, ref byte smem, nuint size)
        //    => RuntimeImports.RhBulkMoveWithWriteBarrier(ref dmem, ref smem, size);

        public static unsafe void Memcpy(byte* dest, byte* src, int len)
        {
            //Debug.Assert(len >= 0, "Negative length in memcpy!");
            __Memmove(dest, src, (nuint)len);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void __Memmove(byte* dest, byte* src, nuint len) =>
            memcpy(dest, src, (ulong)len);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void __Memmove(ref byte dest, ref byte src, nuint len)
        {
            fixed(byte* pdest = &dest)
            fixed(byte* psrc = &src)
            memcpy(pdest, psrc, (ulong)len);
        }

        // This method has different signature for x64 and other platforms and is done for performance reasons.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Memmove<T>(ref T destination, ref T source, nuint elementCount)
        {
            /*
            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                // Blittable memmove

                __Memmove(
                    ref Unsafe.As<T, byte>(ref destination),
                    ref Unsafe.As<T, byte>(ref source),
                    elementCount * (nuint)Unsafe.SizeOf<T>());
            }
            else
            {
                // Non-blittable memmove
                BulkMoveWithWriteBarrier(
                    ref Unsafe.As<T, byte>(ref destination),
                    ref Unsafe.As<T, byte>(ref source),
                    elementCount * (nuint)Unsafe.SizeOf<T>());
            }
            */

            __Memmove(
                ref Unsafe.As<T, byte>(ref destination),
                ref Unsafe.As<T, byte>(ref source),
                elementCount * (nuint)Unsafe.SizeOf<T>());
        }

        [DllImport("*")]
        public static unsafe extern void memset(byte* ptr, byte c, ulong count);

        [DllImport("*")]
        public static unsafe extern void memcpy(byte* dest, byte* src, ulong count);
    }
}