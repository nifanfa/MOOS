namespace MOOS.NET
{
    public abstract unsafe class NIC
    {
        public abstract void Send(byte* Data, int Length);
    }
}