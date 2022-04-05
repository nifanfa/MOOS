/*
 * Copyright(c) 2022 nifanfa, This code is part of the Solution1 licensed under the MIT licence.
 */

using Internal.Runtime.CompilerHelpers;
using Internal.Runtime.CompilerServices;
#if Kernel
using Kernel;
#endif
using System.Runtime.CompilerServices;

namespace System
{
    public abstract unsafe class Array
    {
#pragma warning disable 649
        // This field should be the first field in Array as the runtime/compilers depend on it
        internal int _numComponents;
#pragma warning restore

        public int Length
        {
            get
            {
                // NOTE: The compiler has assumptions about the implementation of this method.
                // Changing the implementation here (or even deleting this) will NOT have the desired impact
                return _numComponents;
            }
        }

        public const int MaxArrayLength = 0x7FEFFFFF;

#if Kernel
        internal static unsafe Array NewMultiDimArray(EETypePtr eeType, int* pLengths, int rank)
        {
            ulong totalLength = 1;
            bool maxArrayDimensionLengthOverflow = false;

            for (int i = 0; i < rank; i++)
            {
                int length = pLengths[i];
                if (length > MaxArrayLength)
                    maxArrayDimensionLengthOverflow = true;
                totalLength = totalLength * (ulong)length;
            }

            var v = StartupCodeHelpers.RhpNewArray(eeType.Value, (int)totalLength);
            Array ret = Unsafe.As<object, Array>(ref v);

            ref int bounds = ref ret.GetRawMultiDimArrayBounds();
            for (int i = 0; i < rank; i++)
            {
                Unsafe.Add(ref bounds, i) = pLengths[i];
            }

            return ret;
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref int GetRawMultiDimArrayBounds()
        {
            return ref Unsafe.AddByteOffset(ref _numComponents, (nuint)sizeof(void*));
        }
    }

    public class Array<T> : Array { }
}