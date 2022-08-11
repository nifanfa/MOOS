namespace System.Text
{
    public abstract unsafe class Encoding
    {
        public static Encoding UTF8;
        public static Encoding ASCII;

        public abstract string GetString(byte* ptr);
    }
}
