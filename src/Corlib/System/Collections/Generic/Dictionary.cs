/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

using Internal.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    public class Dictionary<TKey, TValue>
    {
        public TValue this[TKey key]
        {
            get => values[keys.IndexOf(key)];
            set => values[keys.IndexOf(key)] = value;
        }

        public int Count => values.Count;

        public TKey[] ToKeyArray()
        {
            return keys.ToArray();
        }

        public TValue[] ToValuesArray()
        {
            return values.ToArray();
        }

        public void Remove(TKey key)
        {
            values.Remove(values[keys.IndexOf(key)]);
            keys.Remove(key);
        }

        public Dictionary()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }


        public bool ContainsKey(TKey key)
        {
            return keys.Contains(key);
        }

        public bool ContainsValue(TValue value)
        {
            return values.Contains(value);
        }

        public void Add(TKey key, TValue value)
        {
            keys.Add(key);
            values.Add(value);
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public new void Dispose()
        {
            keys.Clear();
            values.Clear();
            values.Dispose();
            keys.Dispose();
            object obj = this;
            Allocator.Free(Unsafe.As<object, IntPtr>(ref obj));
        }

        private readonly List<TKey> keys;
        private readonly List<TValue> values;
    }
}
