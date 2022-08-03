using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
    /// <summary>
	/// Reserved to be used by the compiler for tracking metadata.
	/// This attribute should not be used by developers in source code.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, Inherited = false)]
    public sealed class IsReadOnlyAttribute : Attribute
    {
        public IsReadOnlyAttribute()
        {
        }
    }
}
