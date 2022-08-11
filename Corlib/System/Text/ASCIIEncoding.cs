using System;
using System.Collections.Generic;
using System.Text;

namespace System.Text
{
    internal unsafe class ASCIIEncoding : Encoding
    {
        public override string GetString(byte* ptr)
        {
            int length = string.strlen(ptr);
            byte* p = (byte*)ptr;
            char* newp = stackalloc char[length];
            for (int i = 0; i < length; i++)
            {
                newp[i] = (char)p[i];
            }
            return new string(newp, 0, length);
        }
    }
}
