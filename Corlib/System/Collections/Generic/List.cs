using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    /// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class List<T> : IList<T>, IList, IReadOnlyList<T>
    {
        private const int _defaultCapacity = 4;

        private static readonly T[] _emptyArray = new T[0];

        private T[] _items;
        private int _size;
        private int _version;
        private readonly Object _syncRoot;

        public List()
        {
            _items = _emptyArray;
            _size = 0;
        }

        public List(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            if (capacity == 0)
                _items = _emptyArray;
            else
                _items = new T[capacity];
        }

        public List(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var c = collection as ICollection<T>;
            if (c != null)
            {
                var count = c.Count;
                if (count == 0)
                {
                    _items = _emptyArray;
                }
                else
                {
                    _items = new T[count];
                    c.CopyTo(_items, 0);
                    _size = count;
                }
            }
            else
            {
                _size = 0;
                _items = _emptyArray;

                // This enumerable could be empty. Let Add handle resizing.
                // Note that the default capacity is 4 so Add will only begin resizing after 4 elements.

                using (var en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                        Add(en.Current);
                }
            }
        }

        public int Capacity
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return _items.Length;
            }
            set
            {
                if (value < _size)
                    //ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
                    throw new ArgumentOutOfRangeException(nameof(value));

                Contract.EndContractBlock();

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                            Copy(_items, 0, newItems, 0, _size);
                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return _size;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }



        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotImplementedException();
                /*
				 if( _syncRoot == null) {
                    System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);    
                }
                return _syncRoot;*/
            }
        }

        /// <summary>
        /// Gets or sets the T at the specified index.
        /// </summary>
        /// <value></value>
        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_size)
                {
                    //  ThrowHelper.ThrowArgumentOutOfRange_IndexException();
                    throw new ArgumentOutOfRangeException();
                }
                Contract.EndContractBlock();
                return _items[index];
            }
            set
            {
                if ((uint)index >= (uint)_size)
                {
                    //  ThrowHelper.ThrowArgumentOutOfRange_IndexException();
                    throw new ArgumentOutOfRangeException();
                }
                Contract.EndContractBlock();
                _items[index] = value;
                _version++;
            }
        }
        //tt

        private static bool IsCompatibleObject(object value)
        {
            // Non-null values are fine. Only accept nulls if T is a class or Nullable<U>.
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>.
            return ((value is T) || (value == null && default(T) == null));
        }

        Object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                //ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);

                throw new ArgumentNullException();

                //TODO fix
                try
                {
                    this[index] = (T)value;
                }
                catch (Exception e)
                {
                    //ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
                }
            }
        }




        private void Copy(T[] source, int sourceIndex, T[] destination, int destinationIndex, int size)
        {
            for (int i = 0; i < size; i++)
            {
                destination[i + destinationIndex] = source[i + sourceIndex];
            }
        }

        private void Copy(T[] source, int sourceIndex, Array destination, int destinationIndex, int size)
        {
            Copy(source, sourceIndex, (T[])destination, destinationIndex, size);
        }

        int ICollection.Count
        {
            get { return _size; }
        }

        public void Add(T item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);

            _items[_size++] = item;
            _version++;
        }

        int IList.Add(object value)
        {
            if (!IsCompatibleObject(value))
                throw new ArgumentException("item is of a type that is not assignable to the IList", nameof(value));
            Add((T)value);
            return Count - 1;
        }


        public void AddRange(IEnumerable<T> collection)
        {
            Contract.Ensures(Count >= Contract.OldValue(Count));

            InsertRange(_size, collection);
        }

        /*	public IReadOnlyCollection<T> AsReadOnly()
			{
				Contract.Ensures(Contract.Result<ReadOnlyCollection<T>>() != null);
				return new ReadOnlyCollection<T>(this);
			}*/
        //TODO FIXXXXXXXXXXXXXXXXXXXXX

        private void EnsureCapacity(int size)
        {
            if (_items.Length < size)
            {
                var newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if (newCapacity < size) newCapacity = size;
                Capacity = newCapacity;
            }
        }










        public void Clear()
        {
            _size = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_items[i].Equals(item))
                    return true;
            }
            return false;
        }

        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value))
                return Contains((T)value);
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            Copy(_items, 0, array, arrayIndex, _size);
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            Copy(_items, 0, array, arrayIndex, _size);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_items[i].Equals(item))
                    return i;
            }
            return -1;
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
                return IndexOf((T)value);
            return -1;
        }

        public void Insert(int index, T item)
        {
            EnsureCapacity(_size + 1);

            _size++;
            for (int i = index; i < _size; i++)
            {
                _items[i] = _items[i + 1];
            }

            _items[index] = item;
        }

        void IList.Insert(int index, object value)
        {
            if (!IsCompatibleObject(value))
                throw new ArgumentException("item is of a type that is not assignable to the IList", nameof(value));
            Insert(index, (T)value);
        }

        public bool Remove(T item)
        {
            int at = IndexOf(item);

            if (at < 0)
                return false;

            RemoveAt(at);

            return true;
        }

        void IList.Remove(object value)
        {
            if (IsCompatibleObject(value))
                Remove((T)value);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            _size--;

            for (int i = index; i < _size; i++)
            {
                _items[i] = _items[i + 1];
            }

            _items[_size] = default(T);
        }



        public T[] ToArray()
        {
            var array = new T[_size];
            CopyTo(array, 0);
            return array;
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                //ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
                throw new ArgumentNullException();
            }

            if ((uint)index > (uint)_size)
            {
                //ThrowHelper.ThrowArgumentOutOfRange_IndexException();
                throw new ArgumentOutOfRangeException();
            }
            Contract.EndContractBlock();

            ICollection<T> c = collection as ICollection<T>;
            if (c != null)
            {    // if collection is ICollection<T>
                int count = c.Count;
                if (count > 0)
                {
                    EnsureCapacity(_size + count);
                    if (index < _size)
                    {
                        Array.Copy(_items, index, _items, index + count, _size - index);
                    }

                    // If we're inserting a List into itself, we want to be able to deal with that.
                    if (this == c)
                    {
                        // Copy first part of _items to insert location
                        Array.Copy(_items, 0, _items, index, index);
                        // Copy last part of _items back to inserted location
                        Array.Copy(_items, index + count, _items, index * 2, _size - index);
                    }
                    else
                    {
                        c.CopyTo(_items, index);
                    }
                    _size += count;
                }
            }
            else if (index < _size)
            {
                // We're inserting a lazy enumerable. Call Insert on each of the constituent items.
                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
            else
            {
                // We're adding a lazy enumerable because the index is at the end of this list.
                AddEnumerable(collection);
            }
            _version++;
        }

        private void AddEnumerable(IEnumerable<T> enumerable)
        {
            Contract.Assert(enumerable != null);
            Contract.Assert(!(enumerable is ICollection<T>), "We should have optimized for this beforehand.");

            using (IEnumerator<T> en = enumerable.GetEnumerator())
            {
                _version++; // Even if the enumerable has no items, we can update _version.

                while (en.MoveNext())
                {
                    // Capture Current before doing anything else. If this throws
                    // an exception, we want to make a clean break.
                    T current = en.Current;

                    if (_size == _items.Length)
                    {
                        EnsureCapacity(_size + 1);
                    }

                    _items[_size++] = current;
                }
            }
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly List<T> list;
            private int index;
            private T current;

            internal Enumerator(List<T> list)
            {
                this.list = list;
                index = 0;
                current = default(T);
            }

            public T Current
            {
                get { return current; }
            }

            object IEnumerator.Current
            {
                get { return current; }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                List<T> localList = list;

                if (((uint)index < (uint)localList._size))
                {
                    current = localList._items[index];
                    index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                index = list._size + 1;
                current = default(T);
                return false;
            }

            void IEnumerator.Reset()
            {
                index = 0;
                current = default(T);
            }

        }
    }
}