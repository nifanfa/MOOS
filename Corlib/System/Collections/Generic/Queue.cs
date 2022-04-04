/*
 * Copyright(c) 2022 nifanfa, This code is part of the Solution1 licensed under the MIT licence.
 */

namespace System.Collections.Generic
{
    /// <summary>A very basic queue implemented with a linked list.</summary>
    /// <typeparam name="T">The type of the object held by the queue.</typeparam>
    public class Queue<T>
    {
        class Entry
        {
            public T Value;
            public Entry Next;

            public Entry(T value)
            {
                Value = value;
            }
        }


        Entry head;
        Entry tail;


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

            var entry = new Entry(item);
            tail.Next = entry;
            tail = entry;
            Length++;
        }

        public T Dequeue()
        {
            var value = head.Value;
            var next = head.Next;
            head.Dispose();
            head = next;
            Length--;

            return value;
        }
    }
}