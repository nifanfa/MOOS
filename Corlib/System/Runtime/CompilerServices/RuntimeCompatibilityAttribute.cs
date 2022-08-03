using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
    [SerializableAttribute]
    [AttributeUsageAttribute(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class RuntimeCompatibilityAttribute : Attribute
    {
        public RuntimeCompatibilityAttribute()
        {
        }

        public bool WrapNonExceptionThrows
        {
            get { return false; }
            set { }
        }
    }
}
