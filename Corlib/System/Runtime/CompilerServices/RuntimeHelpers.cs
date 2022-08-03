using System.Collections.Generic;
using System.Reflection;

namespace System.Runtime.CompilerServices
{
    public static class RuntimeHelpers
    {
        public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetHashCode(Object o);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public new static extern bool Equals(Object o1, Object o2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern T UnsafeCast<T>(Object o) where T : class;

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern IEnumerable<Assembly> GetAssemblies();

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern object CreateInstance(Type type, params object[] args);
    }
}