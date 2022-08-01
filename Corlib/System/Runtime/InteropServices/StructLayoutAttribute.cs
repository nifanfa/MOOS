namespace System.Runtime.InteropServices
{
    public sealed class StructLayoutAttribute : Attribute
    {
        public StructLayoutAttribute(LayoutKind layoutKind)
        {
            Value = layoutKind;
        }

        public LayoutKind Value { get; }

        public int Pack;
        public int Size;
        public CharSet CharSet;
    }

    public enum LayoutKind
    {
        Sequential = 0,
        Explicit = 2,
        Auto = 3,
    }
}