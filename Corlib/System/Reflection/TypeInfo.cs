using System;
using System.Collections.Generic;
using System.Text;

namespace System.Reflection
{
    /// <summary>
    /// Represents type declarations for class types, interface types, array types, value types, enumeration types, type parameters, generic type definitions, and open or closed constructed generic types.
    /// </summary>
    [Serializable]
    public abstract class TypeInfo : MemberInfo, IReflectableType
    {
        /// <summary>
        /// Gets the assembly-qualified name of the System.Type, which includes the name of the assembly from which the System.Type was loaded.
        /// </summary>
        public abstract string AssemblyQualifiedName { get; }

        /// <summary>
        /// Gets the Assembly in which the type is declared. For generic types, gets the Assembly in which the generic type is defined.
        /// </summary>
        public abstract Assembly Assembly { get; }

        /// <summary>
        /// Gets the attributes associated with the Type.
        /// </summary>
        public abstract TypeAttributes Attributes { get; }

        /// <summary>
        /// Gets the type from which the current Type directly inherits.
        /// </summary>
        public abstract Type BaseType { get; }

        /// <summary>
        /// Gets a value indicating whether the current Type object has type parameters that have not been replaced by specific types.
        /// </summary>
        public abstract bool ContainsGenericParameters { get; }

        /// <summary>
        /// Gets a collection of the constructors declared by the current type.
        /// </summary>
        public virtual IEnumerable<ConstructorInfo> DeclaredConstructors
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a collection of the events defined by the current type.
        /// </summary>
        //public virtual IEnumerable<EventInfo> DeclaredEvents
        //{
        //	get { throw new NotImplementedException(); }
        //}

        /// <summary>
        /// Gets a collection of the fields defined by the current type.
        /// </summary>
        public virtual IEnumerable<FieldInfo> DeclaredFields
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a collection of the members defined by the current type.
        /// </summary>
        public virtual IEnumerable<MemberInfo> DeclaredMembers
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a collection of the methods defined by the current type.
        /// </summary>
        public virtual IEnumerable<MethodInfo> DeclaredMethods
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a collection of the nested types defined by the current type.
        /// </summary>
        public virtual IEnumerable<TypeInfo> DeclaredNestedTypes
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a collection of the properties defined by the current type.
        /// </summary>
        public virtual IEnumerable<PropertyInfo> DeclaredProperties
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a MethodBase that represents the declaring method, if the current Type represents a type parameter of a generic method.
        /// </summary>
        public abstract MethodBase DeclaringMethod { get; }

        /// <summary>
        /// Gets the fully qualified name of the Type, including the namespace of the Type but not the assembly.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Gets a combination of GenericParameterAttributes flags that describe the covariance and special constraints of the current generic type parameter.
        /// </summary>
        //public abstract GenericParameterAttributes GenericParameterAttributes { get; }

        /// <summary>
        /// Gets the position of the type parameter in the type parameter list of the generic type or method that declared the parameter, when the System.Type object represents a type parameter of a generic type or a generic method.
        /// </summary>
        public abstract int GenericParameterPosition { get; }

        /// <summary>
        /// Gets an array of the generic type arguments for this type.
        /// </summary>
        public abstract Type[] GenericTypeArguments { get; }

        /// <summary>
        /// Gets an array of the generic parameters of the current type.
        /// </summary>
        public virtual Type[] GenericTypeParameters
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a value indicating whether the current Type encompasses or refers to another type; that is, whether the current Type is an array, a pointer, or is passed by reference.
        /// </summary>
        public bool HasElementType
        {
            get { return HasElementTypeImpl(); }
        }

