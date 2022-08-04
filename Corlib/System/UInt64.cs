
namespace System
{
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
}
