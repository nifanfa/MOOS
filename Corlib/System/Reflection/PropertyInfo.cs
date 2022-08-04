using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
    /// Obtains information about the attributes of a member and provides access to member metadata.
    /// </summary>
    [SerializableAttribute]
    public abstract class PropertyInfo : MemberInfo
    {
        /// <summary>
        /// Gets the attributes for this property.
        /// </summary>
        public abstract PropertyAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        public abstract bool CanRead { get; }

        /// <summary>
        /// Gets a value indicating whether the property can be written to.
        /// </summary>
        public abstract bool CanWrite { get; }

        /// <summary>
        /// Gets the get accessor for this property.
        /// </summary>
        public virtual MethodInfo GetMethod
        {
            get { return null; }
        }

        /// <summary>
        /// Gets a value indicating whether the property is the special name.
        /// </summary>
        public bool IsSpecialName
        {
            get { return (Attributes & PropertyAttributes.SpecialName) == PropertyAttributes.SpecialName; }
        }

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        public abstract Type PropertyType { get; }

        /// <summary>
        /// Gets the set accessor for this property.
        /// </summary>
        public virtual MethodInfo SetMethod
        {
            get { return null; }
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance, or null.</param>
        /// <returns>True if obj equals the type and value of this instance; otherwise, False.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <returns>An Object that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is null.</returns>
        public virtual object GetConstantValue()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// When overridden in a derived class, returns an array of all the index parameters for the property.
        /// </summary>
        /// <returns>An array of type ParameterInfo containing the parameters for the indexes. If the property is not indexed, the array has 0 (zero) elements.</returns>
        public abstract ParameterInfo[] GetIndexParameters();

        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <returns>The property value of the specified object.</returns>
        public object GetValue(object obj)
        {
            return GetValue(obj, null);
        }

        /// <summary>
        /// Returns the property value of a specified object with optional index values for indexed properties.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <returns>The property value of the specified object.</returns>
        public virtual object GetValue(object obj, object[] index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the property value of a specified object.
        /// </summary>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        public void SetValue(object obj, object value)
        {
            SetValue(obj, value, null);
        }

        /// <summary>
        /// Sets the property value of a specified object with optional index values for index properties.
        /// </summary>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        public virtual void SetValue(object obj, object value, object[] index)
        {
            throw new NotImplementedException();
        }
    }
}
