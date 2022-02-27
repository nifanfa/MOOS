namespace System
{
    public struct Int16
    {
        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }
}
