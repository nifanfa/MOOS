using System;
using System.Collections.Generic;
using System.Text;

namespace System.Text
{
    public abstract class Encoding
    {
        public static ASCIIEncoding ASCII;

        public static void Setup()
        {
            ASCII = new ASCIIEncoding();
        }

        public abstract String GetString(byte[] bytes, int index, int count);

        public virtual String GetString(byte[] bytes)
        {
            return GetString(bytes, 0, bytes.Length);
        }
    }
}
