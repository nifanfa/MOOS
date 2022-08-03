using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    /// <summary>
	///
	/// </summary>
	public interface IEnumerable
    {
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator GetEnumerator();
    }
}
