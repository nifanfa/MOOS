using Internal.Runtime;
using Internal.Runtime.CompilerServices;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace System
{
    [Serializable]
    public unsafe class Object
    {
        // The layout of object is a contract with the compiler.
        internal unsafe EEType* m_pEEType;

        [StructLayout(LayoutKind.Sequential)]
        private class RawData
        {
            public byte Data;
        }

        internal ref byte GetRawData()
        {
            return ref Unsafe.As<RawData>(this).Data;
        }

        internal uint GetRawDataSize()
        {
            return m_pEEType->BaseSize - (uint)sizeof(ObjHeader) - (uint)sizeof(EEType*);
        }

        //[NonVersionable]
        public Object() { }
        //[NonVersionable]
        ~Object() { }

        public virtual bool Equals(object o)
        {
            return RuntimeHelpers.Equals(this, o);
        }

        public static bool Equals(object left, object right)
        {
            if (left == right)
                return true;

            if (left == null || right == null)
                return false;

            return left.Equals(right);
        }


        public virtual int GetHashCode()
        {
            return 0;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        protected extern object MemberwiseClone();

        [NonVersionable]
        public static bool ReferenceEquals(object left, object right)
        {
            return (left == right);
        }

        public Type GetType()
        { 
            return null;
        }

        public virtual string ToString()
        {
            return GetType().ToString();
        }


        public virtual void Dispose()
        {
            var obj = this;
            free(Unsafe.As<object, IntPtr>(ref obj));
        }
        /*
        public static bool operator !=(object a, object b)
        {
            return !(a.ToString() == b.ToString());
        }

        public static  bool operator ==(object a, object b)
        {
            return (a.ToString() == b.ToString());
        }
        */

        public static implicit operator bool(object obj)=> obj != null;

        public static implicit operator IntPtr(object obj) => Unsafe.As<object, IntPtr>(ref obj);

        [DllImport("*")]
        static extern ulong free(nint ptr);
    }
}
