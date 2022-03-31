using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    public abstract partial class Type
    {
        protected Type() { }

        public new Type GetType()
        {
            return this;
        }

        public abstract string? Namespace { get; }
        public abstract string? AssemblyQualifiedName { get; }
        public abstract string? FullName { get; }
        [DllImport("*")]
        public static extern Type GetTypeFromHandle(RuntimeTypeHandle handle);

        public bool IsNested => DeclaringType != null;
        public Type? DeclaringType => null;
        public abstract Type UnderlyingSystemType { get; }
        public bool IsArray => IsArrayImpl();
        protected abstract bool IsArrayImpl();
        public bool IsByRef => IsByRefImpl();
        protected abstract bool IsByRefImpl();
        public bool IsPointer => IsPointerImpl();
        protected abstract bool IsPointerImpl();
        public virtual bool IsGenericParameter => false;
        public virtual bool IsGenericType => false;
        public virtual bool IsGenericTypeDefinition => false;
        public bool HasElementType => HasElementTypeImpl();
        protected abstract bool HasElementTypeImpl();
        public abstract Type? GetElementType();
        public bool IsCOMObject => IsCOMObjectImpl();
        protected abstract bool IsCOMObjectImpl();
        public bool IsContextful => IsContextfulImpl();
        protected virtual bool IsContextfulImpl()
        {
            return false;
        }

        public virtual bool IsEnum => this == typeof(Enum);
        public bool IsMarshalByRef => IsMarshalByRefImpl();
        protected virtual bool IsMarshalByRefImpl()
        {
            return false;
        }

        public bool IsPrimitive => IsPrimitiveImpl();
        protected abstract bool IsPrimitiveImpl();
        public bool IsValueType { [Intrinsic] get => IsValueTypeImpl(); }
        protected virtual bool IsValueTypeImpl()
        {
            return this == typeof(ValueType);
        }

        public virtual bool IsSignatureType => false;
        public abstract Type? BaseType { get; }

        public Type? GetInterface(string name)
        {
            return GetInterface(name, ignoreCase: false);
        }

        public abstract Type? GetInterface(string name, bool ignoreCase);
        public abstract Type[] GetInterfaces();


        public virtual bool IsEquivalentTo(Type? other)
        {
            return this == other;
        }

        // This is used by the ToString() overrides of all reflection types. The legacy behavior has the following problems:
        //  1. Use only Name for nested types, which can be confused with global types and generic parameters of the same name.
        //  2. Use only Name for generic parameters, which can be confused with nested types and global types of the same name.
        //  3. Use only Name for all primitive types, void and TypedReference
        //  4. MethodBase.ToString() use "ByRef" for byref parameters which is different than Type.ToString().
        //  5. ConstructorInfo.ToString() outputs "Void" as the return type. Why Void?
        internal string FormatTypeName()
        {
            Type elementType = this;

            if (elementType.IsPrimitive ||
                elementType.IsNested ||
                elementType == typeof(void))
            {
                return FullName;
            }

            return ToString();
        }

        public override string ToString()
        {
            return FullName;
        }

        public override bool Equals(object? o)
        {
            return o == null ? false : Equals(o as Type);
        }

        public override int GetHashCode()
        {
            Type systemType = UnderlyingSystemType;
            if (!systemType.Equals(this))
            {
                return systemType.GetHashCode();
            }

            return base.GetHashCode();
        }
        public virtual bool Equals(Type? o)
        {
            return o == null ? false : UnderlyingSystemType.Equals(o.UnderlyingSystemType);
        }
    }
}