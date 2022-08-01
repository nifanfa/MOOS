using Internal.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
    public unsafe struct EETypePtr
    {
        internal EEType* Value;

        public bool IsSzArray 
        {
            get 
            {
                return Value->IsSzArray;
            }
        }

        public EETypePtr ArrayElementType 
        {
            get 
            {
                return new EETypePtr(Value->RelatedParameterType);
            }
        }

        internal int ArrayRank
        {
            get
            {
                return Value->ArrayRank;
            }
        }

        public IntPtr RawValue 
        {
            get 
            {
                return (IntPtr)Value;
            }
        }

        public EETypePtr(IntPtr value)
        {
            Value = (EEType*)value;
        }

        public EETypePtr(EEType* value)
        {
            Value = value;
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