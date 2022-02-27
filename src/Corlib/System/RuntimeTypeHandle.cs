namespace System
{
    public struct RuntimeTypeHandle
    {
        public IntPtr Value;

        public RuntimeTypeHandle(EETypePtr ptr)
        {
            Value = ptr.RawValue;
        }
    }
}
