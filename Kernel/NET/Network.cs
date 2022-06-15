using MOOS.Driver;
using MOOS.Misc;
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

        public static NIC Controller;

        public delegate void OnDataHandler(byte[] buffer);

        public static void Initialise(IPAddress IPAddress, IPAddress GatewayAddress, IPAddress SubnetMask)
        {
            Controller = null;
            Boardcast = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            Gateway = GatewayAddress.Address;
            Mask = SubnetMask.Address;
            IP = IPAddress.Address;
            UDP.Clients = new();
            ARP.Initialise();
            TCP.Clients = new();

            MAC = null;

            RTL8139.Initialise();
            Intel8254X.Initialize();

            if (Controller == null) Panic.Error("No compatible network controller on this device!");
            if (MAC == null) Panic.Error("NIC didn't set Network.MAC");

            ARP.Require(Network.Gateway);
        }
    }
}