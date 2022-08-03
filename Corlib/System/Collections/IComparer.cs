using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    /// <summary>
    /// Interface for "System.Collections.IComparer"
    /// </summary>
    public interface IComparer
    {
        int Compare(object x, object y);
    }
}
