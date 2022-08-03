using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    /// <summary>
	///
	/// </summary>
	public interface IEnumerator
    {
        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        object Current { get; }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false
        /// if the enumerator has passed the end of the collection.
        /// </returns>
        bool MoveNext();

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element
        /// in the collection.
        /// </summary>
        void Reset();
    }
}
