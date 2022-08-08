using MOOS.Driver;
using MOOS.Misc;
using System.Net;

namespace MOOS.NET
{
    public static class Network
    {
        public static MACAddress MAC;
        public static IPAddress IP;
        public static IPAddress Mask;
        public static MACAddress Boardcast;
        public static IPAddress Gateway;

        public static NIC Controller;

        public delegate void OnDataHandler(byte[] buffer);

        public static void Initialise(IPAddress IPAddress, IPAddress GatewayAddress, IPAddress SubnetMask)
        {
            Controller = null;
            Boardcast = new MACAddress(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);
            Gateway = GatewayAddress;
            Mask = SubnetMask;
            IP = IPAddress;
            UDP.Clients = new();
            ARP.Initialise();
            TCP.Clients = new();

            MAC = default;

            RTL8139.Initialise();
            Intel8254X.Initialize();

            if (Controller == null) Panic.Error("No compatible network controller on this device!");
            if (MAC == default) Panic.Error("NIC didn't set Network.MAC");

            ARP.Require(Network.Gateway);
        }
    }
}