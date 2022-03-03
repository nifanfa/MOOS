// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
namespace System.Net
{
    public class IPAddress
    {
        public byte[] Address;

        public IPAddress(byte octal1, byte octal2, byte octal3, byte octal4)
        {
            Address = new byte[] {
                    octal1,
                    octal2,
                    octal3,
                    octal4
            };
        }

        public static IPAddress FromByteArray(params byte[] IP)
        {
            return new IPAddress(IP[0], IP[1], IP[2], IP[3]);
        }
        public override string ToString()
        {
            return Address[0] + "." + Address[1] + "." + Address[2] + "." + Address[3];
        }
    }
}
