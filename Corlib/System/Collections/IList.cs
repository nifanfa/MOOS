using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    /// <summary>
    ///
    /// </summary>
    public interface IList : ICollection, IEnumerable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is fixed size.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
        /// </value>
        bool IsFixedSize { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        bool IsReadOnly { get; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value></value>
        object this[int index] { get; set; }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        int Add(object value);

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether [contains] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(object value);

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        int IndexOf(object value);

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        void Insert(int index, object value);

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Remove(object value);

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        void RemoveAt(int index);
    }
}
