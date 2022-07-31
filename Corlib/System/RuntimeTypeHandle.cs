namespace System
{
    public struct RuntimeTypeHandle
    {
        private IntPtr Value;

        public RuntimeTypeHandle(EETypePtr ptr)
        {
            Value = ptr.RawValue;
        }
    }
}