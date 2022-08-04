using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
    /// Parameter Attributes.
    /// </summary>
    [Flags]
    public enum ParameterAttributes : ushort
    {
        /// <summary>
        /// Parameter is [in]
        /// </summary>
        In = 0x0001,

        /// <summary>
        /// Parameter is [out]
        /// </summary>
        Out = 0x0002,

        /// <summary>
        /// Parameter is optional
        /// </summary>
        Optional = 0x0010,

        /// <summary>
        /// Parameter has default value.
        /// </summary>
        HasDefault = 0x1000,

        /// <summary>
        /// Parameter has FieldMarshal.
        /// </summary>
        HasFieldMarshal = 0x2000,
    }
}
