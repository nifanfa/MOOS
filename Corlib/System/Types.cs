/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
#if Kernel
using Kernel;
#endif
using System.Runtime.CompilerServices;

namespace System
{
    public struct Void { }

    // The layout of primitive types is special cased because it would be recursive.
    // These really don't need any fields to work.
    public unsafe struct Boolean
    {
        public override string ToString()
            => this ? "true" : "false";

        public static implicit operator bool(byte value)=>value !=0;
        public static implicit operator bool(sbyte value)=>value !=0;
        public static implicit operator bool(short value)=>value !=0;
        public static implicit operator bool(ushort value)=>value !=0;
        public static implicit operator bool(int value)=>value !=0;
        public static implicit operator bool(uint value)=>value !=0;
        public static implicit operator bool(long value) => value != 0;
        public static implicit operator bool(ulong value) => value != 0;
        public static implicit operator bool(float value) => value != 0;
        public static implicit operator bool(double value) => value != 0;
        public static implicit operator bool(void* value) => value != 0;
    }

    public struct Char
    {
        public override string ToString()
        {
            var r = " ";
            r._firstChar = this;

            return r;
        }

        public char ToUpper() 
        {
            char chr = this;
            if (chr >= 'a' && chr <= 'z')
                chr -= (char)('a' - 'A');
            return chr;
        }

        public static bool IsDigit(char c)
            => c >= '0' && c <= '9';
    }

    public struct SByte
    {
        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }

    public struct Byte
    {
        public unsafe override string ToString()
        {
            return ((ulong)this).ToString();
        }

        public string ToString(string format)
        {
            return ((ulong)this).ToString(format);
        }
    }

    public struct Int16 
    {
        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }

    public struct UInt16 
    {
        public unsafe override string ToString()
        {
            return ((ulong)this).ToString();
        }

        public string ToString(string format)
        {
            return ((ulong)this).ToString(format);
        }
    }

    public struct Int32 
    {
        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }

    public struct UInt32
    {
        public unsafe override string ToString()
        {
            return ((ulong)this).ToString();
        }

        public string ToString(string format)
        {
            return ((ulong)this).ToString(format);
        }
    }

    public unsafe struct Int64 
    {
        public override string ToString()
        {
            var val = this;
            bool isNeg = BitHelpers.IsBitSet(val, 63);
            char* x = stackalloc char[22];
            var i = 20;

            x[21] = '\0';

            if (isNeg)
            {
                ulong _val = (ulong)val;
                _val = 0xFFFFFFFFFFFFFFFF - _val;
                _val += 1;
                val = (long)_val;
            }

            do
            {
                var d = val % 10;
                val /= 10;

                d += 0x30;
                x[i--] = (char)d;
            } while (val > 0);

            if (isNeg)
                x[i] = '-';
            else
                i++;

            return new string(x + i, 0, 21 - i);
        }
    }

    public struct UInt64
    {
        public unsafe override string ToString()
        {
            var val = this;
            char* x = stackalloc char[21];
            var i = 19;

            x[20] = '\0';

            do
            {
                var d = val % 10;
                val /= 10;

                d += 0x30;
                x[i--] = (char)d;
            } while (val > 0);

            i++;

            return new string(x + i, 0, 20 - i);
        }

        public unsafe string ToString(string format) 
        {
            if(format == "x2")
            {
                format.Dispose();
                return this.ToStringHex();
            }
            else
            {
                format.Dispose();
                return this.ToString();
            }
        }

        public unsafe string ToStringHex()
        {
            var val = this;
            char* x = stackalloc char[21];
            var i = 19;

            x[20] = '\0';

            do
            {
                var d = val % 16;
                val /= 16;

                if (d > 9)
                    d += 0x37;
                else
                    d += 0x30;
                x[i--] = (char)d;
            } while (val > 0);

            i++;

            return new string(x + i, 0, 20 - i);
        }
    }

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

