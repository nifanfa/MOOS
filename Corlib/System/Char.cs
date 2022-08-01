namespace System
{
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
}