        /// <summary>
        /// Gets a collection of the interfaces implemented by the current type.
        /// </summary>
        public virtual IEnumerable<Type> ImplementedInterfaces
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is abstract and must be overridden.
        /// </summary>
        public bool IsAbstract
        {
            get { return (Attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract; }
        }

        /// <summary>
        /// Gets a value indicating whether the string format attribute AnsiClass is selected for the Type.
        /// </summary>
        public bool IsAnsiClass
        {
            get { return (Attributes & TypeAttributes.StringFormatMask) == TypeAttributes.AnsiClass; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is an array.
        /// </summary>
        public bool IsArray
        {
            get { return IsArrayImpl(); }
        }

        /// <summary>
        /// Gets a value indicating whether the string format attribute AutoClass is selected for the Type.
        /// </summary>
        public bool IsAutoClass
        {
            get { return (Attributes & TypeAttributes.StringFormatMask) == TypeAttributes.AutoClass; }
        }

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out automatically by the common language runtime.
        /// </summary>
        public bool IsAutoLayout
        {
            get { return (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is passed by reference.
        /// </summary>
        public bool IsByRef
        {
            get { return IsByRefImpl(); }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is a class; that is, not a value type or interface.
        /// </summary>
        public bool IsClass
        {
            get { return !(IsInterface || IsValueType); }
        }

        /// <summary>
        /// Gets a value indicating whether the current Type represents an enumeration.
        /// </summary>
        public abstract bool IsEnum { get; }

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out at explicitly specified offsets.
        /// </summary>
        public bool IsExplicitLayout
        {
            get { return (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout; }
        }

        /// <summary>
        /// Gets a value indicating whether the current System.Type represents a type parameter in the definition of a generic type or method.
        /// </summary>
        public abstract bool IsGenericParameter { get; }

        /// <summary>
        /// Gets a value indicating whether the current type is a generic type.
        /// </summary>
        public abstract bool IsGenericType { get; }

        /// <summary>
        /// Gets a value indicating whether the current Type represents a generic type definition, from which other generic types can be constructed.
        /// </summary>
        public abstract bool IsGenericTypeDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether the Type has a ComImportAttribute attribute applied, indicating that it was imported from a COM type library.
        /// </summary>
        public bool IsImport
        {
            get { return (Attributes & TypeAttributes.Import) == TypeAttributes.Import; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is an interface; that is, not a class or a value type.
        /// </summary>
        public bool IsInterface
        {
            get { return (Attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Interface; }
        }

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out sequentially, in the order that they were defined or emitted to the metadata.
        /// </summary>
        public bool IsLayoutSequential
        {
            get { return (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout; }
        }

        /// <summary>
        /// Gets a value indicating whether the current Type object represents a type whose definition is nested inside the definition of another type.
        /// </summary>
        public bool IsNested
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) > TypeAttributes.Public; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is nested and visible only within its own assembly.
        /// </summary>
        public bool IsNestedAssembly
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedAssembly; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is nested and visible only to classes that belong to both its own family and its own assembly.
        /// </summary>
        public bool IsNestedFamANDAssem
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamANDAssem; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is nested and visible only within its own family.
        /// </summary>
        public bool IsNestedFamily
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is nested and visible only to classes that belong to either its own family or to its own assembly.
        /// </summary>
        public bool IsNestedFamORAssem
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamORAssem; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is nested and declared private.
        /// </summary>
        public bool IsNestedPrivate
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate; }
        }

        /// <summary>
        /// Gets a value indicating whether a class is nested and declared public.
        /// </summary>
        public bool IsNestedPublic
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is not declared public.
        /// </summary>
        public bool IsNotPublic
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is a pointer.
        /// </summary>
        public bool IsPointer
        {
            get { return IsPointerImpl(); }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is one of the primitive types.
        /// </summary>
        public bool IsPrimitive
        {
            get { return IsPrimitiveImpl(); }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is declared public.
        /// </summary>
        public bool IsPublic
        {
            get { return (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is declared sealed.
        /// </summary>
        public bool IsSealed
        {
            get { return (Attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is serializable.
        /// </summary>
        public abstract bool IsSerializable { get; }

        /// <summary>
        /// Gets a value indicating whether the Type has a name that requires special handling.
        /// </summary>
        public bool IsSpecialName
        {
            get { return (Attributes & TypeAttributes.SpecialName) == TypeAttributes.SpecialName; }
        }

        /// <summary>
        /// Gets a value indicating whether the string format attribute UnicodeClass is selected for the Type.
        /// </summary>
        public bool IsUnicodeClass
        {
            get { return (Attributes & TypeAttributes.StringFormatMask) == TypeAttributes.UnicodeClass; }
        }

        /// <summary>
        /// Gets a value indicating whether the Type is a value type.
        /// </summary>
        public bool IsValueType
        {
            get { return IsValueTypeImpl(); }
        }

        /// <summary>
        /// Gets a value indicating whether the Type can be accessed by code outside the assembly.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (IsGenericParameter)
                    return true;

                if (HasElementType && GetElementType().GetTypeInfo() != null)
                    return GetElementType().GetTypeInfo().IsVisible;

                TypeInfo type = this;
                while (type.IsNested)
                {
                    if (!type.IsNestedPublic)
                        return false;

                    if (type.DeclaringType.GetTypeInfo() == null)
                        return false;

                    type = type.DeclaringType.GetTypeInfo();
                }

                if (!type.IsPublic)
                    return false;

                // TODO generics

                return true;
            }
        }

        /// <summary>
        /// Gets the namespace of the Type.
        /// </summary>
        public abstract string Namespace { get; }

        /// <summary>
        /// Returns the current type as a Type object.
        /// </summary>
        /// <returns></returns>
        public virtual Type AsType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of dimensions in an Array.
        /// </summary>
        /// <returns>An Int32 containing the number of dimensions in the current Type.</returns>
        public abstract int GetArrayRank();

        /// <summary>
        /// Returns an object that represents the specified public event declared by the current type.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        /// <returns>An object that represents the specified event, if found; otherwise, null.</returns>
        //public virtual EventInfo GetDeclaredEvent(string name) { }

        /// <summary>
        /// Returns an object that represents the specified public field declared by the current type.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <returns>An object that represents the specified field, if found; otherwise, null.</returns>
        public virtual FieldInfo GetDeclaredField(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            foreach (var info in DeclaredFields)
            {
                if (info.Name != name)
                    continue;
                return info;
            }

            return null;
        }

        /// <summary>
        /// Returns an object that represents the specified public method declared by the current type.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <returns>An object that represents the specified method, if found; otherwise, null.</returns>
        public virtual MethodInfo GetDeclaredMethod(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            foreach (var info in DeclaredMethods)
            {
                if (info.Name != name)
                    continue;
                return info;
            }

            return null;
        }

        /// <summary>
        /// Returns a collection that contains all public methods declared on the current type that match the specified name.
        /// </summary>
        /// <param name="name">The method name to search for.</param>
        /// <returns>A collection that contains methods that match name.</returns>
        public virtual IEnumerable<MethodInfo> GetDeclaredMethods(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an object that represents the specified public nested type declared by the current type.
        /// </summary>
        /// <param name="name">The name of the nested type.</param>
        /// <returns>An object that represents the specified nested type, if found; otherwise, null.</returns>
        public virtual TypeInfo GetDeclaredNestedType(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            foreach (var info in DeclaredNestedTypes)
            {
                if (info.Name != name)
                    continue;
                return info;
            }

            return null;
        }

        /// <summary>
        /// Returns an object that represents the specified public property declared by the current type.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>An object that represents the specified property, if found; otherwise, null.</returns>
        public virtual PropertyInfo GetDeclaredProperty(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            foreach (var info in DeclaredProperties)
            {
                if (info.Name != name)
                    continue;
                return info;
            }

            return null;
        }

        /// <summary>
        /// When overridden in a derived class, returns the Type of the object encompassed or referred to by the current array, pointer or reference type.
        /// </summary>
        /// <returns>The Type of the object encompassed or referred to by the current array, pointer, or reference type, or null if the current Type is not an array or a pointer, or is not passed by reference, or represents a generic type or a type parameter in the definition of a generic type or generic method.</returns>
        public abstract Type GetElementType();

        /// <summary>
        /// Returns an array of Type objects that represent the constraints on the current generic type parameter.
        /// </summary>
        /// <returns>An array of Type objects that represent the constraints on the current generic type parameter.</returns>
        public abstract Type[] GetGenericParameterConstraints();

        /// <summary>
        /// Returns a Type object that represents a generic type definition from which the current generic type can be constructed.
        /// </summary>
        /// <returns>A Type object representing a generic type from which the current type can be constructed.</returns>
        public abstract Type GetGenericTypeDefinition();

        /// <summary>
        /// Returns a value that indicates whether the specified type can be assigned to the current type.
        /// </summary>
        /// <param name="typeInfo">The type to check.</param>
        /// <returns>True if the specified type can be assigned to this type; otherwise, False.</returns>
        public virtual bool IsAssignableFrom(TypeInfo typeInfo)
        {
            if (typeInfo == null)
                return false;

            if (typeInfo == this)
                return true;

            if (typeInfo.IsSubclassOf(AsType()))
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the class represented by the current Type derives from the class represented by the specified Type.
        /// </summary>
        /// <param name="c">The type to compare with the current type.</param>
        /// <returns>True if the Type represented by the c parameter and the current Type represent classes, and the class represented by the current Type derives from the class represented by c; otherwise, False. This method also returns False if c and the current Type represent the same class.</returns>
        public virtual bool IsSubclassOf(Type c)
        {
            if (c == null || c == AsType())
                return false;

            Type type = BaseType;
            while (type != null)
            {
                if (type == c)
                    return true;
                if (type.GetTypeInfo() == null)
                    break;
                type = type.GetTypeInfo().BaseType;
            }

            return false;
        }

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
        /// When overridden in a derived class, implements the IsPrimitive property and determines whether the Type is a primitive.
        /// </summary>
        /// <returns>True if the Type is a primitive; otherwise, False.</returns>
        protected abstract bool IsPrimitiveImpl();

        /// <summary>
        /// When overridden in a derived class, implements the IsValueType property and determines whether the Type is a value type.
        /// </summary>
        /// <returns>True if the Type is a value type; otherwise, False.</returns>
        protected abstract bool IsValueTypeImpl();

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

        TypeInfo IReflectableType.GetTypeInfo()
        {
            return this;
        }
    }
}
