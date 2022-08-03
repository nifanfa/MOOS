
namespace System
{
    /// <summary>
    /// Represents a method using an internal metadata token.
    /// </summary>
    public struct RuntimeMethodHandle
    {
        internal RuntimeMethodHandle(IntPtr handle)
        {
            m_ptr = handle;
        }

        private readonly IntPtr m_ptr;

        /// <summary>
        /// Gets a handle to the type represented by this instance.
        /// </summary>
        public IntPtr Value
        {
            get
            {
                return m_ptr;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RuntimeMethodHandle))
                return false;

            return ((RuntimeMethodHandle)obj).m_ptr == m_ptr;
        }

        public static bool operator ==(RuntimeMethodHandle left, RuntimeMethodHandle right)
        {
            return left.m_ptr == right.m_ptr;
        }

        public static bool operator !=(RuntimeMethodHandle left, RuntimeMethodHandle right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return m_ptr.ToInt32();
        }
    }
}
