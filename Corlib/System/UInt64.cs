
namespace System
{
    /// <summary>
	///
	/// </summary>
	public struct UInt64
    {
        public const ulong MaxValue = 0xffffffffffffffff;
        public const ulong MinValue = 0;

        internal ulong _value;

        public int CompareTo(ulong value)
        {
            if (_value < value) return -1;
            else if (_value > value) return 1;
            return 0;
        }

        public bool Equals(ulong obj)
        {
            return Equals((object)obj);
        }

        public override bool Equals(object obj)
        {
            return ((ulong)obj) == _value;
        }

        public override int GetHashCode()
        {
            return (int)_value;
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

        public unsafe string ToString(string format)
        {
            if (format == "x2")
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
    }
}
