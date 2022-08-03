using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
	/// Represents a field using an internal metadata token.
	/// </summary>
	public struct RuntimeFieldHandle
    {
        internal RuntimeFieldHandle(IntPtr handle)
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
            if (!(obj is RuntimeFieldHandle))
                return false;

            return ((RuntimeFieldHandle)obj).m_ptr == m_ptr;
        }

        public static bool operator ==(RuntimeFieldHandle left, RuntimeFieldHandle right)
        {
            return left.m_ptr == right.m_ptr;
        }

        public static bool operator !=(RuntimeFieldHandle left, RuntimeFieldHandle right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return m_ptr.ToInt32();
        }
    }
}
