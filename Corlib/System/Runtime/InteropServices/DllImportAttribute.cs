namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class DllImportAttribute : Attribute
    {
        public DllImportAttribute(string dllName)
        {
            Value = dllName;
        }

        public string Value { get; }

        public string EntryPoint;
        public bool SetLastError;
        public bool ExactSpelling;
        public CallingConvention CallingConvention;
        public bool BestFitMapping;
        public bool PreserveSig;
        public bool ThrowOnUnmappableChar;
    }

    public enum CharSet
    {
        None = 1,
        Ansi = 2,
        Unicode = 3,
        Auto = 4,
    }
}