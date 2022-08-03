using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
    /// Property Attributes.
    /// </summary>
    [Flags]
    public enum PropertyAttributes : ushort
    {
        /// <summary>
        /// Property is special. Name describes how.
        /// </summary>
        SpecialName = 0x0200,

        /// <summary>
        /// Runtime(metadata internal APIs) should check name encoding.
        /// </summary>
        RTSpecialName = 0x0400,

        /// <summary>
        /// Property has default
        /// </summary>
        HasDefault = 0x1000,
    }
}
