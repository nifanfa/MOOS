using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.InteropServices
{
    public static class NativeMethods
    {
        private const string Libname = "libNativeExceptionsThrower";

        [DllImport(Libname)]
        internal static extern void throw_runtime_exception();

        [DllImport(Libname)]
        internal static extern void throw_access_violation_exception();

        [DllImport(Libname)]
        internal static extern void throw_floating_point_exception();
    }
}
