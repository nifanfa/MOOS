using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a node in a LinkedList<T>. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    public sealed class LinkedListNode<T>
    {
        internal LinkedList<T> list;
        internal LinkedListNode<T> next;
        internal LinkedListNode<T> previous;
        internal T value;

        /// <summary>
        /// Gets the LinkedList<T> that the LinkedListNode<T> belongs to.
        /// </summary>
        public LinkedList<T> List
        {
            get { return list; }
        }

        /// <summary>
        /// Gets the next node in the LinkedList<T>.
        /// </summary>
        public LinkedListNode<T> Next
        {
            get { return next; }
        }

        /// <summary>
        /// Gets the previous node in the LinkedList<T>.
        /// </summary>
        public LinkedListNode<T> Previous
        {
            get { return previous; }
        }

        /// <summary>
        /// Gets the value contained in the node.
        /// </summary>
        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Initializes a new instance of the LinkedListNode<T> class, containing the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public LinkedListNode(T value)
        {
            this.value = value;
        }
    }
}
