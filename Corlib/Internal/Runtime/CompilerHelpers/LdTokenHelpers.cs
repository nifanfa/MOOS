using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.Runtime.CompilerHelpers
{
    /// <summary>
    /// These methods are used to implement ldtoken instruction.
    /// </summary>
    internal static class LdTokenHelpers
    {
        private static RuntimeTypeHandle GetRuntimeTypeHandle(IntPtr pEEType)
        {
            return new RuntimeTypeHandle(new EETypePtr(pEEType));
        }

        private static unsafe RuntimeMethodHandle GetRuntimeMethodHandle(IntPtr pHandleSignature)
        {
            RuntimeMethodHandle returnValue;
            *(IntPtr*)&returnValue = pHandleSignature;
            return returnValue;
        }

        private static unsafe RuntimeFieldHandle GetRuntimeFieldHandle(IntPtr pHandleSignature)
        {
            RuntimeFieldHandle returnValue;
            *(IntPtr*)&returnValue = pHandleSignature;
            return returnValue;
        }
    }
}
