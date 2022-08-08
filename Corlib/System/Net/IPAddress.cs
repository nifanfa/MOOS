namespace System.Net
{
    public unsafe class IPAddress
    {
        public uint AddressV4;

        public static IPAddress Parse(params byte[] IPAddressV4)
        {
            var IP = new IPAddress();
            fixed(uint* p = &IP.AddressV4)
            {
                ((byte*)p)[0] = IPAddressV4[0];
                ((byte*)p)[1] = IPAddressV4[1];
                ((byte*)p)[2] = IPAddressV4[2];
                ((byte*)p)[3] = IPAddressV4[3];
            }
            return IP;
        }

        public bool Equals(IPAddress b)
        {
            return this.AddressV4 == b.AddressV4;
        }

        public static bool operator ==(IPAddress a, IPAddress b)
        {
            return a.AddressV4 == b.AddressV4;
        }

        public static bool operator !=(IPAddress a, IPAddress b)
        {
            return !(a.AddressV4 == b.AddressV4);
        }
    }
}