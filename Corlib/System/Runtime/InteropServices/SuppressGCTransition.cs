namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class SuppressGCTransitionAttribute : Attribute
    {
        public SuppressGCTransitionAttribute()
        {
        }
    }
}