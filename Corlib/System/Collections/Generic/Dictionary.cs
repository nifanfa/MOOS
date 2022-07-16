using Internal.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    public class Dictionary<TKey, TValue>
    {
        public TValue this[TKey key]
        {
            get
            {
                return Values[Keys.IndexOf(key)];
            }
            set
            {
                Values[Keys.IndexOf(key)] = value;
            }
        }

        public int Count { get { return Values.Count; } }

        public void Remove(TKey key)
        {
            Values.Remove(Values[Keys.IndexOf(key)]);
            Keys.Remove(key);
        }

        public Dictionary()
        {
            Keys = new List<TKey>();
            Values = new List<TValue>();
        }


        public bool ContainsKey(TKey key)
        {
            return Keys.IndexOf(key) != -1;
        }

        public bool ContainsValue(TValue value)
        {
            return Values.IndexOf(value) != -1;
        }

        public void Add(TKey key, TValue value)
        {
            Keys.Add(key);
            Values.Add(value);
        }

        public void Clear()
        {
            Keys.Clear();
            Values.Clear();
        }

        public override void Dispose()
        {
            Keys.Clear();
            Values.Clear();
            Values.Dispose();
            Keys.Dispose();
            base.Dispose();
        }

        public List<TKey> Keys;
        public List<TValue> Values;
    }
}