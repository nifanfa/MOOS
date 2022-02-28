// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence

using System.Runtime.CompilerServices;
using Internal.Runtime.CompilerHelpers;
using Internal.Runtime.CompilerServices;

namespace System
{
    public abstract unsafe partial class Array
    {
        internal int _numComponents;

        // We impose limits on maximum array length in each dimension to allow efficient
        // implementation of advanced range check elimination in future.
        // Keep in sync with vm\gcscan.cpp and HashHelpers.MaxPrimeArrayLength.
        // The constants are defined in this method: inline SIZE_T MaxArrayLength(SIZE_T componentSize) from gcscan
        // We have different max sizes for arrays with elements of size 1 for backwards compatibility
        internal const int MaxArrayLength = 0X7FEFFFFF;
        internal const int MaxByteArrayLength = 0x7FFFFFC7;

        // This is the threshold where Introspective sort switches to Insertion sort.
        // Empirically, 16 seems to speed up most cases without slowing down others, at least for integers.
        // Large value types may benefit from a smaller number.
        internal const int IntrosortSizeThreshold = 16;

        // This ctor exists solely to prevent C# from generating a protected .ctor that violates the surface area.
        private protected Array() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref int GetRawMultiDimArrayBounds()
        {
            return ref Unsafe.AddByteOffset(ref _numComponents, (nuint)sizeof(void*));
        }
        public static unsafe Array NewMultiDimArray(EETypePtr eeType, int* pLengths, int rank)
        {
            ulong totalLength = 1;

            for (int i = 0; i < rank; i++)
            {
                int length = pLengths[i];
                if (length > MaxArrayLength)
                {
                    /*throw new Exception*/
                    Kernel.Misc.Panic.Error("Length of array is too large, Max: " + MaxArrayLength);
                }

                totalLength *= (ulong)length;
            }

            object v = StartupCodeHelpers.RhpNewArray(eeType.Value, (int)totalLength);
            Array ret = Unsafe.As<object, Array>(ref v);

            ref int bounds = ref ret.GetRawMultiDimArrayBounds();
            for (int i = 0; i < rank; i++)
            {
                Unsafe.Add(ref bounds, i) = pLengths[i];
            }

            return ret;
        }

        public int Length => _numComponents;

#nullable enable
        public object? this[int i]
#nullable disable
        {
            get => GetValue(i);
            set => SetValue(value, i);
        }

#nullable enable
        public static void Resize<T>(ref T[]? array, int newSize)
#nullable disable
        {
            if (newSize < 0)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }


#nullable enable
            T[]? larray = array;
#nullable disable
            if (larray == null)
            {
                array = new T[newSize];
                return;
            }

            if (larray.Length != newSize)
            {
                T[] newArray = new T[newSize];
                Copy(larray, ref newArray, 0, larray.Length > newSize ? newSize : larray.Length);
                array = newArray;
            }
        }

        public static Array CreateInstance<T>(uint length)
        {
            if (length < MaxArrayLength)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            return new T[length];
        }

        public static void Copy(Array sourceArray, ref Array destinationArray)
        {
            Copy(sourceArray, ref destinationArray, 0);
        }

        public static void Copy<T>(T[] sourceArray, ref T[] destinationArray)
        {
            Copy(sourceArray, ref destinationArray, 0);
        }

        public static void Copy(Array sourceArray, ref Array destinationArray, int startIndex)
        {
            Copy(sourceArray, ref destinationArray, startIndex, sourceArray.Length);
        }

        public static void Copy<T>(T[] sourceArray, ref T[] destinationArray, int startIndex)
        {
            Copy(sourceArray, ref destinationArray, startIndex, sourceArray.Length);
        }

