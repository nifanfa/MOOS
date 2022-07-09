#if Kernel
using MOOS;
#endif

namespace System
{
    public struct Single 
    {
        public override unsafe string ToString()
        {
            return ((double)this).ToString();
        }
    }
}