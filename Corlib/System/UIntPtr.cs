using System.Runtime.CompilerServices;

namespace System
{
    public unsafe struct UIntPtr
    {
        void* _value;

        public UIntPtr(void* value) { _value = value; }
        public UIntPtr(int value) { _value = (void*)value; }
        public UIntPtr(uint value) { _value = (void*)value; }
        public UIntPtr(long value) { _value = (void*)value; }
        public UIntPtr(ulong value) { _value = (void*)value; }

        [Intrinsic]
        public static readonly UIntPtr Zero;

        public bool Equals(UIntPtr ptr)
            => _value == ptr._value;

        public static explicit operator UIntPtr(int value) => new UIntPtr(value);

        public static explicit operator UIntPtr(uint value) => new UIntPtr(value);

        public static explicit operator UIntPtr(long value) => new UIntPtr(value);

        public static explicit operator UIntPtr(ulong value) => new UIntPtr(value);

        public static explicit operator UIntPtr(void* value) => new UIntPtr(value);

        public static explicit operator void*(UIntPtr value) => value._value;

        public static explicit operator int(UIntPtr value)
        {
            var l = (long)value._value;

            return checked((int)l);
        }

        public static explicit operator long(UIntPtr value) => (long)value._value;

        public static explicit operator ulong(UIntPtr value) => (ulong)value._value;

        public static explicit operator UIntPtr(IntPtr ptr) => new UIntPtr() { _value = (void*)ptr };

        public static UIntPtr operator +(UIntPtr a, uint b)
            => new UIntPtr((byte*)a._value + b);

        public static UIntPtr operator +(UIntPtr a, ulong b)
            => new UIntPtr((byte*)a._value + b);

        public static bool operator ==(UIntPtr a, UIntPtr b)
        {
            return a._value == b._value;
        }

        public static bool operator !=(UIntPtr a, UIntPtr b)
        {
            return !(a._value == b._value);
        }

        public override string ToString()
        {
            return ((ulong)_value).ToString();
        }
    }
}
