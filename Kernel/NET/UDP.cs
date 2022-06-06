/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using MOOS.Misc;
using MOOS.NET;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;

namespace MOOS
{
    public static unsafe class UDP
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UDPHeader
        {
            public ushort SrcPort;
            public ushort DestPort;
            public ushort Length;
            public ushort Checksum;
        }

        public static List<UdpClient> Clients;

        public static void SendPacket(byte[] DestIP, ushort SourcePort, ushort DestPort, byte[] Data)
        {
            int PacketLen = (sizeof(UDPHeader) + Data.Length);
            byte* Buffer = (byte*)Allocator.Allocate((ulong)PacketLen);
            UDPHeader* header = (UDPHeader*)Buffer;
            Native.Stosb(header, 0, (ulong)PacketLen);
            header->SrcPort = Ethernet.SwapLeftRight(SourcePort);
            header->DestPort = Ethernet.SwapLeftRight(DestPort);
            header->Length = Ethernet.SwapLeftRight(((ushort)PacketLen));
            header->Checksum = 0;
            for (int i = 0; i < Data.Length; i++) (Buffer + sizeof(UDPHeader))[i] = Data[i];

            IPv4.SendPacket(DestIP, 17, Buffer, PacketLen);

            Console.WriteLine("UDP Packet Sent");
        }

        internal static void HandlePacket(byte* frame, int length)
        {
            UDPHeader* header = (UDPHeader*)frame;
            frame += sizeof(UDPHeader);
            length -= (ushort)sizeof(UDPHeader);

            byte[] Buffer = new byte[length];
            fixed (byte* P = Buffer)
            {
                Native.Movsb(P, frame, (ulong)length);
            }

            for (int i = 0; i < Clients.Count; i++) 
            {
                Clients[i]._OnData(Buffer);
            }

            //Do something
            Buffer.Dispose();
        }
    }


    public class UdpClient
    {
        IPAddress iPAddress;
        ushort Port;
        public event Network.OnDataHandler OnData;

        internal void _OnData(byte[] buffer) 
        {
            OnData?.Invoke(buffer);
        }

        public UdpClient(IPAddress address, ushort port)
        {
            this.iPAddress = address;
            this.Port = port;
            if (UDP.Clients != null)
            {
                UDP.Clients.Add(this);
            }
            else
            {
                Panic.Error("[UdpClient] Network is not initialized!");
            }
        }

        public void Send(byte[] buffer)
        {
            UDP.SendPacket(iPAddress.Address, Port, Port, buffer);
        }
    }
}

