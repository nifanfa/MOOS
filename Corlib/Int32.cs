#if Kernel
using MOOS;
#endif

namespace System
{
    public struct Int32 
    {
        public const int MinValue = -2147483648;

        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }
}