using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    public struct DictionaryEntry
    {
        private Object _key;
        private Object _value;

        // Constructs a new DictionaryEnumerator by setting the Key
        // and Value fields appropriately.
        public DictionaryEntry(Object key, Object value)
        {
            _key = key;
            _value = value;
        }

        public Object Key
        {
            get
            {
                return _key;
            }

            set
            {
                _key = value;
            }
        }

        public Object Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        // BLOCKED (do not add now): [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out object key, out object value)
        {
            key = Key;
            value = Value;
        }
    }
}
