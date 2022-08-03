using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class NonVersionableAttribute : Attribute
    {
        public NonVersionableAttribute()
        {
        }
    }
}
