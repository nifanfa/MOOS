namespace System.Runtime.CompilerServices
{
    public sealed class MethodImplAttribute : Attribute
    {
        public MethodImplAttribute(MethodImplOptions methodImplOptions) { }
    }

    public enum MethodImplOptions
    {
        Unmanaged = 0x0004,
        NoInlining = 0x0008,
        NoOptimization = 0x0040,
        AggressiveInlining = 0x0100,
        AggressiveOptimization = 0x200,
        InternalCall = 0x1000,
    }
}