/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using MOOS.Misc;
using MOOS.NET;

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
			int PacketLen = sizeof(UDPHeader) + Data.Length;
			byte* Buffer = (byte*)Allocator.Allocate((ulong)PacketLen);
			UDPHeader* header = (UDPHeader*)Buffer;
			Native.Stosb(header, 0, (ulong)PacketLen);
			header->SrcPort = Ethernet.SwapLeftRight(SourcePort);
			header->DestPort = Ethernet.SwapLeftRight(DestPort);
			header->Length = Ethernet.SwapLeftRight((ushort)PacketLen);
			header->Checksum = 0;
			for (int i = 0; i < Data.Length; i++)
			{
				(Buffer + sizeof(UDPHeader))[i] = Data[i];
			}

			IPv4.SendPacket(DestIP, 17, Buffer, PacketLen);

			Console.WriteLine("UDP Packet Sent");
		}

		internal static void HandlePacket(byte* frame, int length)
		{
			UDPHeader* header = (UDPHeader*)frame;
			frame += sizeof(UDPHeader);
			length -= (ushort)sizeof(UDPHeader);

			header->SrcPort = Ethernet.SwapLeftRight(header->SrcPort);
			header->DestPort = Ethernet.SwapLeftRight(header->DestPort);

			int len = Ethernet.SwapLeftRight(header->Length);

			byte[] Buffer = new byte[len];
			fixed (byte* P = Buffer)
			{
				Native.Movsb(P, frame, (ulong)len);
			}

			for (int i = 0; i < Clients.Count; i++)
			{
				//When we send a packet we use dest port as local port
				if (Clients[i].Port == header->DestPort)
				{
					Clients[i]._OnData(Buffer);
					break;
				}
			}

			//Do something
			Buffer.Dispose();
		}
	}


	public class UdpClient
	{
		private IPAddress iPAddress;
		public ushort Port;
		public event Network.OnDataHandler OnData;

		internal void _OnData(byte[] buffer)
		{
			OnData?.Invoke(buffer);
		}

		public UdpClient(IPAddress address, ushort port)
		{
			iPAddress = address;
			Port = port;
			if (UDP.Clients != null)
			{
				UDP.Clients.Add(this);
			} else
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