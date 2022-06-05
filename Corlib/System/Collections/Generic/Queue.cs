/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */

namespace System.Collections.Generic
{
    public class Queue<T>
    {
        List<T> list;

        public Queue(int initsize = 256)
        {
            list = new List<T>(initsize);
        }

        public int Length => list.Count;


        public void Enqueue(T item)
        {
            list.Add(item);
        }

        public T Dequeue()
        {
            if (list.Count == 0) return default;

            list.Count--;
            return list[list.Count];
        }
    }
}