#if Kernel
using Kernel;
using Kernel.Misc;

namespace System.Net
{
    public class UdpClient
    {
        IPAddress iPAddress;
        ushort Port;

        public void Connect(IPAddress address,ushort port) 
        {
            this.iPAddress = address;
            this.Port = port;
            if(UDP.Clients != null)
            {
                UDP.Clients.Add(this);
            }
            else 
            {
                Panic.Error("[UdpClient] Network is not initialized!");
            }
            DataAvailable = false;
            LastData = null;
        }

        public void Send(byte[] buffer) 
        {
            UDP.SendPacket(iPAddress.Address, Port, Port, buffer);
        }

        internal bool DataAvailable;
        private byte[] LastData;

        public unsafe void OnData(byte[] buffer)
        {
            if (LastData != null) LastData.Dispose();
            LastData = new byte[buffer.Length];
            fixed (byte* pl = LastData) 
            {
                fixed (byte* pb = buffer)
                {
                    Native.Movsb(pl, pb, (ulong)buffer.Length);
                }
            }
            DataAvailable = true;
        }

        public byte[] Receive() 
        {
            while (!DataAvailable) Native.Hlt();
            DataAvailable = false;
            return LastData;
        }
    }
}
#endif