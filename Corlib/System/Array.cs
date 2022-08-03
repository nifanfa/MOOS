using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// Array
    /// </summary>
    public unsafe abstract class Array : ICloneable, IList, ICollection, IEnumerable, IStructuralComparable, IStructuralEquatable
    {
        [DllImport("*")]
        private static extern void Panic(string message);

        private readonly int length;

        public int Length
        {
            get
            {
                return length;
            }
        }

        internal const int MaxArrayLength = 0X7FEFFFFF;
        internal const int MaxByteArrayLength = 0x7FFFFFC7;

        /// <summary>
        /// Gets the rank (number of dimensions) of the Array. For example, a one-dimensional array returns 1, a two-dimensional array returns 2, and so on.
        /// </summary>
        public int Rank
        {
            // TODO: support multidimensional arrays
            get { return 1; }
        }

        // This ctor exists solely to prevent C# from generating a protected .ctor that violates the surface area.
        private protected Array() { }

        /// <summary>
        /// SetValue
        /// </summary>
        public void SetValue(object value, params int[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException(nameof(indices));
            if (Rank != indices.Length)
                throw new ArgumentException("The number of dimensions in the current Array is not equal to the number of elements in indices.", nameof(indices));

            // TODO
        }

        /// <summary>
        /// GetValue
        /// </summary>
        public object GetValue(params int[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException(nameof(indices));
            if (Rank != indices.Length)
                throw new ArgumentException("The number of dimensions in the current Array is not equal to the number of elements in indices.", nameof(indices));

            // TODO
            return null;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length, bool reliable);

        /// <summary>
        /// Copies a range of elements from an Array starting at the specified source index and pastes them to another Array starting at the specified destination index.
        /// The length and the indexes are specified as 32-bit integers.
        /// </summary>
        public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
        {
            Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length, true);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int GetLength(int dimension);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int GetLowerBound(int dimension);

        public int GetUpperBound(int dimension)
        {
            return GetLowerBound(dimension) + GetLength(dimension) - 1;
        }

        object ICloneable.Clone()
        {
            //TODO
            return null;
        }

        bool IList.IsFixedSize
        {
            get { return true; }
        }

        bool IList.IsReadOnly
        {
            get { return true; }
        }

        object IList.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        int IList.Add(object value)
        {
            // NOT SUPPORTED
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            // NOT SUPPORTED
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            // NOT SUPPORTED
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            // NOT SUPPORTED
            throw new NotSupportedException();
        }

        int ICollection.Count
        {
            get
            {
                return length;
            }
        }

        bool ICollection.IsSynchronized
        {
            //TODO
            get
            {
                return true;
            }
        }

        object ICollection.SyncRoot
        {
            //TODO
            get
            {
                return this;
            }
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return new SZArrayEnumerator(this);
        }

        public static T[] Empty<T>()
        {
            return EmptyArray<T>.Value;
        }

        // TODO Support multidimensional arrays
        [Serializable]
        private sealed class SZArrayEnumerator : IEnumerator
        {
            private readonly Array array;
            private int currentPosition;
            private readonly int length;

            public SZArrayEnumerator(Array array)
            {
                if (array.Rank != 1 || array.GetLowerBound(0) != 0)
                    throw new InvalidOperationException("SZArrayEnumerator only works on single dimension arrays with a lower bound of zero.");
                this.array = array;
                currentPosition = -1;
                length = array.Length;
            }

            public object Current
            {
                get
                {
                    if (currentPosition < 0)
                        throw new InvalidOperationException("Enumeration has not started.");
                    if (currentPosition >= length)
                        throw new InvalidOperationException("Enumeration has already ended.");
                    return array.GetValue(currentPosition);
                }
            }

            public bool MoveNext()
            {
                if (currentPosition < length)
                    currentPosition++;
                if (currentPosition < length)
                    return true;
                else
                    return false;
            }

            public void Reset()
            {
                currentPosition = -1;
            }
        }

        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            throw new NotImplementedException();
        }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }

        // ----------- STATIC METHODS ----------- //

        public static int IndexOf(Array array, object value)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            // TODO
            throw new NotImplementedException();
        }

        // ---------------------------------------------- //
        // -------------------SZARRAYS------------------- //
        // ---------------------------------------------- //
        // The "this" in methods inside this class is not an instance of SZArrayHelper.
        // It is actually an array. The generic type parameter is filled in by the compiler.
        // This only occurs for SZ arrays. The methods are attached and the generic interfaces are added.
        private sealed class SZArrayHelper
        {
            private SZArrayHelper()
            {
                throw new InvalidOperationException("Cannot instantiate this class!!!");
            }

            // -----------------------------------------------------------
            // ------- Implement IEnumerable<T> interface methods --------
            // -----------------------------------------------------------
            private IEnumerator<T> GetEnumerator<T>()
            {
                T[] _this = RuntimeHelpers.UnsafeCast<T[]>(this);
                int length = _this.length;
                return (length == 0) ? SZGenericArrayEnumerator<T>.Empty : new SZGenericArrayEnumerator<T>(_this, length);
            }

            // -----------------------------------------------------------
            // ------- Implement ICollection<T> interface methods --------
            // -----------------------------------------------------------
            private void CopyTo<T>(T[] array, int index)
            {
                if (array != null && array.Rank != 1)
                    throw new ArgumentException("Multidimensional arrays are not supported");

                T[] _this = RuntimeHelpers.UnsafeCast<T[]>(this);
                Copy(_this, 0, array, index, _this.Length);
            }

            private int get_Count<T>()
            {
                T[] _this = RuntimeHelpers.UnsafeCast<T[]>(this);
                return _this.Length;
            }

            // -----------------------------------------------------------
            // ---------- Implement IList<T> interface methods -----------
            // -----------------------------------------------------------
            private T get_Item<T>(int index)
            {
                T[] _this = RuntimeHelpers.UnsafeCast<T[]>(this);
                if ((uint)index >= (uint)_this.Length)
                    throw new ArgumentOutOfRangeException("index");

                return _this[index];
            }

            private void set_Item<T>(int index, T value)
            {
                T[] _this = RuntimeHelpers.UnsafeCast<T[]>(this);
                if ((uint)index >= (uint)_this.Length)
                    throw new ArgumentOutOfRangeException("index");

                _this[index] = value;
            }

            private void Add<T>(T value)
            {
                // NOT SUPPORTED
                throw new NotSupportedException();
            }

            private bool Contains<T>(T value)
            {
                T[] _this = RuntimeHelpers.UnsafeCast<T[]>(this);
                return Array.IndexOf(_this, value) != -1;
            }

            private bool get_IsReadOnly<T>()
            {
                return true;
            }

            private void Clear<T>()
            {
                // NOT SUPPORTED
                throw new NotSupportedException();
            }

            private int IndexOf<T>(T value)
            {
                T[] _this = RuntimeHelpers.UnsafeCast<T[]>(this);
                return Array.IndexOf(_this, value);
            }

            private void Insert<T>(int index, T value)
            {
                // NOT SUPPORTED
                throw new NotSupportedException();
            }

            private bool Remove<T>(T value)
            {
                // NOT SUPPORTED
                throw new NotSupportedException();
            }

            private void RemoveAt<T>(int index)
            {
                // NOT SUPPORTED
                throw new NotSupportedException();
            }

            // This is a normal generic Enumerator for SZ arrays.
            // It doesn't have any of the "this" stuff that SZArrayHelper does.
            [Serializable]
            private sealed class SZGenericArrayEnumerator<T> : IEnumerator<T>
            {
                private readonly T[] array;
                private int currentPosition;
                private readonly int length;

                internal static readonly SZGenericArrayEnumerator<T> Empty = new SZGenericArrayEnumerator<T>(null, -1);

                internal SZGenericArrayEnumerator(T[] array, int length)
                {
                    if (!((array == null && length == -1) || (array.Rank == 1 || array.GetLowerBound(0) == 0)))
                        throw new InvalidOperationException("SZGenericArrayEnumerator only works on single dimension arrays with a lower bound of zero.");
                    this.array = array;
                    this.length = length;
                    currentPosition = -1;
                }

                public T Current
                {
                    get
                    {
                        if (currentPosition < 0)
                            throw new InvalidOperationException("Enumeration has not started.");
                        if (currentPosition >= length)
                            throw new InvalidOperationException("Enumeration has already ended.");
                        return array[currentPosition];
                    }
                }

                public bool MoveNext()
                {
                    if (currentPosition < length)
                        currentPosition++;
                    if (currentPosition < length)
                        return true;
                    else
                        return false;
                }

                public void Reset()
                {
                    currentPosition = -1;
                }

                public void Dispose()
                {
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }

                void IEnumerator.Reset()
                {
                    currentPosition = -1;
                }
            }
        }
    }


    public class Array<T> : Array { }

    internal static class EmptyArray<T>
    {
        internal static readonly T[] Value = new T[0];
    }
}