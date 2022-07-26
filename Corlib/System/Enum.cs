using System.Runtime.CompilerServices;

namespace System
{
    public abstract class Enum : ValueType
    {
        [Intrinsic]
        public bool HasFlag(Enum flag)
        {
            return false;
        }
    }
}
