using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System
{
    [Serializable]
    public struct UIntPtr
    {
        private readonly unsafe void* _value; // Do not rename (binary serialization)

        public static readonly UIntPtr Zero;

        [NonVersionable]
        public unsafe UIntPtr(uint value)
        {
            _value = (void*)value;
        }

        [NonVersionable]
        public unsafe UIntPtr(ulong value)
        {
            _value = (void*)((uint)value);
        }

        [NonVersionable]
        public unsafe UIntPtr(void* value)
        {
            _value = value;
        }

        public unsafe override bool Equals(Object obj)
        {
            if (obj is UIntPtr)
            {
                return (_value == ((UIntPtr)obj)._value);
            }
            return false;
        }

        public unsafe override int GetHashCode()
        {
            return ((int)_value);
        }

        [NonVersionable]
        public unsafe uint ToUInt32()
        {
            return ((uint)_value);
        }

        [NonVersionable]
        public unsafe ulong ToUInt64()
        {
            return (ulong)_value;
        }

        [NonVersionable]
        public static explicit operator UIntPtr(uint value)
        {
            return new UIntPtr(value);
        }

        [NonVersionable]
        public static explicit operator UIntPtr(ulong value)
        {
            return new UIntPtr(value);
        }

        [NonVersionable]
        public static unsafe explicit operator UIntPtr(void* value)
        {
            return new UIntPtr(value);
        }

        [NonVersionable]
        public static unsafe explicit operator void*(UIntPtr value)
        {
            return value._value;
        }

        [NonVersionable]
        public static unsafe explicit operator uint(UIntPtr value)
        {
            return (uint)value._value;
        }

        [NonVersionable]
        public static unsafe explicit operator ulong(UIntPtr value)
        {
            return (ulong)value._value;
        }

        [NonVersionable]
        public static unsafe bool operator ==(UIntPtr value1, UIntPtr value2)
        {
            return value1._value == value2._value;
        }

        [NonVersionable]
        public static unsafe bool operator !=(UIntPtr value1, UIntPtr value2)
        {
            return value1._value != value2._value;
        }

        [NonVersionable]
        public static UIntPtr Add(UIntPtr pointer, int offset)
        {
            return pointer + offset;
        }

        [NonVersionable]
        public static unsafe UIntPtr operator +(UIntPtr pointer, int offset)
        {
            return new UIntPtr((ulong)((long)pointer._value + offset));
        }

        [NonVersionable]
        public static UIntPtr Subtract(UIntPtr pointer, int offset)
        {
            return pointer - offset;
        }

        [NonVersionable]
        public static unsafe UIntPtr operator -(UIntPtr pointer, int offset)
        {
            return new UIntPtr((ulong)((long)pointer._value - offset));
        }

        public static unsafe int Size
        {
            [NonVersionable]
            get
            {
                return sizeof(void*);
            }
        }

        [NonVersionable]
        public unsafe void* ToPointer()
        {
            return _value;
        }

        public unsafe override string ToString()
        {
            return ((long)_value).ToString();
        }
    }
}
