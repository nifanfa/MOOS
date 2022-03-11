// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Internal.Runtime.CompilerHelpers;
using Internal.Runtime.CompilerServices;


namespace System
{
    public sealed unsafe class String
    {
        [Intrinsic]
        public static readonly string Empty = "";


        // The layout of the string type is a contract with the compiler.
        private int _length;
        internal char _firstChar;
        public static string Join<T>(T[] x, string c)
        {
            string o = string.Empty;
            for (int i = 0; i < x.Length; i++)
            {
                o += i == x.Length - 1 ? x[i] : x[i] + c;
            }
            return o;
        }

        public string Substring(int startIndex)
        {
            string o = string.Empty;
            for (int i = startIndex; i < Length; i++)
            {
                o += this[i];
            }
            return o;
        }
        public bool StartsWith(string s)
        {
            bool o = false;
            if (Length < s.Length)
            {
                return false;
            }

            for (int i = 0; i < s.Length; i++)
            {
                o = this[i] == s[i];
            }
            return o;
        }

        public string[] Split(char c)
        {
            string split = "";
            List<string> splits = new();
            for (int i = 0; i < Length; i++)
            {
                if (c == this[i] && split != "")
                {
                    splits.Add(split);
                    split = "";
                }
                else if (c != this[i])
                {
                    split += this[i];
                }
            }
            splits.Add(split);
            return splits.ToArray();
        }
        public int Length
        {
            [Intrinsic]
            get => _length;
            internal set => _length = value;
        }

        public unsafe char this[int index]
        {
            [Intrinsic]
            get => Unsafe.Add(ref _firstChar, index);

            set
            {
                fixed (char* p = &_firstChar)
                {
                    p[index] = value;
                }
            }
        }


#pragma warning disable 824
        public extern unsafe String(char* ptr);
        public extern String(char[] buf);
        public extern unsafe String(char* ptr, int index, int length);
        public extern unsafe String(char[] buf, int index, int length);

#pragma warning restore 824


        public static unsafe string FromASCII(IntPtr ptr, int length)
        {
            char[] buf = new char[length];
            byte* _ptr = (byte*)ptr;

            for (int i = 0; i < length; i++)
            {
                buf[i] = (char)_ptr[i];
            }

            string s = new(buf);
            buf.Dispose();

            return s;
        }

        public static unsafe string FromASCII(IntPtr ptr, int length, byte ignore)
        {
            char[] buf = new char[length];
            byte* _ptr = (byte*)ptr;

            int len = 0;

            for (int i = 0; i < length; i++)
            {
                if (_ptr[i] != ignore)
                {
                    buf[i] = (char)_ptr[i];
                    len++;
                }
            }

            string s = new(buf, 0, len);
            buf.Dispose();

            return s;
        }

        public string ToLower()
        {
            string x = "";
            for (int i = 0; i < Length; i++)
            {
                x += this[i].ToLower();
            }
            return x;
        }

        private static unsafe string Ctor(char* ptr)
        {
            int i = 0;

            while (ptr[i++] != '\0') { }

            return Ctor(ptr, 0, i - 1);
        }

        private static unsafe string Ctor(IntPtr ptr)
        {
            return Ctor((char*)ptr);
        }

        private static unsafe string Ctor(char[] buf)
        {
            fixed (char* _buf = buf)
            {
                return Ctor(_buf, 0, buf.Length);
            }
        }

        private static unsafe string Ctor(char* ptr, int index, int length)
        {
            EETypePtr et = EETypePtr.EETypePtrOf<string>();

            char* start = ptr + index;
            object data = StartupCodeHelpers.RhpNewArray(et.Value, length);
            string s = Unsafe.As<object, string>(ref data);

            fixed (char* c = &s._firstChar)
            {
                Allocator.MemoryCopy((IntPtr)c, (IntPtr)start, (ulong)length * sizeof(char));
                c[length] = '\0';
            }

            return s;
        }

        private static unsafe string Ctor(char[] ptr, int index, int length)
        {
            fixed (char* _ptr = ptr)
            {
                return Ctor(_ptr, index, length);
            }
        }

        public override string ToString()
        {
            return this;
        }
        public override bool Equals(object obj)
        {
            if (obj is not string)
            {
                return false;
            }

            return Equals((string)obj);
        }

        public bool Equals(string val)
        {
            if (Length != val.Length)
            {
                return false;
            }

            for (int i = 0; i < Length; i++)
            {
                if (this[i] != val[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator ==(string a, string b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(string a, string b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static string Concat(string a, string b)
        {
            int Length = a.Length + b.Length;
            char* ptr = stackalloc char[Length];
            int currentIndex = 0;
            for (int i = 0; i < a.Length; i++)
            {
                ptr[currentIndex] = a[i];
                currentIndex++;
            }
            for (int i = 0; i < b.Length; i++)
            {
                ptr[currentIndex] = b[i];
                currentIndex++;
            }
            return new string(ptr, 0, Length);
        }

        internal int IndexOf(char j)
        {
            for (int i = 0; i < Length; i++)
            {
                if (this[i] == j)
                {
                    return i;
                }
            }

            return -1;
        }

        public static string Concat(string a, string b, string c)
        {
            string p1 = a + b;
            string p2 = p1 + c;
            p1.Dispose();
            return p2;
        }

        public static string Concat(string a, string b, string c, string d)
        {
            string p1 = a + b;
            string p2 = p1 + c;
            string p3 = p2 + d;
            p1.Dispose();
            p2.Dispose();
            return p3;
        }

        public static string Concat(params string[] vs)
        {
            string s = "";
            for (int i = 0; i < vs.Length; i++)
            {
                string tmp = s + vs[i];
                s.Dispose();
                s = tmp;
            }
            vs.Dispose();
            return s;
        }
    }
}