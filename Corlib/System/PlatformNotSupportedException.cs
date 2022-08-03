using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    [Serializable]
    public class PlatformNotSupportedException : NotSupportedException
    {
        public PlatformNotSupportedException()
        {
        }

        public PlatformNotSupportedException(String message)
            : base(message)
        {
        }
    }
}
