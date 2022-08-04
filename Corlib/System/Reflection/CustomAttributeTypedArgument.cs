using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace System.Reflection
{
    /// <summary>
    /// Represents an argument of a custom attribute in the reflection-only context, or an element of an array argument.
    /// </summary>
    [Serializable]
    public struct CustomAttributeTypedArgument
    {
        #region Public Static Members

        public static bool operator ==(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
        {
            return !left.Equals(right);
        }

        #endregion Public Static Members

        #region Private Data Members

        private readonly object m_value;
        private readonly Type m_argumentType;

        #endregion Private Data Members

        #region Constructor

        public CustomAttributeTypedArgument(Type argumentType, object value)
        {
            // value can be null.
            if (argumentType == null)
                throw new ArgumentNullException(nameof(argumentType));

            m_value = (value == null) ? null : CanonicalizeValue(value);
            m_argumentType = argumentType;
        }

        public CustomAttributeTypedArgument(object value)
        {
            // value cannot be null.
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            m_value = CanonicalizeValue(value);
            m_argumentType = value.GetType();
        }

        private static object CanonicalizeValue(object value)
        {
            //Debug.Assert(value != null);

            //if (value.GetType().IsEnum)
            //{
            //	return ((Enum)value).GetValue();
            //}
            return value;
        }

        #endregion Constructor

        #region Object Overrides

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj == (object)this;
        }

        #endregion Object Overrides

        #region Public Members

        public Type ArgumentType
        {
            get
            {
                return m_argumentType;
            }
        }

        public object Value
        {
            get
            {
                return m_value;
            }
        }

        #endregion Public Members
    }
}
