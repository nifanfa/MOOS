using System;
using System.Collections.Generic;
using System.Text;

namespace System.Text
{
    public class StringBuilder
    {
        private char[] Characters = new char[int.MaxValue];

        private int Length = 0;

        public StringBuilder Clear()
        {
            Length = 0;
            return this;
        }

        public StringBuilder Append(char c)
        {
            if (Length >= Characters.Length)
            {
                var chars = new char[Length + 100];
                for (var i = 0; i < Length; i++)
                    chars[i] = Characters[i];
                Characters = chars;
            }

            Characters[Length] = c;
            Length++;

            return this;
        }

        public StringBuilder AppendLine(string s)
        {
            for (var i = 0; i < s.Length; i++)
                Append(s[i]);

            return this;
        }

        public char[] ToCharArray()
        {
            return Characters;
        }

        public override string ToString()
        {
            return new string(Characters, 0, Length);
        }
    }
}
