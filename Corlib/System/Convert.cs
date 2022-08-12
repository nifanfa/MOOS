namespace System
{
    public static class Convert
    {
        public static unsafe string ToString(ulong value, ulong toBase)
        {
            if (toBase != 0 && toBase <= 16 && (toBase % 2) == 0)
            {
                char* x = stackalloc char[128];
                var i = 126;

                x[127] = '\0';

                do
                {
                    var d = value % toBase;
                    value /= toBase;

                    if (d > 9)
                        d += 0x37;
                    else
                        d += 0x30;

                    x[i--] = (char)d;
                } while (value > 0);

                i++;

                return new string(x + i, 0, 127 - i);
            }
            return null;
        }

        public static int ToUInt16(bool boolean)
        {
            return boolean ? 1 : 0;
        }

        public static int ToInt16(bool boolean)
        {
            return boolean ? 1 : 0;
        }

        public static int ToInt16(byte b)
        {
            return b;
        }

        public static bool ToBoolean(int integer)
        {
            return integer != 0;
        }

        public static byte ToByte(int v)
        {
            return (byte)v;
        }

        public static byte ToByte(uint v)
        {
            return (byte)v;
        }

        public static int ToInt32(byte b)
        {
            return b;
        }

        public static int ToInt32(int b)
        {
            return b;
        }

        public static long ToInt64(string str)
        {
            int i = 0;
            long val = 0;
            bool neg = false;
            if (str[0] == '-')
            {
                i = 1;
                neg = true;
            }
            for (; i < str.Length; i++)
            {
                val *= 10;
                val += str[i] - 0x30;
            }
            return neg ? -(val) : val;
        }

        public static int ToInt32(string str)
        {
            return (int)ToInt64(str);
        }

        public static short ToInt16(string str)
        {
            return (short)ToInt64(str);
        }

        public static sbyte ToInt8(string str)
        {
            return (sbyte)ToInt64(str);
        }

        public static ulong ToUInt64(string str)
        {
            return (ulong)ToInt64(str);
        }

        public static uint ToUInt32(string str)
        {
            return (uint)ToInt64(str);
        }

        public static ushort ToUInt16(string str)
        {
            return (ushort)ToInt64(str);
        }

        public static byte ToUInt8(string str)
        {
            return (byte)ToInt64(str);
        }
    }
}
