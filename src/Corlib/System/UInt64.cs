// Copywrite (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace System
{
    public struct UInt64
    {
        public override unsafe string ToString()
        {
            ulong val = this;
            char* x = stackalloc char[21];
            int i = 19;

            x[20] = '\0';

            do
            {
                ulong d = val % 10;
                val /= 10;

                d += 0x30;
                x[i--] = (char)d;
            } while (val > 0);

            i++;

            return new string(x + i, 0, 20 - i);
        }

        public unsafe string ToString(string format)
        {
            if (format == "x2")
            {
                format.Dispose();
                return ToStringHex();
            }
            else
            {
                format.Dispose();
                return ToString();
            }
        }

        public unsafe string ToStringHex()
        {
            ulong val = this;
            char* x = stackalloc char[21];
            int i = 19;

            x[20] = '\0';

            do
            {
                ulong d = val % 16;
                val /= 16;

                if (d > 9)
                {
                    d += 0x37;
                }
                else
                {
                    d += 0x30;
                }

                x[i--] = (char)d;
            } while (val > 0);

            i++;

            return new string(x + i, 0, 20 - i);
        }
    }
}
