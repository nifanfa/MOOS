using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    ///
    /// </summary>
    public interface IEqualityComparer<in T>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare.</param>
        /// <param name="y">The second object of type T to compare.</param>
        /// <returns>True if the specified objects are equal; otherwise, false.</returns>
        bool Equals(T x, T y);

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        int GetHashCode(T obj);
    }
}
