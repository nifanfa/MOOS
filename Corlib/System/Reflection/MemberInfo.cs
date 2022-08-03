using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
    /// Obtains information about the attributes of a member and provides access to member metadata.
    /// </summary>
    [SerializableAttribute]
    public abstract class MemberInfo
    {
        /// <summary>
        /// A collection that contains this member's custom attributes.
        /// </summary>
        public virtual IEnumerable<CustomAttributeData> CustomAttributes
        {
            get { return new CustomAttributeData[0]; }
        }

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        public abstract Type DeclaringType { get; }

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance, or null.</param>
        /// <returns>True if obj equals the type and value of this instance; otherwise, False.</returns>
        public override bool Equals(object obj)
        {
            return this == obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
