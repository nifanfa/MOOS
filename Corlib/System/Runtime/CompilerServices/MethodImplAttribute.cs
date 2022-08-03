namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false)]
    [Serializable]
    public sealed class MethodImplAttribute : Attribute
    {
        private readonly MethodImplOptions options;

        public MethodImplAttribute()
        {
        }

        public MethodImplAttribute(short options)
        {
            this.options = (MethodImplOptions)options;
        }

        public MethodImplAttribute(MethodImplOptions options)
        {
            this.options = options;
        }

        public MethodImplOptions Value
        {
            get
            {
                return options;
            }
        }
    }
}