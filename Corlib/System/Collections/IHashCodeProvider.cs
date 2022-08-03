using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections
{
    [Obsolete("Please use IEqualityComparer instead.")]
    public interface IHashCodeProvider
    {
        // Interfaces are not serializable
        // Returns a hash code for the given object.  
        // 
        int GetHashCode(Object obj);
    }
}
