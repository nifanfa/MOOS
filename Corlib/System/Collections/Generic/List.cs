using Kernel;

namespace System.Collections.Generic
{
    public class List<T>
    {
        private T[] _value;

        public int Count = 0;

        public List(int initsize)
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
    }
}
