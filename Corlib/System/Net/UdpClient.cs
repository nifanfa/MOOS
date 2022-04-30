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
            Data = null;
        }

        public void Send(byte[] buffer) 
        {
            UDP.SendPacket(iPAddress.Address, Port, Port, buffer);
        }

        private byte[] Data;

        public unsafe void OnData(byte[] buffer)
        {
            if (Data != null) Data.Dispose();
            Data = new byte[buffer.Length];
            fixed (byte* dest = Data) 
            {
                fixed (byte* source = buffer)
                {
                    Native.Movsb(dest, source, (ulong)buffer.Length);
                }
            }
        }

        public unsafe byte[] Receive() 
        {
            while (Data == null) Native.Hlt();
            byte[] data  = new byte[Data.Length];
            fixed (byte* dest = data)
            {
                fixed (byte* source = Data)
                {
                    Native.Movsb(dest, source, (ulong)data.Length);
                }
            }
            Data.Dispose();
            Data = null;
            return data;
        }
    }
}
#endif