using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    /// <summary>
    /// Interface for "System.Collections.IStructuralComparable"
    /// </summary>
    public interface IStructuralComparable
    {
        int CompareTo(object other, IComparer comparer);
    }
}
