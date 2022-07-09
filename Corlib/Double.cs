using System.Runtime.InteropServices;

namespace System
{
    public unsafe struct Double
    {
#if Kernel
        [DllImport("*")]
        public static extern void double_tostring(byte* buffer, double value);

        public override string ToString()
        {
            char* p = stackalloc char[22];
            byte* buffer = stackalloc byte[22];
            double_tostring(buffer, this);
            int length = 0;
            while (buffer[length] != 0)
            {
                p[length] = (char)buffer[length];
                length++;
            }

            string s = new string(p, 0, length);
            return s;
        }
#endif
    }
}