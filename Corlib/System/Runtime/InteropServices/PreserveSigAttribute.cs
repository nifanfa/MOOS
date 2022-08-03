using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    [ComVisible(true)]
    public sealed class PreserveSigAttribute : Attribute
    {
        public PreserveSigAttribute()
        {
        }
    }
}
