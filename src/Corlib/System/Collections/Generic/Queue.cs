// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence

namespace System.Collections.Generic
{
    /// <summary>A very basic queue implemented with a linked list.</summary>
    /// <typeparam name="T">The type of the object held by the queue.</typeparam>
    public class Queue<T>
    {
        private class Entry
        {
            public T Value;
            public Entry Next;

            public Entry(T value)
            {
                Value = value;
            }
        }

        private Entry head;
        private Entry tail;


        public int Length { get; private set; }


        public void Enqueue(T item)
        {
            if (head == null)
            {
                head = new Entry(item);
                tail = head;
                Length = 1;

                return;
            }

            Entry entry = new Entry(item);
            tail.Next = entry;
            tail = entry;
            Length++;
        }

        public T Dequeue()
        {
            T value = head.Value;
            Entry next = head.Next;
            head.Dispose();
            head = next;
            Length--;

            return value;
        }
    }
}
