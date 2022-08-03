using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public interface IEquatable<T>
    {
        bool Equals(T other);
    }
}
