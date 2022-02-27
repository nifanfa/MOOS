namespace System
{
    public struct Int32
    {
        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }
}
