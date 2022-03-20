// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
namespace System.Net
{
    public class IPAddress
    {
        public byte[] Address;

        public IPAddress(byte A, byte AA, byte AAA, byte AAAA)
        {
            Address = new byte[]
            {
                A,
                AA,
                AAA,
                AAAA
            };
        }

        public static IPAddress Parse(params byte[] IP)
        {
            return new IPAddress(IP[0], IP[1], IP[2], IP[3]);
        }

        public override string ToString()
        {
            return Address[0] + "." + Address[1] + "." + Address[2] + "." + Address[3];
        }
    }
}
