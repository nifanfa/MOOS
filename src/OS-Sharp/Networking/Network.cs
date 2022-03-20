// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System.Net;

namespace OS_Sharp.Networking
{
    public static class Network
    {
        public static byte[] MAC;
        public static byte[] IP;
        public static byte[] Mask;
        public static byte[] Boardcast;
        public static byte[] Gateway;

        public static void Initialise(IPAddress IPAddress, IPAddress GatewayAddress, IPAddress SubnetMask)
        {
            Boardcast = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Gateway = GatewayAddress.Address;
            Mask = SubnetMask.Address;
            IP = IPAddress.Address;
            ARP.Initialise();
        }
    }
}
