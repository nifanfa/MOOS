using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// Defines methods to manipulate generic collections.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements in the collection.
    /// </typeparam>
    public interface ICollection<T> : IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Gets the number of elements contained in the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value indicating whether the System.Collections.Generic.ICollection&lt;T&gt;
        /// is read-only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Adds an item to the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </summary>
        /// <param name="item">
        /// The object to add to the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </param>
        void Add(T item);

        /// <summary>
        /// Removes all items from the System.Collections.Generic.ICollection&lt;T&gt;.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the System.Collections.Generic.ICollection&lt;T&gt; contains
        /// a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.ICollection&lt;T&gt;.</param>
        /// <returns>
        /// true if item is found in the System.Collections.Generic.ICollection&lt;T&gt; otherwise,
        /// false.
        /// </returns>
        bool Contains(T item);

        /// <summary>
        /// Copies the elements of the <see cref="T:ICollection`1"/> to an <see cref="T:Array"/>, starting at a particular <see cref="T:Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:Array"/> that is the destination of the elements copied from <see cref="T:ICollection`1"/>. The <see cref="T:Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        void CopyTo(T[] array, int arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:ICollection`1"/>.</param>
        /// <returns>true if item was successfully removed from the <see cref="T:ICollection`1"/>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:ICollection`1"/>.</returns>
        bool Remove(T item);
    }
}
