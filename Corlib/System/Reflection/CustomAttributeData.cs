using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
    /// Provides access to custom attribute data for assemblies, modules, types, members and parameters that are loaded into the reflection-only context.
    /// </summary>
    [Serializable]
    public class CustomAttributeData
    {
        protected Type attributeType;
        protected IList<CustomAttributeTypedArgument> ctorArgs;
        protected IList<CustomAttributeNamedArgument> namedArgs;

        /// <summary>
        /// The type of the attribute.
        /// </summary>
        public Type AttributeType
        {
            get { return attributeType; }
        }

        /// <summary>
        /// A collection of structures that represent the positional arguments specified for the custom attribute instance.
        /// </summary>
        public virtual IList<CustomAttributeTypedArgument> ConstructorArguments
        {
            get { return ctorArgs; }
        }

        /// <summary>
        /// A collection of structures that represent the named arguments specified for the custom attribute instance.
        /// </summary>
        public virtual IList<CustomAttributeNamedArgument> NamedArguments
        {
            get { return namedArgs; }
        }
    }
}
