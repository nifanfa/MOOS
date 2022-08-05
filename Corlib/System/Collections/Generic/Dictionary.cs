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
                //return Values[IndexOf(key)];
            }
            set
            {
                Values[Keys.IndexOf(key)] = value;
               // Values[IndexOf(key)] = value;
            }
        }

        //improvised
        int IndexOf(TKey key)
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Keys[i].ToString() == key.ToString())
                {
                    return i;
                }
            }
            return -1;
        }

        public int Count { get { return Values.Count; } }

        public void Remove(TKey key)
        {
            Values.RemoveAt(Keys.IndexOf(key));
            Keys.RemoveAt(Keys.IndexOf(key));
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