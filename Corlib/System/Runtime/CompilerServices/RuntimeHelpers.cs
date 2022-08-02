using System.Reflection;

namespace System.Runtime.CompilerServices
{
    public static class RuntimeHelpers
    {
        
        public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);
        /*
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);
        */
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetHashCode(object o);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public new static extern bool Equals(object o1, object o2);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern T UnsafeCast<T>(object o) where T : class;


        // TODO: Implement?
        [Intrinsic]
        internal static bool EnumEquals<T>(T x, T y) where T : Enum
        {
            // The body of this function will be replaced by the EE with unsafe code
            // See getILIntrinsicImplementation for how this happens.
            return x.Equals(y);
        }
        

    }
}