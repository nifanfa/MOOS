using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    [Serializable]
    public sealed class IndexerNameAttribute : Attribute
    {
        public IndexerNameAttribute(string indexerName)
        {
        }
    }
}
