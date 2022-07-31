namespace System.Collections.Generic
{
    public class Queue<T>
    {
        private List<T> list;

        public Queue(int initsize = 256)
        {
            list = new List<T>(initsize);
        }

        public T Tail => Count == 0 ? default : list[list.Count - 1];

        public T Head => Count == 0 ? default : list[0];

        public int Count => list.Count;


        public void Enqueue(T item)
        {
            list.Add(item);
        }

        public T Dequeue()
        {
            if (list.Count == 0)
            {
                return default;
            }

            list.Count--;
            return list[list.Count];
        }
    }
}