// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace System
{
    public struct Char
    {
        public const char MaxValue = (char)0xffff;
        public const char MinValue = (char)0;

        internal char _value;

        public int CompareTo(char value)
        {
            if (_value < value)
            {
                return -1;
            }
            else if (_value > value)
            {
                return 1;
            }

            return 0;
        }

        public bool Equals(char obj)
        {
            return Equals((object)obj);
        }

        public override bool Equals(object obj)
        {
            return ((char)obj) == _value;
        }

        public static bool IsUpper(char c)
        {
            unsafe
            {
                ushort value = c;
                return value >= 65 && value <= 90;
            }
        }

        public static bool IsUpper(string s, int index)
        {
            return IsUpper(s[index]);
        }

        public static bool IsLower(char c)
        {
            unsafe
            {
                ushort value = c;
                return value >= 97 && value <= 122;
            }
        }

        public static bool IsLower(string s, int index)
        {
            return IsLower(s[index]);
        }

        public override string ToString()
        {
            string x = " ";
            x._firstChar = this;
            return x;
        }

        public override int GetHashCode()
        {
            return _value;
        }

        private static bool IsWhiteSpaceLatin1(char c)
        {
            // There are characters which belong to UnicodeCategory.Control but are considered as white spaces.
            // We use code point comparisons for these characters here as a temporary fix.

            // U+0009 = <control> HORIZONTAL TAB
            // U+000a = <control> LINE FEED
            // U+000b = <control> VERTICAL TAB
            // U+000c = <contorl> FORM FEED
            // U+000d = <control> CARRIAGE RETURN
            // U+0085 = <control> NEXT LINE
            // U+00a0 = NO-BREAK SPACE
            if ((c == ' ') || (c >= '\x0009' && c <= '\x000d') || c == '\x00a0' || c == '\x0085')
            {
                return true;
            }
            return false;
        }

        public static bool IsWhiteSpace(char c)
        {
            return IsWhiteSpaceLatin1(c);
        }

        public static bool IsLetter(char c)
        {
            unsafe
            {
                ushort value = c;
                return ((value >= 65 && value <= 90) || (value >= 97 && value <= 122));
            }
        }

        public static bool IsDigit(char c)
        {
            unsafe
            {
                ushort value = c;
                return (value >= 48 && value <= 57);
            }
        }

        public static bool IsLetterOrDigit(char c)
        {
            return (IsLetter(c) || IsDigit(c));
        }


        public static char ToUpper(char c)
        {
            if (IsUpper(c) || !IsLetter(c))
            {
                return c;
            }

            return (char)(c - 32);
        }

        public static char ToLower(char c)
        {
            if (IsLower(c) || !IsLetter(c))
            {
                return c;
            }

            return (char)(c + 32);
        }

        public char ToUpper()
        {
            if (IsUpper(this) || !IsLetter(this))
            {
                return this;
            }

            return (char)(this - 32);
        }

        public char ToLower()
        {
            if (IsLower(this) || !IsLetter(this))
            {
                return this;
            }

            return (char)(this + 32);
        }
        public static bool IsSurrogate(char c)
        {
            return c >= 0x00d800 && c <= 0x00dfff;
        }

        public static bool IsSurrogate(string s, int index)
        {
            if (s == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error(nameof(s));
            }

            if (((uint)index) >= ((uint)s.Length))
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error(nameof(index));
            }

            return IsSurrogate(s[index]);
        }

        public static bool IsHighSurrogate(char c)
        {
            return (c >= '\ud800') && (c <= '\udbff');
        }

        public static bool IsHighSurrogate(string s, int index)
        {
            if (s == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error(nameof(s));
            }

            if (index < 0 || index >= s.Length)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error(nameof(index));
            }

            return IsHighSurrogate(s[index]);
        }

        public static bool IsLowSurrogate(char c)
        {
            return (c >= '\udc00') && (c <= '\udfff');
        }

        public static bool IsLowSurrogate(string s, int index)
        {
            if (s == null)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error(nameof(s));
            }

            if (index < 0 || index >= s.Length)
            {
                /*throw new Exception*/
                Kernel.Misc.Panic.Error(nameof(index));
            }

            return IsLowSurrogate(s[index]);
        }

        public static bool operator ==(char left, char right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(char left, char right)
        {
            return !(left == right);
        }
    }
}
