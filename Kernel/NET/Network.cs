/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using System.Net;

namespace MOOS.NET
{
    public static class Network
    {
        public static byte[] MAC;
        public static byte[] IP;
        public static byte[] Mask;
        public static byte[] Boardcast;
        public static byte[] Gateway;

        public delegate void OnDataHandler(byte[] buffer);

        public static void Initialise(IPAddress IPAddress, IPAddress GatewayAddress, IPAddress SubnetMask)
        {
            Boardcast = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Gateway = GatewayAddress.Address;
            Mask = SubnetMask.Address;
            IP = IPAddress.Address;
            UDP.Clients = new();
            ARP.Initialise();
        }
    }
}
