// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Runtime.CompilerServices
{
    /// <summary>
	/// Indicates that a method is an extension method, or that a class or assembly contains extension methods.
	/// </summary>
	[AttributeUsageAttribute(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionAttribute"/> class.
        /// </summary>
        public ExtensionAttribute()
        {
        }
    }
}
