using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
	/// Discovers the attributes of a field and provides access to field metadata.
	/// </summary>
	[SerializableAttribute]
    public abstract class FieldInfo : MemberInfo
    {
        /// <summary>
        /// Gets the attributes associated with this field.
        /// </summary>
        public abstract FieldAttributes Attributes { get; }

        /// <summary>
        /// Gets the type of this field object.
        /// </summary>
        public abstract Type FieldType { get; }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this field is described by FieldAttributes.Assembly; that is, the field is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
        /// </summary>
        public bool IsAssembly
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly; }
        }

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by FieldAttributes.Family; that is, the field is visible only within its class and derived classes.
        /// </summary>
        public bool IsFamily
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family; }
        }

        /// <summary>
        /// Gets a value indicating whether the visibility of this field is described by FieldAttributes.FamANDAssem; that is, the field can be accessed from derived classes, but only if they are in the same assembly.
        /// </summary>
        public bool IsFamilyAndAssembly
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem; }
        }

        /// <summary>
        /// Gets a value indicating whether the potential visibility of this field is described by FieldAttributes.FamORAssem; that is, the field can be accessed by derived classes wherever they are, and by classes in the same assembly.
        /// </summary>
        public bool IsFamilyOrAssembly
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem; }
        }

        /// <summary>
        /// Gets a value indicating whether the field can only be set in the body of the constructor.
        /// </summary>
        public bool IsInitOnly
        {
            get { return (Attributes & FieldAttributes.InitOnly) == FieldAttributes.InitOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether the value is written at compile time and cannot be changed.
        /// </summary>
        public bool IsLiteral
        {
            get { return (Attributes & FieldAttributes.Literal) == FieldAttributes.Literal; }
        }

        /// <summary>
        /// Gets a value indicating whether the field is private.
        /// </summary>
        public bool IsPrivate
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private; }
        }

        /// <summary>
        /// Gets a value indicating whether the field is public.
        /// </summary>
        public bool IsPublic
        {
            get { return (Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public; }
        }

        /// <summary>
        /// Gets a value indicating whether the corresponding SpecialName attribute is set in the FieldAttributes enumerator.
        /// </summary>
        public bool IsSpecialName
        {
            get { return (Attributes & FieldAttributes.SpecialName) == FieldAttributes.SpecialName; }
        }

        /// <summary>
        /// Gets a value indicating whether the field is static.
        /// </summary>
        public bool IsStatic
        {
            get { return (Attributes & FieldAttributes.Static) == FieldAttributes.Static; }
        }

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
        /// Gets a FieldInfo for the field represented by the specified handle.
        /// </summary>
        /// <param name="handle">A RuntimeFieldHandle structure that contains the handle to the internal metadata representation of a field.</param>
        /// <returns>A FieldInfo object representing the field specified by handle.</returns>
        public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle)
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a FieldInfo for the field represented by the specified handle, for the specified generic type.
        /// </summary>
        /// <param name="handle">A RuntimeFieldHandle structure that contains the handle to the internal metadata representation of a field.</param>
        /// <param name="declaringType">A RuntimeTypeHandle structure that contains the handle to the generic type that defines the field.</param>
        /// <returns>A FieldInfo object representing the field specified by handle, in the generic type specified by declaringType.</returns>
        public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType)
        {
            // TODO
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
        /// When overridden in a derived class, returns the value of a field supported by a given object.
        /// </summary>
        /// <param name="obj">The object whose field value will be returned.</param>
        /// <returns>An object containing the value of the field reflected by this instance.</returns>
        public abstract object GetValue(object obj);

        public void SetValue(object obj, object value)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
