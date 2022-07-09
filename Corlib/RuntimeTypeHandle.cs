#if Kernel
using MOOS;
#endif

namespace System
{
    public struct RuntimeTypeHandle 
    {
        IntPtr Value;

        public RuntimeTypeHandle(EETypePtr ptr)
        {
            Value = ptr.RawValue;
        }
    }
}