        public static void Copy(Array sourceArray, ref Array destinationArray, int startIndex, int count)
        {
            if (sourceArray == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            if (destinationArray == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            if (startIndex < 0)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            if (destinationArray.Length < sourceArray.Length - count)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            if (count <= 0)
            {
                return;
            }

            int x = 0;
            object[] temp = new object[count];
            for (int i = startIndex; i < startIndex + count; i++)
            {
                Kernel.Console.WriteLine(i);
                temp[x] = sourceArray[i];
                x++;
            }
            destinationArray = temp;
        }

        public static void Copy<T>(T[] sourceArray, ref T[] destinationArray, int startIndex, int count)
        {
            if (sourceArray == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            if (destinationArray == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            if (startIndex < 0)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            if (destinationArray.Length > sourceArray.Length - count)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }
            if (count <= 0)
            {
                return;
            }

            int x = 0;
            T[] temp = new T[count];
            for (int i = startIndex; i < startIndex + count; i++)
            {
                temp[x] = sourceArray[i];
                x++;
            }
            destinationArray = temp;
        }

#nullable enable
        public object? GetValue(long index)
#nullable disable
        {
            int iindex = (int)index;
            if (index != iindex)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            return GetValue(iindex);
        }

#nullable enable
        public object? GetValue(long index1, long index2)
#nullable disable
        {
            int iindex1 = (int)index1;
            int iindex2 = (int)index2;

            if (index1 != iindex1)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (index2 != iindex2)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            return GetValue(iindex1, iindex2);
        }

#nullable enable
        public object? GetValue(long index1, long index2, long index3)
#nullable disable
        {
            int iindex1 = (int)index1;
            int iindex2 = (int)index2;
            int iindex3 = (int)index3;

            if (index1 != iindex1)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (index2 != iindex2)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (index3 != iindex3)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            return GetValue(iindex1, iindex2, iindex3);
        }

#nullable enable
        public void SetValue(object? value, long index)
#nullable disable
        {
            int iindex = (int)index;

            if (index != iindex)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            SetValue(value, iindex);
        }

#nullable enable
        public void SetValue(object? value, long index1, long index2)
#nullable disable
        {
            int iindex1 = (int)index1;
            int iindex2 = (int)index2;

            if (index1 != iindex1)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (index2 != iindex2)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            SetValue(value, iindex1, iindex2);
        }

#nullable enable
        public void SetValue(object? value, long index1, long index2, long index3)
#nullable disable
        {
            int iindex1 = (int)index1;
            int iindex2 = (int)index2;
            int iindex3 = (int)index3;

            if (index1 != iindex1)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (index2 != iindex2)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (index3 != iindex3)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            SetValue(value, iindex1, iindex2, iindex3);
        }


        // Returns an object appropriate for synchronizing access to this
        // Array.
        public object SyncRoot => this;

        // Is this Array read-only?
        public static bool IsReadOnly => false;

        public static bool IsFixedSize => true;

        public static bool IsSynchronized => false;


        private static class EmptyArray<T>
        {
#pragma warning disable CA1825 // this is the implementation of Array.Empty<T>()
            internal static readonly T[] Value = new T[0];
#pragma warning restore CA1825
        }

        public static T[] Empty<T>()
        {
            return EmptyArray<T>.Value;
        }

        public static bool Exists<T>(T[] array, T match)
        {
            return IndexOf(array, match) != -1;
        }

        public static void Fill<T>(T[] array, T value)
        {
            if (array == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument null");
            }

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static void Fill<T>(T[] array, T value, int startIndex, int count)
        {
            if (array == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument null");
            }

            if (startIndex < 0 || startIndex > array.Length)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (count < 0 || startIndex > array.Length - count)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            for (int i = startIndex; i < startIndex + count; i++)
            {
                array[i] = value;
            }
        }

        // Returns the index of the first occurrence of a given value in an array.
        // The array is searched forwards, and the elements of the array are
        // compared to the given value using the Object.Equals method.
        //
#nullable enable
        public static int IndexOf(Array array, object? value)
#nullable disable
        {
            return IndexOf(array, value, 0);
        }

#nullable enable
        public static int IndexOf(Array array, object? value, int startIndex)
#nullable disable
        {
            if (array == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument null");
            }

            if ((uint)startIndex > (uint)array.Length)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            for (int i = startIndex; i < array.Length; i++)
            {
                if (array.GetValue(i).Equals(value))
                {
                    return i;
                }
            }

            return -1;


        }

        public static int IndexOf<T>(T[] array, T value)
        {
            return IndexOf(array, value, 0);
        }

        public static int IndexOf<T>(T[] array, T value, int startIndex)
        {
            if (array == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument null");
            }

            if ((uint)startIndex > (uint)array.Length)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            for (int i = startIndex; i < startIndex + array.Length; i++)
            {
                if (array[i].Equals(value))
                {
                    return i;
                }
            }
            return -1;
        }
        // Reverses all elements of the given array. Following a call to this
        // method, an element previously located at index i will now be
        // located at index length - i - 1, where length is the
        // length of the array.
        //
        public static void Reverse(ref Array array)
        {
            Reverse(ref array, 0, array.Length);
        }

        // Reverses the elements in a range of an array. Following a call to this
        // method, an element in the range given by index and count
        // which was previously located at index i will now be located at
        // index index + (index + count - i - 1).
        // Reliability note: This may fail because it may have to box objects.
        //
        public static void Reverse(ref Array array, int index, int length)
        {
            if (array == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument null");
            }

            if (index < 0)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (length < 0)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (array.Length - index < length)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (length <= 1)
            {
                return;
            }
            object[] o = new object[length];
            int x = 0;
            for (int i = array.Length; i <= index; i--)
            {
                o.SetValue(array.GetValue(i), x);
                x++;
            }
            array = o;
        }

        public static void Reverse<T>(ref T[] array)
        {
            Reverse(ref array, 0, array.Length);
        }

        public static void Reverse<T>(ref T[] array, int index, int length)
        {
            if (array == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument null");
            }

            if (index < 0)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (length < 0)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (array.Length - index < length)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error("Argument out of range");
            }

            if (length <= 1)
            {
                return;
            }
            T[] o = new T[length];
            int x = 0;
            for (int i = array.Length; i <= index; i--)
            {
                o.SetValue(array.GetValue(i), x);
                x++;
            }
            array = o;
        }
    }
    public class Array<T> : Array { }
}
