using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
	/// Represents a named argument of a custom attribute in the reflection-only context.
	/// </summary>
	[Serializable]
    public struct CustomAttributeNamedArgument
    {
        private readonly string memberName;
        private readonly CustomAttributeTypedArgument typedArgument;
        private readonly bool isField;

        public CustomAttributeNamedArgument(string memberName, CustomAttributeTypedArgument typedArgument, bool isField)
        {
            this.memberName = memberName;
            this.typedArgument = typedArgument;
            this.isField = isField;
        }

        /// <summary>
        /// Gets a value that indicates whether the named argument is a field.
        /// </summary>
        public bool IsField
        {
            get { return isField; }
        }

        /// <summary>
        /// Gets the name of the attribute member that would be used to set the named argument.
        /// </summary>
        public string MemberName
        {
            get { return memberName; }
        }

        /// <summary>
        /// Gets a CustomAttributeTypedArgument structure that can be used to obtain the type and value of the current named argument.
        /// </summary>
        public CustomAttributeTypedArgument TypedValue
        {
            get { return typedArgument; }
        }
    }
}