    public struct Single 
    {
        public override unsafe string ToString()
        {
            return ((double)this).ToString();
        }
    }

    public unsafe struct Double
    {
        public override string ToString()
        {
            char* p = stackalloc char[22];
            string s = new string(p, 0, 22);
            dtoa(s, this);
            return s;

        }

        static void dtoa(string c, double d)
        {
            int i = 0, e = 0, n = 0, flag = 0;//flag=0E+;1E-

            if (d < 0)
            {
                c[i++] = '-';
                d = -d;
            }
            while (d >= 10)
            {
                d /= 10;//here is the problem
                e++;
            }
            while (d < 1)
            {
                d *= 10;
                e++;
                flag = 1;
            }
            int v = (int)d, dot;
            c[i++] = (char)('0' + v);//the integer part
            dot = i;
            n++;
            c[i++] = '.';
            d -= v;
            while (d != 0 && n < 10)
            {
                d *= 10;
                v = (int)d;
                c[i++] = (char)('0' + v);
                n++;
                d -= v;
            }
            if (d != 0)
            {

                if (d * 10 >= 5)//rounding
                {
                    int j = i - 1;
                    c[j]++;
                    while (c[j] > '9')
                    {
                        c[j] = '0';
                        if (j - 1 == dot)
                            j--;
                        c[--j]++;
                    }
                }
            }
            else
            {
                while (n < 10)
                {
                    c[i++] = '0';
                    n++;
                }
            }

            if (e != 0)
            {
                c[i++] = 'E';
                c[i++] = (flag == 0) ? '+' : '-';
                if (e >= 100)
                {
                    int tmp = e / 100;
                    c[i++] = (char)('0' + tmp);
                    e -= (tmp * 100);
                    c[i++] = (char)('0' + e / 10);
                    c[i++] = (char)('0' + e % 10);
                }
                else if (e <= 9)
                {
                    c[i++] = '0';
                    c[i++] = '0';
                    c[i++] = (char)('0' + e);
                }
                else
                {
                    c[i++] = '0';
                    c[i++] = (char)('0' + e / 10);
                    c[i++] = (char)('0' + e % 10);
                }
            }
            c[i] = '\0';
            c.Length = i;
        }
    }

    public abstract class ValueType { }

    public abstract class Enum : ValueType
    {
        [Intrinsic]
        public bool HasFlag(Enum flag)
        {
            return false;
        }
    }

    public abstract class Delegate
    {
        protected internal object m_firstParameter;
        protected internal object m_helperObject;
        protected internal IntPtr m_extraFunctionPointerOrData;
        protected internal IntPtr m_functionPointer;

        public static Delegate? Combine(Delegate? a, Delegate? b)
        {
            return b;
        }

        public static Delegate? Remove(Delegate? source, Delegate? value)
        {
            return null;
        }

        // This function is known to the compiler backend.
        protected void InitializeOpenStaticThunk(object firstParameter, IntPtr functionPointer, IntPtr functionPointerThunk)
        {
            // This sort of delegate is invoked by calling the thunk function pointer with the arguments to the delegate + a reference to the delegate object itself.
            m_firstParameter = this;
            m_functionPointer = functionPointerThunk;
            m_extraFunctionPointerOrData = functionPointer;
        }

        // This function is known to the IL Transformer.
        protected void InitializeClosedInstance(object firstParameter, IntPtr functionPointer)
        {
            if (firstParameter == null)
            {
                m_firstParameter = this;
            }
            else
            {
                m_firstParameter = firstParameter;
            }

            m_functionPointer = functionPointer;
        }
    }

    public abstract class MulticastDelegate : Delegate { }

    public struct Nullable<T> where T : struct { }

    public struct RuntimeTypeHandle 
    {
        IntPtr Value;

        public RuntimeTypeHandle(EETypePtr ptr)
        {
            Value = ptr.RawValue;
        }
    }
    public struct RuntimeMethodHandle { }
    public struct RuntimeFieldHandle { }
}