// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace System.Collections.Generic
{
    public class List<T>
    {
        private readonly T[] _value;

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
            return IndexOf(item) != -1;
        }

        public T[] ToArray()
        {
            T[] array = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = this[i];
            }
            return array;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(_value, item);
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
