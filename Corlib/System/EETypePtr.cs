using Internal.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
    public unsafe struct EETypePtr
    {
        internal EEType* _value;

        public bool IsSzArray 
        {
            get 
            {
                return _value->IsSzArray;
            }
        }

        internal bool IsValueType
        {
            get
            {
                return _value->IsValueType;
            }
        }

        // Has internal gc pointers. 
        internal bool HasPointers
        {
            get
            {
                return _value->HasGCPointers;
            }
        }

        public EETypePtr ArrayElementType 
        {
            get 
            {
                return new EETypePtr(_value->RelatedParameterType);
            }
        }

        internal int ArrayRank
        {
            get
            {
                return _value->ArrayRank;
            }
        }

        public IntPtr RawValue 
        {
            get 
            {
                return (IntPtr)_value;
            }
        }

        public EETypePtr(IntPtr value)
        {
            _value = (EEType*)value;
        }

        public EETypePtr(EEType* value)
        {
            _value = value;
        }

        [Intrinsic]
        internal static EETypePtr EETypePtrOf<T>()
        {
            // Compilers are required to provide a low level implementation of this method.
            // This can be achieved by optimizing away the reflection part of this implementation
            // by optimizing typeof(!!0).TypeHandle into "ldtoken !!0", or by
            // completely replacing the body of this method.
            return default;
        }
    }
}