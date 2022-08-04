using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    [Serializable]
    public class ParameterInfo
    {
        /// <summary>
        /// The attributes of the parameter.
        /// </summary>
        protected ParameterAttributes AttrsImpl;

        /// <summary>
        /// The <see cref="System.Type">Type</see> of the parameter.
        /// </summary>
        protected Type ClassImpl;

        /// <summary>
        /// The default value of the parameter.
        /// </summary>
        protected Object DefaultValueImpl;

        /// <summary>
        /// The member in which the field is implemented.
        /// </summary>
        protected MemberInfo MemberImpl;

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        protected string NameImpl;

        /// <summary>
        /// The zero-based position of the parameter in the parameter list.
        /// </summary>
        protected int PositionImpl;

        /// <summary>
        /// Gets the attributes for this parameter.
        /// </summary>
        public virtual ParameterAttributes Attributes
        {
            get { return AttrsImpl; }
        }

        /// <summary>
        /// Gets a collection that contains this parameter's custom attributes.
        /// </summary>
        public virtual IEnumerable<CustomAttributeData> CustomAttributes
        {
            get { return new CustomAttributeData[0]; }
        }

        /// <summary>
        /// Gets a value indicating the default value if the parameter has a default value.
        /// </summary>
        public virtual object DefaultValue
        {
            get { return DefaultValueImpl; }
        }

        /// <summary>
        /// Gets a value that indicates whether this parameter has a default value.
        /// </summary>
        public virtual bool HasDefaultValue
        {
            get { return (AttrsImpl & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault; }
        }

        /// <summary>
        /// Gets a value indicating whether this is an input parameter.
        /// </summary>
        public bool IsIn
        {
            get { return (AttrsImpl & ParameterAttributes.In) == ParameterAttributes.In; }
        }

        /// <summary>
        /// Gets a value indicating whether this parameter is optional.
        /// </summary>
        public bool IsOptional
        {
            get { return (AttrsImpl & ParameterAttributes.Optional) == ParameterAttributes.Optional; }
        }

        /// <summary>
        /// Gets a value indicating whether this is an output parameter.
        /// </summary>
        public bool IsOut
        {
            get { return (AttrsImpl & ParameterAttributes.Out) == ParameterAttributes.Out; }
        }

        /// <summary>
        /// Gets a value indicating the member in which the parameter is implemented.
        /// </summary>
        public virtual MemberInfo Member
        {
            get { return MemberImpl; }
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public virtual string Name
        {
            get { return NameImpl; }
        }

        /// <summary>
        /// Gets the <see cref="System.Type">Type</see> of this parameter.
        /// </summary>
        public virtual Type ParameterType
        {
            get { return ClassImpl; }
        }

        /// <summary>
        /// Gets the zero-based position of the parameter in the formal parameter list.
        /// </summary>
        public virtual int Position
        {
            get { return PositionImpl; }
        }
    }
}
