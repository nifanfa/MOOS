/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

namespace System.Collections.Generic
{
    public class List<T>
    {
        internal T[] _value;

        public int Count = 0;

        public List(int initsize = 256)
        {
            _value = new T[initsize];
        }

        public T this[int index]
        {
            get => _value[index];
            set => _value[index] = value;
        }

        public void Add(T t)
        {
            _value[Count] = t;
            Count++;
        }

        public void Insert(int index, T item)
        {
            for (int i = Count; i > index; i--)
            {
                _value[i] = _value[i - 1];
            }
            _value[index] = item;
            Count++;
        }

        public bool Contains(T item)
        {
            return Array.Exists(ToArray(), item);
        }

        public T[] ToArray()
        {
            T[] array = new T[Count];
            Array.Copy(_value, ref array, 0, Count);
            return array;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(ToArray(), item);
        }
        public bool Remove(T item)
        {
            int at = IndexOf(item);

            if (at < 0)
            {
                return false;
            }

            RemoveAt(at);

            return true;
        }

        public void RemoveAt(int index)
        {
            Count--;

            for (int i = index; i < Count; i++)
            {
                _value[i] = _value[i + 1];
            }

            _value[Count] = default;
        }

        public void Clear()
        {
            Count = 0;
        }

    }
}
