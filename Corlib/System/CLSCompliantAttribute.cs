using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Implementation of the "CLSCompliantAttribute" class.
    /// </summary>
    public sealed class CLSCompliantAttribute : Attribute
    {
        private readonly bool is_compliant;

        public CLSCompliantAttribute(bool isCompliant)
        {
            is_compliant = isCompliant;
        }

        public bool IsCompliant
        {
            get
            {
                return is_compliant;
            }
        }
    }
}
