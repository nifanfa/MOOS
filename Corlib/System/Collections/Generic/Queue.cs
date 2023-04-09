namespace System.Collections.Generic
{
    public class Queue<T>
    {
        List<T> list;

        public Queue(int initsize = 256)
        {
            list = new List<T>(initsize);
        }

        public T Tail
        {
            get
            {
                if (Count == 0) return default;
                else
                {
                    return list[Count - 1];
                }
            }
        }

        public T Head
        {
            get
            {
                if (Count == 0) return default;
                else
                {
                    return list[0];
                }
            }
        }

        public int Count
        {
            get => list.Count;
            set => list.Count = value;
        }

        public void Enqueue(T item)
        {
            list.Add(item);
        }

        public T Dequeue()
        {
            if (Count == 0) return default;

            T res = list[0];
            for (int i = 1; i < Count; i++)
            {
                list[i - 1] = list[i];
            }
            Count--;
            return res;
        }
    }
}