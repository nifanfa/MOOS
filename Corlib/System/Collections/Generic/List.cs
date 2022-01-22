using Kernel;

namespace System.Collections.Generic
{
    public class List<T>
    {
        private T[] value;

        public ulong Count = 0;

        public List(ulong initsize)
        {
            value = new T[initsize];
        }

        public T this[ulong index]
        {
            get
            {
                return value[index];
            }
        }

        public void Add(T t)
        {
            return;
            value[Count] = t;
            Count++;
        }
    }
}
