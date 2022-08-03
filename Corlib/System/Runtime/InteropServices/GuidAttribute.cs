using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Delegate, Inherited = false)]
    public sealed class GuidAttribute : Attribute
    {
        public GuidAttribute(string guid)
        {
            Value = guid;
        }

        public string Value { get; }
    }
}
