/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Internal.Runtime.CompilerHelpers;
using Internal.Runtime.CompilerServices;
using MOOS.Misc;
using System.Runtime.CompilerServices;


namespace System
{
    public sealed unsafe class String
    {
        [Intrinsic]
        public static readonly string Empty = "";


        // The layout of the string type is a contract with the compiler.
        int _length;
        internal char _firstChar;


        public int Length
        {
            [Intrinsic]
            get { return _length; }
            internal set { _length = value; }
        }

        public unsafe char this[int index]
        {
            [Intrinsic]
            get
            {
                return Unsafe.Add(ref _firstChar, index);
            }

            set 
            {
                fixed (char* p = &_firstChar) p[index] = value;
            }
        }


#pragma warning disable 824
        public extern unsafe String(char* ptr);
        public extern String(IntPtr ptr);
        public extern String(char[] buf);
        public extern unsafe String(char* ptr, int index, int length);
        public extern unsafe String(char[] buf, int index, int length);
#pragma warning restore 824


        public unsafe static string FromASCII(IntPtr ptr, int length)
        {
            var buf = new char[length];
            var _ptr = (byte*)ptr;

            for (int i = 0; i < length; i++)
                buf[i] = (char)_ptr[i];

            var s = new string(buf);
            buf.Dispose();

            return s;
        }

        public unsafe static string FromASCII(IntPtr ptr, int length, byte ignore)
        {
            var buf = new char[length];
            var _ptr = (byte*)ptr;

            int len = 0;

            for (int i = 0; i < length; i++)
                if (_ptr[i] != ignore)
                {
                    buf[i] = (char)_ptr[i];
                    len++;
                }

            var s = new string(buf, 0, len);
            buf.Dispose();

            return s;
        }

#if Kernel
        static unsafe string Ctor(char* ptr)
        {
            var i = 0;

            while (ptr[i++] != '\0') { }

            return Ctor(ptr, 0, i - 1);
        }
#endif

#if Kernel
        static unsafe string Ctor(IntPtr ptr)
            => Ctor((char*)ptr);
#endif

#if Kernel
        static unsafe string Ctor(char[] buf)
        {
            fixed (char* _buf = buf)
                return Ctor(_buf, 0, buf.Length);
        }
#endif

#if Kernel
        static unsafe string Ctor(char* ptr, int index, int length)
        {
            var et = EETypePtr.EETypePtrOf<string>();

            var start = ptr + index;
            var data = StartupCodeHelpers.RhpNewArray(et.Value, length);
            var s = Unsafe.As<object, string>(ref data);

            fixed (char* c = &s._firstChar)
            {
                Allocator.MemoryCopy((IntPtr)c, (IntPtr)start, (ulong)length * sizeof(char));
                c[length] = '\0';
            }

            return s;
        }
#endif

#if Kernel
        static unsafe string Ctor(char[] ptr, int index, int length)
        {
            fixed (char* _ptr = ptr)
                return Ctor(_ptr, index, length);
        }
#endif

        public override string ToString()
        {
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj is not string)
                return false;

            return Equals((string)obj);
        }

        public bool Equals(string val)
        {
            if (this.Length != val.Length)
                return false;

            for (int i = 0; i < this.Length; i++)
            {
                if (this[i] != val[i])
                    return false;
            }

            return true;
        }

        public static bool operator ==(string a, string b)
            => a.Equals(b);

        public static bool operator !=(string a, string b)
            => !a.Equals(b);

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
            for (int i = 0; i < this.Length; i++) if (this[i] == j) return i;
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

#if Kernel
        public static string Format(string format, params object[] args) 
        {
            string res = string.Empty;
            for(int i = 0; i < format.Length; i++) 
            {
                string chr = null;
                if ((i + 2) < format.Length && format[i] == '{' && format[i + 2] == '}')
                {
                    chr = args[(int)(format[i + 1] - 0x30)].ToString();
                    i += 2;
                }
                else
                {
                    chr = format[i].ToString();
                }
                string str = res + chr;
                chr.Dispose();
                res.Dispose();
                res = str;
            }

            for (int i = 0; i < args.Length; i++) args[i].Dispose();
            args.Dispose();
            return res;
        }
#endif
    }
}