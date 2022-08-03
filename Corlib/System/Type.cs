using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    /// <summary>
    /// Represents type declarations: class types, interface types, array types, value types, enumeration types, type parameters, generic type definitions, and open or closed constructed generic types.
    /// </summary>
    [Serializable]
    public abstract class Type
    {
        /// <summary>
        /// Represents a missing value in the Type information. This field is read-only.
        /// </summary>
        public static readonly object Missing = null; //TODO

        /// <summary>
        /// Gets the assembly-qualified name of the System.Type, which includes the name of the assembly from which the System.Type was loaded.
        /// </summary>
        public abstract string AssemblyQualifiedName { get; }

        /// <summary>
        /// Gets the type that declares the current nested type or generic type parameter.
        /// </summary>
        public abstract Type DeclaringType { get; }

        /// <summary>
        /// Gets the fully qualified name of the Type, including the namespace of the Type but not the assembly.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Gets the position of the type parameter in the type parameter list of the generic type or method that declared the parameter, when the System.Type object represents a type parameter of a generic type or a generic method.
        /// </summary>
        public abstract int GenericParameterPosition { get; }

        /// <summary>
        /// Gets an array of the generic type arguments for this type.
        /// </summary>
        public abstract Type[] GenericTypeArguments { get; }

        /// <summary>
        /// Gets a value indicating whether the current Type encompasses or refers to another type; that is, whether the current Type is an array, a pointer, or is passed by reference.
        /// </summary>
        public bool HasElementType
        {
            get { return HasElementTypeImpl(); }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is an array.
        /// </summary>
        public bool IsArray
        {
            get { return IsArrayImpl(); }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is passed by reference.
        /// </summary>
        public bool IsByRef
        {
            get { return IsByRefImpl(); }
        }

        /// <summary>
        /// Gets a value that indicates whether this object represents a constructed generic type. You can create instances of a constructed generic type.
        /// </summary>
        public abstract bool IsConstructedGenericType { get; }

        /// <summary>
        /// Gets a value indicating whether the current System.Type represents a type parameter in the definition of a generic type or method.
        /// </summary>
        public abstract bool IsGenericParameter { get; }

        /// <summary>
        /// Gets a value indicating whether the current Type object represents a type whose definition is nested inside the definition of another type.
        /// </summary>
        public bool IsNested
        {
            get { return IsNestedImpl(); }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is a pointer.
        /// </summary>
        public bool IsPointer
        {
            get { return IsPointerImpl(); }
        }

        /// <summary>
        /// Gets the name of the System.Type.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the namespace of the Type.
        /// </summary>
        public abstract string Namespace { get; }

        /// <summary>
        /// Gets the handle for the current Type.
        /// </summary>
        public virtual RuntimeTypeHandle TypeHandle
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Determines if the underlying system type of the current Type is the same as the underlying system type of the specified Object.
        /// </summary>
        /// <param name="obj">The object whose underlying system type is to be compared with the underlying system type of the current Type.</param>
        /// <returns>True if the underlying system type of o is the same as the underlying system type of the current Type; otherwise, False. This method also returns False if the object specified by the o parameter is not a Type.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Type))
                return false;

            return ((Type)obj).TypeHandle.Equals(TypeHandle);
        }

        public static bool operator ==(Type left, Type right)
        {
            var l = left as object;
            var r = right as object;

            if (l == null && r == null)
                return true;

            if (l == null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Type left, Type right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines if the underlying system type of the current Type is the same as the underlying system type of the specified Type.
        /// </summary>
        /// <param name="obj">The object whose underlying system type is to be compared with the underlying system type of the current Type.</param>
        /// <returns>True if the underlying system type of o is the same as the underlying system type of the current Type; otherwise, False.</returns>
        public virtual bool Equals(Type obj)
        {
            var o = obj as object;

            if (o == null)
                return false;

            //Debug.Assert(obj.TypeHandle != null, "obj.TypeHandle != null");

            return obj.TypeHandle.Equals(TypeHandle);
        }

        public virtual bool IsSerializable
        {
            [Pure]
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the number of dimensions in an Array.
        /// </summary>
        /// <returns>An Int32 containing the number of dimensions in the current Type.</returns>
        public abstract int GetArrayRank();

        /// <summary>
        /// When overridden in a derived class, returns the Type of the object encompassed or referred to by the current array, pointer or reference type.
        /// </summary>
        /// <returns>The Type of the object encompassed or referred to by the current array, pointer, or reference type, or null if the current Type is not an array or a pointer, or is not passed by reference, or represents a generic type or a type parameter in the definition of a generic type or generic method.</returns>
        public abstract Type GetElementType();

        /// <summary>
        /// Returns a Type object that represents a generic type definition from which the current generic type can be constructed.
        /// </summary>
        /// <returns>A Type object representing a generic type from which the current type can be constructed.</returns>
        public abstract Type GetGenericTypeDefinition();

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the Type with the specified name, performing a case-sensitive search.
        /// </summary>
        /// <param name="typeName">The assembly-qualified name of the type to get. See AssemblyQualifiedName. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
        /// <returns>The type with the specified name, if found; otherwise, null.</returns>
        public static Type GetType(string typeName)
        {
            return GetType(typeName, false);
        }

        /// <summary>
        /// Gets the Type with the specified name, performing a case-sensitive search and specifying whether to throw an exception if the type is not found.
        /// </summary>
        /// <param name="typeName">The assembly-qualified name of the type to get. See AssemblyQualifiedName. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace.</param>
        /// <param name="throwOnError">True to throw an exception if the type cannot be found; False to return null. Specifying False also suppresses some other exception conditions, but not all of them.</param>
        /// <returns>The type with the specified name. If the type is not found, the throwOnError parameter specifies whether null is returned or an exception is thrown. In some cases, an exception is thrown regardless of the value of throwOnError.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Type GetType(string typeName, bool throwOnError);

        /// <summary>
        /// Gets the type referenced by the specified type handle.
        /// </summary>
        /// <param name="handle">The object that refers to the type.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Type GetTypeFromHandle(RuntimeTypeHandle handle);

        /// <summary>
        /// When overridden in a derived class, implements the HasElementType property and determines whether the current Type encompasses or refers to another type; that is, whether the current Type is an array, a pointer, or is passed by reference.
        /// </summary>
        /// <returns>True if the Type is an array, a pointer, or is passed by reference; otherwise, False.</returns>
        protected abstract bool HasElementTypeImpl();

        /// <summary>
        /// When overridden in a derived class, implements the IsArray property and determines whether the Type is an array.
        /// </summary>
        /// <returns>True if the Type is an array; otherwise, False.</returns>
        protected abstract bool IsArrayImpl();

        /// <summary>
        /// When overridden in a derived class, implements the IsByRef property and determines whether the Type is passed by reference.
        /// </summary>
        /// <returns>True if the Type is passed by reference; otherwise, False.</returns>
        protected abstract bool IsByRefImpl();

        /// <summary>
        /// When overridden in a derived class, implements the IsNested property and determines whether the Type is a nested tyoe.
        /// </summary>
        /// <returns>True if the Type is a nested type; otherwise, False.</returns>
        protected abstract bool IsNestedImpl();

        /// <summary>
        /// When overridden in a derived class, implements the IsPointer property and determines whether the Type is a pointer.
        /// </summary>
        /// <returns>True if the Type is a pointer; otherwise, False.</returns>
        protected abstract bool IsPointerImpl();

        /// <summary>
        /// Returns a Type object representing a one-dimensional array of the current type, with a lower bound of zero.
        /// </summary>
        /// <returns>A Type object representing a one-dimensional array of the current type, with a lower bound of zero.</returns>
        public abstract Type MakeArrayType();

        /// <summary>
        /// Returns a Type object representing an array of the current type, with the specified number of dimensions.
        /// </summary>
        /// <param name="rank">The number of dimensions for the array. This number must be less than or equal to 32.</param>
        /// <returns>An object representing an array of the current type, with the specified number of dimensions.</returns>
        public abstract Type MakeArrayType(int rank);

        /// <summary>
        /// Returns a Type object that represents the current type when passed as a ref parameter.
        /// </summary>
        /// <returns>A Type object that represents the current type when passed as a ref parameter.</returns>
        public abstract Type MakeByRefType();

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic type definition and returns a Type object representing the resulting constructed type.
        /// </summary>
        /// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic type.</param>
        /// <returns>A Type representing the constructed type formed by substituting the elements of typeArguments for the type parameters of the current generic type.</returns>
        public abstract Type MakeGenericType(params Type[] typeArguments);

        /// <summary>
        /// Returns a Type object that represents a pointer to the current type.
        /// </summary>
        /// <returns>A Type object that represents a pointer to the current type.</returns>
        public abstract Type MakePointerType();

        /// <summary>
        /// Returns a String representing the name of the current Type.
        /// </summary>
        /// <returns>A String representing the name of the current Type.</returns>
        public override string ToString()
        {
            return FullName;
        }
    }
}
