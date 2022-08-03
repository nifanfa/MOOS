using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Runtime.InteropServices
{
    public static class Marshal
    {
        public static IntPtr GetFunctionPointerForDelegate(Delegate d)
        {
            if (d == null)
            {
                throw new ArgumentNullException(nameof(d));
            }

            return GetFunctionPointerForDelegateInternal(d);
        }

        public static IntPtr GetFunctionPointerForDelegate<TDelegate>(TDelegate d)
        {
            return GetFunctionPointerForDelegate((Delegate)(object)d);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern IntPtr GetFunctionPointerForDelegateInternal(Delegate d);
    }
}
