/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

using System.Runtime.CompilerServices;

namespace System
{
    public unsafe struct UIntPtr
    {
        private void* _value;

        public UIntPtr(void* value) { _value = value; }
        public UIntPtr(int value) { _value = (void*)value; }
        public UIntPtr(uint value) { _value = (void*)value; }
        public UIntPtr(long value) { _value = (void*)value; }
        public UIntPtr(ulong value) { _value = (void*)value; }

        [Intrinsic]
        public static readonly UIntPtr Zero;

        public bool Equals(UIntPtr ptr)
        {
            return _value == ptr._value;
        }

        public override bool Equals(object o)
        {
            return base.Equals(o);
        }

        public static explicit operator UIntPtr(int value)
        {
            return new(value);
        }

        public static explicit operator UIntPtr(uint value)
        {
            return new(value);
        }

        public static explicit operator UIntPtr(long value)
        {
            return new(value);
        }

        public static explicit operator UIntPtr(ulong value)
        {
            return new(value);
        }

        public static explicit operator UIntPtr(void* value)
        {
            return new(value);
        }

        public static explicit operator void*(UIntPtr value)
        {
            return value._value;
        }

        public static explicit operator int(UIntPtr value)
        {
            long l = (long)value._value;

            return checked((int)l);
        }

        public static explicit operator long(UIntPtr value)
        {
            return (long)value._value;
        }

        public static explicit operator ulong(UIntPtr value)
        {
            return (ulong)value._value;
        }

        public static explicit operator UIntPtr(IntPtr ptr)
        {
            return new() { _value = (void*)ptr };
        }

        public static UIntPtr operator +(UIntPtr a, uint b)
        {
            return new((byte*)a._value + b);
        }

        public static UIntPtr operator +(UIntPtr a, ulong b)
        {
            return new((byte*)a._value + b);
        }

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
