using Kernel;

namespace System.Collections.Generic
{
    public class List<T>
    {
        private T[] _value;

        public int Count = 0;

        public List(int initsize = 256)
        {
            _value = new T[initsize];
        }

        public T this[int index]
        {
            get
            {
                return _value[index];
            }
            set 
            {
                _value[index] = value;
            }
        }

        public void Add(T t)
        {
            _value[Count] = t;
            Count++;
        }

        public void Insert(int index,T item) 
        {
            for(int i = Count; i > index; i--) 
            {
                _value[i] = _value[i - 1];
            }
            _value[index] = item;
            Count++;
        }
    }
}
