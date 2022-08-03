
namespace System
{
    /// <summary>
    /// Represents a type using an internal metadata token.
    /// </summary>
    public struct RuntimeTypeHandle
    {
        private readonly IntPtr m_type;

        public RuntimeTypeHandle(IntPtr handle) // FIXME: hack - should be internal, but the plug needs access
        {
            m_type = handle;
        }

        /// <summary>
        /// Gets a handle to the type represented by this instance.
        /// </summary>
        public IntPtr Value { get { return m_type; } }

        public bool Equals(RuntimeTypeHandle obj)
        {
            return obj.m_type == m_type;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RuntimeTypeHandle))
                return false;

            var handle = (RuntimeTypeHandle)obj;

            return handle.m_type == m_type;
        }

        public static bool operator ==(RuntimeTypeHandle left, object right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(object left, RuntimeTypeHandle right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(RuntimeTypeHandle left, object right)
        {
            return !left.Equals(right);
        }

        public static bool operator !=(object left, RuntimeTypeHandle right)
        {
            return !right.Equals(left);
        }

        public override int GetHashCode()
        {
            return m_type.ToInt32();
        }
    }
}
