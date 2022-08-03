using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
	/// Represents a doubly linked list.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class LinkedList<T> : IEnumerable<T>, ICollection<T>
    {
        protected int count;
        protected LinkedListNode<T> first;
        protected LinkedListNode<T> last;

        /// <summary>
        /// Gets the number of nodes actually contained in the LinkedList<T>.
        /// </summary>
        public int Count
        {
            get
            {
                var result = 0;
                var node = First;
                while (node != null)
                {
                    node = node.Next;
                    result++;
                }
                return result;
            }
        }

        public bool IsEmpty { get { return first == null; } }

        /// <summary>
        /// Gets the first node of the LinkedList<T>.
        /// </summary>
        public LinkedListNode<T> First
        {
            get { return first; }
        }

        /// <summary>
        /// Gets the last node of the LinkedList<T>.
        /// </summary>
        public LinkedListNode<T> Last
        {
            get { return last; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LinkedList"/> class.
        /// </summary>
        public LinkedList()
        {
            first = last = null;
            count = 0;
        }

        /// <summary>
        /// Initializes a new instance of the LinkedList<T> class that contains elements copied from the specified IEnumerable and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The IEnumerable whose elements are copied to the new LinkedList<T>.</param>
        public LinkedList(IEnumerable<T> collection)
            : this()
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (T value in collection)
                AddLast(value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:ICollection`1"/> is read-only.
        /// </exception>
        public void Clear()
        {
            first = last = null;
        }

        /// <summary>
        /// Finds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> cur = first;

            while (cur != null)
            {
                if (cur.value.Equals(value))
                    return cur;
                cur = cur.next;
            }

            return null;
        }

        /// <summary>
        /// Determines whether [contains] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T value)
        {
            return (Find(value) != null);
        }

        /// <summary>
        /// Finds the last.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public LinkedListNode<T> FindLast(T value)
        {
            LinkedListNode<T> found = null;
            LinkedListNode<T> cur = first;

            while (cur != null)
            {
                if (cur.value.Equals(value))
                    found = cur;
                cur = cur.next;
            }
            return found;
        }

        /// <summary>
        /// Adds the last.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(value);
            node.list = this;
            node.previous = last;
            node.next = null;
            return AddLast(node);
        }

        /// <summary>
        /// Adds the last.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public LinkedListNode<T> AddLast(LinkedListNode<T> node)
        {
            if (first == null)
            {
                first = node;
                last = node;
            }
            else
            {
                if (node.previous == null)
                    node.previous = last;
                node.previous.next = node;
                last = node;
            }
            ++count;
            return node;
        }

        /// <summary>
        /// Adds the first.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(value);
            node.list = this;
            node.previous = null;
            node.next = first;
            return AddFirst(node);
        }

        /// <summary>
        /// Adds the first.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public LinkedListNode<T> AddFirst(LinkedListNode<T> node)
        {
            if (first != null)
                first.previous = node;

            first = node;

            count++;
            return node;
        }

        /// <summary>
        /// Adds the after.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            if (node == null)
                return null;

            LinkedListNode<T> cur = new LinkedListNode<T>(value);
            cur.list = this;
            cur.previous = node;
            cur.next = node.next;

            if (node.next != null)
                node.next.previous = cur;
            node.next = cur;

            if (cur.next == null)
                last = cur;

            count++;
            return cur;
        }

        /// <summary>
        /// Adds the before.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            if (node == null)
                return null;

            LinkedListNode<T> cur = new LinkedListNode<T>(value);
            cur.list = this;
            cur.previous = node.previous;
            cur.next = node;

            if (node.previous != null)
                node.previous.next = cur;
            node.previous = cur;

            if (cur.previous == null)
                first = cur;

            count++;
            return cur;
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Remove(T value)
        {
            LinkedListNode<T> node = Find(value);

            if (node == null)
                return false;

            Remove(node);
            return true;
        }

        /// <summary>
        /// Removes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Remove(LinkedListNode<T> node)
        {
            if (node == null)
                return;

            if (node.previous != null)
                node.previous.next = node.next;

            if (node.next != null)
                node.next.previous = node.previous;

            if (node == first)
                first = node.next;

            if (node == last)
                last = node.previous;

            count--;
        }

        /// <summary>
        /// Removes the first.
        /// </summary>
        public void RemoveFirst()
        {
            if (first == null)
                return;

            first = first.next;
            first.previous = null;

            if (first.next == null)
                last = first;

            count--;
        }

        /// <summary>
        /// Removes the last.
        /// </summary>
        public void RemoveLast()
        {
            if (last == null)
                return;

            if (last.previous != null)
                last.previous.next = null;

            count--;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            //if (array.Rank != 1)
            //    throw new ArgumentException();
            //if (array.Length - arrayIndex + array.GetLowerBound(0) < count)
            //    throw new ArgumentException();

            LinkedListNode<T> cur = First;

            while (cur != null)
            {
                array[arrayIndex++] = cur.value;
                cur = cur.next;
            }
        }

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            T[] array = new T[count];

            LinkedListNode<T> cur = First;
            uint index = 0;

            while (cur != null)
            {
                array[index++] = cur.value;
                cur = cur.next;
            }

            return array;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="T:LinkedList`1"/>.
        /// </summary>
        /// <returns>An <see cref="T:LinkedList`1.Enumerator"/> for the <see cref="T:LinkedList`1"/>.</returns>
        public LinkedList<T>.Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Adds an item at the end of the <see cref="T:ICollection`1"/>.
        /// </summary>
        /// <param name="value">The value to add at the end of the <see cref="T:ICollection`1"/>.</param>
        void ICollection<T>.Add(T value)
        {
            AddLast(value);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:ICollection`1"/> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<T>.IsReadOnly { get { return false; } }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            private LinkedList<T> list;
            private LinkedListNode<T> current;
            private int index;

            internal Enumerator(LinkedList<T> parent)
            {
                list = parent;
                current = null;
                index = -1;
            }

            public T Current
            {
                get
                {
                    if (list == null)
                        throw new ObjectDisposedException(null);

                    return current.value;
                }
            }

            public bool MoveNext()
            {
                if (list == null)
                    throw new ObjectDisposedException(null);

                if (current == null)
                {
                    current = list.first;
                }
                else
                {
                    current = current.next;
                    if (current == list.first)
                        current = null;
                }

                if (current == null)
                {
                    index = -1;
                    return false;
                }

                ++index;
                return true;
            }

            public void Dispose()
            {
                if (list == null)
                    throw new ObjectDisposedException(null);

                current = null;
                list = null;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            void IEnumerator.Reset()
            {
                if (list == null)
                    throw new ObjectDisposedException(null);

                current = null;
                index = -1;
            }
        }
    }
}
