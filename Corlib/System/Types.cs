
using System.Runtime.CompilerServices;

namespace System
{
    public struct Void { }

    // The layout of primitive types is special cased because it would be recursive.
    // These really don't need any fields to work.
    public struct Boolean
    {
        public override string ToString()
            => this ? "true" : "false";
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

    public struct Byte { }

    public struct Int16 { }

    public struct UInt16 { }

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
            var val = this;
            char* x = stackalloc char[11];
            var i = 9;

            x[10] = '\0';

            do
            {
                var d = val % 10;
                val /= 10;

                d += 0x30;
                x[i--] = (char)d;
            } while (val > 0);

            i++;

            return new string(x + i, 0, 10 - i);
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
    public struct Single { }
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