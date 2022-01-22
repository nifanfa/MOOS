
namespace System.Runtime.CompilerServices
{
    public sealed class MethodImplAttribute : Attribute
    {
        public MethodImplAttribute(MethodImplOptions methodImplOptions) { }
    }

    public enum MethodImplOptions
    {
        NoInlining = 0x0008,
    }
}