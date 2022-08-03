using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    /// <summary>
	///
	/// </summary>
	public interface IEqualityComparer
    {
        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="x">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        bool Equals(object x, object y);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        int GetHashCode(object obj);
    }
}
