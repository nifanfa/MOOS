using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    // Provides the Create factory method for KeyValuePair<TKey, TValue>.
    public static class KeyValuePair
    {
        // Creates a new KeyValuePair<TKey, TValue> from the given values.
        public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }

    // A KeyValuePair holds a key and a value from a dictionary.
    // It is used by the IEnumerable<T> implementation for both IDictionary<TKey, TValue>
    // and IReadOnlyDictionary<TKey, TValue>.
    [Serializable]
    public struct KeyValuePair<TKey, TValue>
    {
        private readonly TKey key;
        private readonly TValue value;

        public KeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey Key
        {
            get { return key; }
        }

        public TValue Value
        {
            get { return value; }
        }

        public override string ToString()
        {//TODO implmenet
         //StringBuilder s = StringBuilderCache.Acquire();

            //	s.Append('[');
            //	if (Key != null)
            //	{
            //		s.Append(Key.ToString());
            //	}
            //	s.Append(", ");
            //	if (Value != null)
            //	{
            //		s.Append(Value.ToString());
            //	}
            //	s.Append(']');
            //	return StringBuilderCache.GetStringAndRelease(s);

            return "[" + Key.ToString() + ", " + Value.ToString() + "]";
        }

        // BLOCKED (do not add now): [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out TKey key, out TValue value)
        {
            key = Key;
            value = Value;
        }
    }
}
