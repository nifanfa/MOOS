using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    /// <summary>
	/// Interface for "System.Collections.IStructuralEquatable"
	/// </summary>
	internal interface IStructuralEquatable
    {
        bool Equals(Object other, IEqualityComparer comparer);

        int GetHashCode(IEqualityComparer comparer);
    }
}
