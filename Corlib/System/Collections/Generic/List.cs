using Kernel;

namespace System.Collections.Generic
{
    public class List<T>
    {
        private T[] value;

        public int Count = 0;

        public List(int initsize)
        {
            value = new T[initsize];
        }

        public T this[int index]
        {
            get
            {
                return value[index];
            }
        }

        public void Add(T t)
        {
            value[Count] = t;
            Count++;
        }
    }
}
