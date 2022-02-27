// Copywrite (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
using System.Net;

namespace Kernel.NET
{
    public static class Network
    {
        public static byte[] MAC;
        public static byte[] IP;
        public static byte[] Boardcast;
        public static byte[] Gateway;

        public static void Initialise(IPAddress IPAddress, IPAddress GatewayAddress)
        {
            Boardcast = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Gateway = GatewayAddress.Address;
            IP = IPAddress.Address;
            ARP.Initialise();
        }
    }
}
