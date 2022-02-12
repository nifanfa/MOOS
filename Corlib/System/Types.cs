
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

        public static bool IsDigit(char c)
            => c >= '0' && c <= '9';
    }

    public struct SByte { }

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

    public struct Int16 { }

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
        public const int MaxValue = 0x7FFFFFFF;

        // TODO: ToString for all other primitives
        public unsafe override string ToString()
        {
            var val = this;
            bool isNeg = BitHelpers.IsBitSet(val, 31);
            char* x = stackalloc char[12];
            var i = 10;

            x[11] = '\0';

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

            return new string(x + i, 0, 11 - i);
        }

        public static int Parse(string val)
        {
            // TODO: Throw an error on incorrect format
            int r = 0;

            for (var i = 0; i < val.Length; i++)
            {
                r *= 10;
                r += val[i] - 48;
            }

            return r;
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

    public struct Int64 { }

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

    public struct UIntPtr { }

    public struct Single 
    {
        public override unsafe string ToString()
        {
            char* p = stackalloc char[21];
            string s = new string(p, 0, 21);
            float_to_string(this, s);
            return s;
        }

        //https://stackoverflow.com/questions/7228438/convert-double-float-to-string
        /** Number on countu **/

        static int n_tu(int number, int count)
        {
            int result = 1;
            while (count-- > 0)
                result *= number;

            return result;
        }

        /*** Convert float to string ***/
        static void float_to_string(float f, string r)
        {
            long length, length2, number, position, sign;
            int i;
            float number2;

            sign = -1;   // -1 == positive number
            if (f < 0)
            {
                sign = '-';
                f *= -1;
            }

            number2 = f;
            number = (long)f;
            length = 0;  // Size of decimal part
            length2 = 0; // Size of tenth

            /* Calculate length2 tenth part */
            while ((number2 - (float)number) != 0.0 && !((number2 - (float)number) < 0.0))
            {
                number2 = f * (n_tu((int)10.0f, (int)(length2 + 1)));
                number = (long)number2;

                length2++;
            }

            /* Calculate length decimal part */
            for (length = (f > 1) ? 0 : 1; f > 1; length++)
                f /= 10;

            position = length;
            length = length + 1 + length2;
            number = (long)number2;
            if (sign == '-')
            {
                length++;
                position++;
            }

            for (i = (int)length; i >= 0; i--)
            {
                if (i == (length))
                    r[i] = '\0';
                else if (i == (position))
                    r[i] = '.';
                else if (sign == '-' && i == 0)
                    r[i] = '-';
                else
                {
                    r[i] = (char)((number % 10) + '0');
                    number /= 10;
                }
            }
            r.Length = (int)length;
        }
    }

    public struct Double { }

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
        protected internal object firstParameter;
        protected internal IntPtr functionPointer;


        protected void InitializeClosedInstance(object firstParameter, IntPtr functionPointer)
        {
            if (firstParameter == null)
                return;

            this.firstParameter = firstParameter;
            this.functionPointer = functionPointer;
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