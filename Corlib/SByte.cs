﻿#if Kernel
using MOOS;
#endif

namespace System
{
    public struct SByte
    {
        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }
}