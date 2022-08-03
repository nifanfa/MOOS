using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Interface for "System.IComparable"
    /// </summary>
    public interface IComparable
    {
        int CompareTo(object obj);
    }

    public interface IComparable<T>
    {
        int CompareTo(T other);
    }
}
