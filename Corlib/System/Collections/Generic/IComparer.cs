using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IComparer<T>
    {
        /// <summary>
        /// Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        int Compare(T x, T y);
    }
}
