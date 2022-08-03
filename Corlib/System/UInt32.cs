
namespace System
{
    /// <summary>
    ///
    /// </summary>
    public struct UInt32
    {
        public const uint MaxValue = 0xffffffff;
        public const uint MinValue = 0;

        internal uint _value;

        public int CompareTo(uint value)
        {
            if (_value < value) return -1;
            else if (_value > value) return 1;
            return 0;
        }

        public bool Equals(uint obj)
        {
            return Equals((object)obj);
        }

        public override bool Equals(object obj)
        {
            return ((uint)obj) == _value;
        }

        public override string ToString()
        {
            return int.CreateString(_value, false, false);
        }

        public string ToString(string format)
        {
            return int.CreateString(_value, false, true);
        }

        public override int GetHashCode()
        {
            return (int)_value;
        }

        public static uint Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException();

            if (s.Length == 0)
                throw new FormatException();

            uint result;
            if (TryParse(s, out result))
                return result;

            throw new FormatException();
        }

        public static bool TryParse(string s, out uint result)
        {
            int len = s.Length;
            uint n = 0;
            result = 0;
            var i = 0;
            while (i < len)
            {
                if (n > (0xFFFFFFFF / 10))
                {
                    return false;
                }
                n *= 10;
                if (s[i] != '\0')
                {
                    uint newN = n + (uint)(s[i] - '0');
                    // Detect an overflow here...
                    if (newN < n)
                    {
                        return false;
                    }
                    n = newN;
                }
                i++;
            }
            result = n;
            return true;
        }
    }
}
