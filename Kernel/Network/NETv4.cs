//In order to make this work the NIC driver should:
//Set NETv4.MAC
//Set NETv4.Sender (Warning: Make sure the packet was sent and return)
//Call NETv4.OnData when data received

//TODO - looks it doesn't work on GUI mode? stackoverflow?

using MOOS.Driver;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MOOS
{
    /// <summary>
    /// Implementation of Ethernet, IPv4, ARP, ICMP, UDP, TCP, DHCP(client), DNS(client)
    /// </summary>
    internal unsafe class NETv4
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MACAddress
        {
            internal byte P1;
            internal byte P2;
            internal byte P3;
            internal byte P4;
            internal byte P5;
            internal byte P6;

            public MACAddress(byte p1, byte p2, byte p3, byte p4, byte p5, byte p6)
            {
                this.P1 = p1;
                this.P2 = p2;
                this.P3 = p3;
                this.P4 = p4;
                this.P5 = p5;
                this.P6 = p6;
            }

            public static bool operator ==(MACAddress a, MACAddress b)
            {
                return
                    a.P1 == b.P1 &&
                    a.P2 == b.P2 &&
                    a.P3 == b.P3 &&
                    a.P4 == b.P4 &&
                    a.P5 == b.P5 &&
                    a.P6 == b.P6;
            }

            public static bool operator !=(MACAddress a, MACAddress b)
            {
                return !(a == b);
            }
        }

        public static IPAddress IP;
        public static IPAddress Mask;
        public static IPAddress GatewayIP;

        public static MACAddress MAC;

        public static delegate*<byte*, int, void> Sender;

        public static void Initialize()
        {
            NETv4.IP = default;
            NETv4.Mask = default;
            NETv4.GatewayIP = default;
            NETv4.ARPTable = new List<ARPCache>();
            NETv4.TCPClients = new List<TCPClient>();
            NETv4.UDPClients = new List<UDPClient>();
            NETv4.ICMPReplyBytes = 0;
            NETv4.ICMPReplyTTL = 0;
            NETv4.IsICMPRespond = false;
            NETv4.MAC = default;
            Sender = null;
        }

        public static void Configure(IPAddress address, IPAddress gateway, IPAddress mask)
        {
            NETv4.IP = address;
            NETv4.Mask = mask;
            NETv4.GatewayIP = gateway;
        }

        public static ushort SwapLeftRight(ushort Value)
        {
            return ((ushort)((((Value) & 0xff) << 8) | (((Value) & 0xff00) >> 8)));
        }

        public static uint SwapLeftRight(uint val)
        {
            uint prev = val;
            return ((prev & 0x000000ff) << 24) +
              ((prev & 0x0000ff00) << 8) +
              ((prev & 0x00ff0000) >> 8) +
              ((prev & 0xff000000) >> 24);
        }

        public static void SwapLeftRight(ref ushort Value)
        {
            Value = ((ushort)((((Value) & 0xff) << 8) | (((Value) & 0xff00) >> 8)));
        }

        public static void SwapLeftRight(ref uint val)
        {
            uint prev = val;
            val = ((prev & 0x000000ff) << 24) +
              ((prev & 0x0000ff00) << 8) +
              ((prev & 0x00ff0000) >> 8) +
              ((prev & 0xff000000) >> 24);
        }

        public static void OnData(byte* ptr)
        {
            EthernetHeader* eth_hdr = (EthernetHeader*)ptr;
            ptr += sizeof(EthernetHeader);
            SwapLeftRight(ref eth_hdr->EthernetType);
            switch (eth_hdr->EthernetType)
            {
                case 0x0806: ARPOnData((ARPHeader*)ptr); break;
                case 0x0800: IPOnData(ptr); break;
            }
        }

        #region DNS
        public struct DNSHeader
        {
            public ushort ID;
            public ushort Flags;
            public ushort QuestionCount;
            public ushort AnswerCount;
            public ushort AuthorityCount;
            public ushort AdditionalCount;
        }

        public static IPAddress DNSQuery(string host)
        {
            UDPClient udp = new UDPClient(new IPAddress(8, 8, 8, 8), 53, 5418);
            udp.Bind();
            byte* buffer = stackalloc byte[384];

            {
                byte* p = buffer;
                DNSHeader* dns_hdr = (DNSHeader*)buffer;
                p += sizeof(DNSHeader);
                dns_hdr->ID = 0;
                dns_hdr->Flags = 0x0100;
                dns_hdr->QuestionCount = 1;
                SwapLeftRight(ref dns_hdr->ID);
                SwapLeftRight(ref dns_hdr->Flags);
                SwapLeftRight(ref dns_hdr->QuestionCount);
                dns_hdr->AnswerCount = 0;
                dns_hdr->AuthorityCount = 0;
                dns_hdr->AdditionalCount = 0;
                byte* len = p++;
                fixed (char* phost = host)
                {
                    char* ptr = phost;
                    for (; ; )
                    {
                        char c = *ptr++;
                        if (c == '.' || c == '\0')
                        {
                            uint part_len = (uint)(p - len - 1);
                            *len = (byte)part_len;
                            len = p;
                        }
                        *p++ = (byte)c;
                        if (c == '\0') break;
                    }
                }
                *(ushort*)p = SwapLeftRight(1);
                p += 2;
                *(ushort*)p = SwapLeftRight(1);
                p += 2;

                udp.Send(buffer, (int)(p - buffer));
            }

            {
                byte[] data = null;
                while ((data = udp.Receive()) == null) ACPITimer.Sleep(10);
                fixed(byte* ptr = data)
                {
                    byte* p = ptr;
                    DNSHeader* dns_hdr = (DNSHeader*)ptr;
                    SwapLeftRight(ref dns_hdr->ID);
                    SwapLeftRight(ref dns_hdr->Flags);
                    SwapLeftRight(ref dns_hdr->QuestionCount);
                    SwapLeftRight(ref dns_hdr->AnswerCount);
                    SwapLeftRight(ref dns_hdr->AuthorityCount);
                    SwapLeftRight(ref dns_hdr->AdditionalCount);
                    p += sizeof(DNSHeader);
                    for(int i = 0; i < dns_hdr->QuestionCount; i++)
                    {
                        byte length;
                        while ((length = *p++) != 0)
                        {
                            p += length;
                        }
                        ushort type = SwapLeftRight(*(ushort*)p);
                        p += 2;
                        ushort Class = SwapLeftRight(*(ushort*)p);
                        p += 2;
                    }
                    for (int i = 0; i < dns_hdr->AnswerCount; i++)
                    {
                        byte len = *p++;
                        if (len >= 64)
                        {
                            ushort offset = (ushort)((len & 0x3f) << 6 | (*p++));
                        }
                        else
                        {
                            p += len;
                        }
                        ushort type = SwapLeftRight(*(ushort*)p);
                        p += 2;
                        ushort Class = SwapLeftRight(*(ushort*)p);
                        p += 2;
                        uint ttl = SwapLeftRight(*(uint*)p);
                        p += 4;
                        ushort length = SwapLeftRight(*(ushort*)p);
                        p += 2;
                        IPAddress ip = *(IPAddress*)p;
                        p += length;
                        //A
                        if(type == 1)
                        {
                            Console.Write($"[DNS] type:{(ulong)type} class:{(ulong)Class} ttl:{(ulong)ttl} length:{(ulong)length} {(ulong)ip.P1}.{(ulong)ip.P2}.{(ulong)ip.P3}.{(ulong)ip.P4}\n");
                            udp.Remove();
                            return ip;
                        }
                    }
                }
            }

            udp.Remove();
            return default;
        }
        #endregion

        #region DHCP
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DHCPHeader
        {
            public byte MessageType;
            public byte HardwareType;
            public byte HardwareLength;
            public byte Hops;
            public uint ID;
            public ushort Elasped;
            public ushort Flags;
            public IPAddress ClientIP;
            public IPAddress YourIP;
            public IPAddress ServerIP;
            public IPAddress GatewayIP;
            public MACAddress ClientMAC;
            public fixed byte Reserved[10];
            public fixed byte ServerName[64];
            public fixed byte BootFilename[128];
        }

        public static bool DHCPDiscover()
        {
            if (NETv4.IP != default) return false;

            UDPClient udp = new UDPClient(new IPAddress(0xFF, 0xFF, 0xFF, 0xFF), 67, 68);

            uint xid = 0x6166696E;
            byte* buffer = stackalloc byte[512];

            {
                byte* p = buffer;
                p += MakeDHCPRequestPacket(p, xid, 1);
                *p++ = 55;
                *p++ = 3;
                *p++ = 1;
                *p++ = 3;
                *p++ = 6;
                *p++ = 255;
                udp.Bind();
                udp.Send(buffer, (int)(p - buffer));
            }

            IPAddress OfferedIP = default;
            IPAddress OfferedGateway = default;
            IPAddress OfferedSubnetMask = default;
            IPAddress ServerID = default;

            {
                byte[] data = null;
                while ((data = udp.Receive()) == null) ACPITimer.Sleep(10);

                fixed (byte* pdata = data)
                {
                    DHCPHeader* dhcp_hdr = (DHCPHeader*)pdata;
                    byte* ptr = pdata + sizeof(DHCPHeader);
                    ptr += 4;
                    for (; ; )
                    {
                        if (*ptr == 0xff) break;
                        if (*ptr == 0) break;

                        byte code = *ptr++;
                        byte length = *ptr++;
                        switch (code)
                        {
                            case 53:
                                {
                                    if (*ptr != 2)
                                    {
                                        udp.Remove();
                                        return false;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    OfferedGateway = *(IPAddress*)ptr;
                                }
                                break;
                            case 54:
                                {
                                    ServerID = *(IPAddress*)ptr;
                                }
                                break;
                            case 1:
                                {
                                    OfferedSubnetMask = *(IPAddress*)ptr;
                                }
                                break;
                        }
                        ptr += length;
                    }
                    OfferedIP = dhcp_hdr->YourIP;
                }
            }

            {
                byte* p = buffer;
                p += MakeDHCPRequestPacket(p, xid, 3);

                *p++ = 54;
                *p++ = (byte)sizeof(IPAddress);
                *(IPAddress*)p = ServerID;
                p += sizeof(IPAddress);

                *p++ = 50;
                *p++ = (byte)sizeof(IPAddress);
                *(IPAddress*)p = OfferedIP;
                p += sizeof(IPAddress);

                *p++ = 55;
                *p++ = 3;
                *p++ = 1;
                *p++ = 3;
                *p++ = 6;
                *p++ = 255;
                udp.Bind();
                udp.Send(buffer, (int)(p - buffer));
            }

            {
                byte[] data = null;
                while ((data = udp.Receive()) == null) ACPITimer.Sleep(10);

                fixed (byte* pdata = data)
                {
                    byte* ptr = pdata + sizeof(DHCPHeader);
                    ptr += 4;
                    for (; ; )
                    {
                        if (*ptr == 0xff) break;
                        if (*ptr == 0) break;

                        byte code = *ptr++;
                        byte length = *ptr++;
                        switch (code)
                        {
                            case 53:
                                {
                                    if (*ptr != 5)
                                    {
                                        udp.Remove();
                                        return false;
                                    }
                                }
                                break;
                        }
                        ptr += length;
                    }
                }
            }

            NETv4.GatewayIP = OfferedGateway;
            NETv4.IP = OfferedIP;
            NETv4.Mask = OfferedSubnetMask;

            Console.Write($"[DHCP] configured. IP:{IP.P1}.{IP.P2}.{IP.P3}.{IP.P4}\n");

            udp.Remove();

            return true;
        }

        public static int MakeDHCPRequestPacket(byte* buffer,uint xid,byte type) 
        {
            DHCPHeader* dhcp_hdr = (DHCPHeader*)buffer;
            Allocator.ZeroFill((IntPtr)dhcp_hdr, (ulong)sizeof(DHCPHeader));
            dhcp_hdr->MessageType = 1;
            dhcp_hdr->HardwareType = 1;
            dhcp_hdr->HardwareLength = (byte)sizeof(MACAddress);
            dhcp_hdr->ID = xid;
            dhcp_hdr->ClientMAC = MAC;
            SwapLeftRight(ref dhcp_hdr->ID);
            byte* p = buffer + sizeof(DHCPHeader);
            *(uint*)p = SwapLeftRight(0x63825363);
            p += 4;
            *p++ = 53;
            *p++ = 1;
            *p++ = type;
            return (int)(p - buffer);
        }
        #endregion

        #region UDP
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UDPHeader
        {
            public ushort SrcPort;
            public ushort DestPort;
            public ushort Length;
            public ushort Checksum;
        }

        public static List<UDPClient> UDPClients;

        public class UDPClient
        {
            public ushort Port;
            public ushort LocalPort;
            public IPAddress Dest;

            internal Queue<byte[]> DataReceived;

            public UDPClient(IPAddress address, ushort port, ushort localPort)
            {
                this.Port = port;
                this.Dest = address;
                this.LocalPort = localPort;
                DataReceived = new Queue<byte[]>();
            }

            public byte[] Receive()
            {
                if (DataReceived.Count > 0) return DataReceived.Dequeue();
                return null;
            }

            public void Bind() => UDPBind(this);

            public void Remove() => UDPRemove(this);

            public void Send(byte* buffer, int length) => UDPSend(this, buffer, length);
        }

        public static void UDPRemove(UDPClient client)
        {
            NETv4.UDPClients.Remove(client);
        }

        public static void UDPBind(UDPClient client)
        {
            if(NETv4.UDPClients.IndexOf(client) == -1)
            {
                NETv4.UDPClients.Add(client);
            }
        }

        public static void UDPSend(UDPClient client,byte* buffer,int length)
        {
            IPAddress ip_boardcast = new IPAddress(0xFF, 0xFF, 0xFF, 0xFF);
            MACAddress mac_boardcast = new MACAddress(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);
            byte* ptr = stackalloc byte[length + sizeof(UDPHeader) + sizeof(IPv4Header) + sizeof(EthernetHeader)];
            byte* p = ptr;
            MakeEthernetPacket(p, client.Dest == ip_boardcast ? mac_boardcast : ARPLookup(client.Dest,true), 0x0800);
            p += sizeof(EthernetHeader);
            byte* ip_p = p;
            p += sizeof(IPv4Header);
            p += MakeUDPPacket(p, client, buffer, length);
            MakeIPPacket(ip_p, client.Dest, 17, length + sizeof(UDPHeader));
            if (Sender != null)
            {
                Sender(ptr, length + sizeof(UDPHeader) + sizeof(IPv4Header) + sizeof(EthernetHeader));
            }
        }

        public static int MakeUDPPacket(byte* buffer, UDPClient client, byte* data, int length)
        {
            UDPHeader* udp_hdr = (UDPHeader*)buffer;
            udp_hdr->DestPort = client.Port;
            udp_hdr->SrcPort = client.LocalPort;
            udp_hdr->Length = (ushort)(length + sizeof(UDPHeader));
            udp_hdr->Checksum = 0;
            SwapLeftRight(ref udp_hdr->DestPort);
            SwapLeftRight(ref udp_hdr->SrcPort);
            SwapLeftRight(ref udp_hdr->Length);
            byte* p = buffer + sizeof(UDPHeader);
            Allocator.MemoryCopy((IntPtr)p, (IntPtr)data, (ulong)length);
            p += length;
            PseudoIPv4Header* phdr = (PseudoIPv4Header*)(buffer - sizeof(PseudoIPv4Header));
            phdr->Source = NETv4.IP;
            phdr->Dest = client.Dest;
            phdr->Reserved = 0;
            phdr->Protocol = 17;
            phdr->Length = ((ushort)((uint)p - (uint)buffer));
            phdr->Bits1 = 0;
            phdr->Bits2 = 0;
            SwapLeftRight(ref phdr->Length);
            ushort checksum = IPChecksum(buffer - sizeof(PseudoIPv4Header), (int)(p - (buffer - sizeof(PseudoIPv4Header))));
            udp_hdr->Checksum = checksum;
            return (int)(p - buffer);
        }

        public static void UDPOnData(IPv4Header* ipv4_hdr,byte* buffer)
        {
            UDPHeader* udp_hdr = (UDPHeader*)buffer;
            buffer += sizeof(UDPHeader);
            int length = ipv4_hdr->TotalLength - sizeof(IPv4Header) - sizeof(UDPHeader);
            SwapLeftRight(ref udp_hdr->SrcPort);
            SwapLeftRight(ref udp_hdr->DestPort);
            SwapLeftRight(ref udp_hdr->Length);

            UDPClient client = null;
            for(int i = 0; i < UDPClients.Count; i++)
            {
                if (udp_hdr->DestPort == UDPClients[i].LocalPort)
                {
                    client = UDPClients[i];
                }
            }
            if(client != null)
            {
                byte[] buf = new byte[length];
                for (int i = 0; i < length; i++) buf[i] = buffer[i];
                client.DataReceived.Enqueue(buf);
            }
        }
        #endregion

        #region TCP
        public enum TCPStatus
        {
            Closed,
            Listen,
            SynSent,
            SynReceived,
            Established,
            FinWait1,
            FinWait2,
            CloseWait,
            Closing,
            LastAcknowledge,
            TimeWait,
        }

        public enum TCPOption
        {
            End,
            Nop,
            MSS,
            WS,
            SACK
        }

        [Flags]
        public enum TCPFlags : byte
        {
            TCP_FIN = (1 << 0),
            TCP_SYN = (1 << 1),
            TCP_RST = (1 << 2),
            TCP_PSH = (1 << 3),
            TCP_ACK = (1 << 4),
            TCP_URG = (1 << 5),
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TCPHeader
        {
            public ushort SourcePort;
            public ushort DestPort;
            public uint Seq;
            public uint Ack;
            public byte Off;
            public TCPFlags Flags;
            public ushort WindowSize;
            public ushort Checksum;
            public ushort Urgent;
        }

        public const ushort MSS = 536;
        private const ushort TCP_WINDOW_SIZE = 8192;

        public class TCPClient
        {
            public TCPStatus Status;

            public uint SndUna;
            public uint SndNxt;
            public ushort SndWnd;
            public uint SndUp;
            public uint SndWl1;
            public uint SndWl2;
            public uint ISS;
            public uint RcvNxt;
            public uint RcvWnd;
            public uint RcvUp;
            public uint IRS;

            public IPAddress Dest;
            public ushort Port;

            public ushort LocalPort;

            private byte[] DataCached;
            private Queue<byte[]> DataReceived;

            internal bool IsDestAcknowledged;

            public TCPClient(IPAddress address, ushort port,ushort localPort)
            {
                this.Port = port;
                this.Dest = address;
                this.LocalPort = localPort;
                DataCached = new byte[0];
                DataReceived = new Queue<byte[]>();
            }

            public byte[] Receive()
            {
                if (DataReceived.Count > 0) return DataReceived.Dequeue();
                return null;
            }

            public void Connect() => TCPConnect(this);

            public void Send(byte* buffer, int length) => TCPSendData(this, buffer, length);

            public void Close() => TCPClose(this);

            public void Remove() => TCPRemove(this);

            internal void CacheConcat(byte* buffer, int length)
            {
                byte[] c = new byte[this.DataCached.Length + length];
                int i = 0;
                for (int x = 0; x < this.DataCached.Length; x++, i++) c[i] = this.DataCached[x];
                for (int x = 0; x < length; x++, i++) c[i] = buffer[x];
                this.DataCached.Dispose();
                this.DataCached = c;
            }

            internal void CacheFlush()
            {
                DataReceived.Enqueue(this.DataCached);
                this.DataCached = new byte[0];
            }
        }

        public class TCPListener : TCPClient
        {
            public TCPListener(ushort port) : base(default, 0, port)
            {
            }

            public void Listen() => TCPListen(this);
        }

        private static List<TCPClient> TCPClients;

        public static void TCPOnData(IPv4Header* ipv4_hdr, byte* buffer)
        {
            TCPHeader* tcp_hdr = (TCPHeader*)buffer;
            buffer += tcp_hdr->Off >> 2;
            int length = ipv4_hdr->TotalLength - sizeof(IPv4Header) - (tcp_hdr->Off >> 2);

            SwapLeftRight(ref tcp_hdr->SourcePort);
            SwapLeftRight(ref tcp_hdr->DestPort);
            SwapLeftRight(ref tcp_hdr->Seq);
            SwapLeftRight(ref tcp_hdr->Ack);
            SwapLeftRight(ref tcp_hdr->WindowSize);
            SwapLeftRight(ref tcp_hdr->Urgent);

            TCPClient client = null;
            for(int i = 0; i < TCPClients.Count; i++)
            {
                if (tcp_hdr->DestPort == TCPClients[i].LocalPort)
                {
                    client = TCPClients[i];
                }
            }
            if(client != null)
            {
                //Just ack
                if(tcp_hdr->Flags == TCPFlags.TCP_ACK)
                {
                    client.IsDestAcknowledged = true;
                }
                switch (client.Status)
                {
                    case TCPStatus.SynSent:
                        {
                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_SYN))
                            {
                                client.IRS = tcp_hdr->Seq;
                                client.RcvNxt = tcp_hdr->Seq + 1;
                                if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_ACK))
                                {
                                    client.SndUna = tcp_hdr->Ack;
                                    client.SndWnd = tcp_hdr->WindowSize;
                                    client.SndWl1 = tcp_hdr->Seq;
                                    client.SndWl2 = tcp_hdr->Ack;

                                    client.Status = TCPStatus.Established;
                                    TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                    Console.Write("Connection Established\n");
                                }
                                else
                                {
                                    client.Status = TCPStatus.Closed;
                                }
                            }
                            else if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_ACK))
                            {
                                if (tcp_hdr->Ack - client.ISS < 0 || tcp_hdr->Ack - client.SndNxt > 0)
                                {
                                    TCPSend(client, tcp_hdr->Ack, TCPFlags.TCP_RST, null, 0);
                                }
                                else
                                {
                                    client.RcvNxt = tcp_hdr->Seq;
                                    client.SndNxt = tcp_hdr->Ack;

                                    client.Status = TCPStatus.Established;

                                    Console.Write("Connection Established\n");
                                }
                            }
                            else
                            {
                                client.Status = TCPStatus.Closed;
                            }
                        }
                        break;
                    case TCPStatus.Listen:
                        {
                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_RST))
                            {
                                return;
                            }
                            else if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_FIN))
                            {
                                client.Status = TCPStatus.Closed;
                            }
                            else if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_ACK))
                            {
                                client.RcvNxt = tcp_hdr->Seq;
                                client.SndNxt = tcp_hdr->Ack;
                                client.Status = TCPStatus.Established;
                            }
                            else if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_SYN))
                            {
                                client.Port = tcp_hdr->SourcePort;
                                client.Dest = ipv4_hdr->SourceIP;

                                uint seq = 123456;

                                client.SndUna = seq;
                                client.SndNxt = seq;
                                client.SndWnd = TCP_WINDOW_SIZE;
                                client.SndWl1 = tcp_hdr->Seq - 1;
                                client.ISS = seq;
                                client.RcvNxt = tcp_hdr->Seq + 1;
                                client.RcvWnd = TCP_WINDOW_SIZE;
                                client.IRS = tcp_hdr->Seq;

                                TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK | TCPFlags.TCP_SYN, null, 0);

                                client.Status = TCPStatus.SynReceived;
                            }
                        }
                        break;
                    default:
                        {
                            if ((client.RcvNxt <= tcp_hdr->Seq && tcp_hdr->Seq + length < client.RcvNxt + client.RcvWnd))
                            {
                                switch (client.Status)
                                {
                                    case TCPStatus.SynReceived:
                                        {
                                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_ACK))
                                            {
                                                if (client.SndUna <= tcp_hdr->Ack && tcp_hdr->Ack <= client.SndNxt)
                                                {
                                                    client.SndWnd = tcp_hdr->WindowSize;
                                                    client.SndWl1 = tcp_hdr->Seq;
                                                    client.SndWl2 = tcp_hdr->Seq;

                                                    client.Status = TCPStatus.Established;
                                                    Console.Write("Client accepted.\n");
                                                }
                                                else
                                                {
                                                    TCPSend(client, client.SndNxt, TCPFlags.TCP_RST, null, 0);
                                                }
                                            }
                                        }
                                        break;
                                    case TCPStatus.Established:
                                        {
                                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_ACK))
                                            {
                                                if (client.SndUna < tcp_hdr->Ack && tcp_hdr->Ack <= client.SndNxt)
                                                {
                                                    client.SndUna = tcp_hdr->Ack;
                                                    if (client.SndWl1 < tcp_hdr->Seq || (client.SndWl1 == tcp_hdr->Seq && client.SndWl2 <= tcp_hdr->Ack))
                                                    {
                                                        client.SndWnd = tcp_hdr->WindowSize;
                                                        client.SndWl1 = tcp_hdr->Seq;
                                                        client.SndWl2 = tcp_hdr->Ack;
                                                    }
                                                }
                                                if (tcp_hdr->Ack < client.SndUna)
                                                {
                                                    return;
                                                }
                                                if (tcp_hdr->Ack > client.SndNxt)
                                                {
                                                    TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                                }
                                                if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_PSH))
                                                {
                                                    client.CacheConcat(buffer, length);
                                                    client.CacheFlush();
                                                    client.RcvNxt += (uint)length;
                                                    TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                                }
                                                else if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_FIN))
                                                {
                                                    client.RcvNxt++;
                                                    TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                                    client.Status = TCPStatus.TimeWait;
                                                    client.Status = TCPStatus.Closed;
                                                }
                                                if (length > 0 && tcp_hdr->Seq >= client.RcvNxt)
                                                {
                                                    client.CacheConcat(buffer, length);
                                                    client.RcvNxt += (uint)length;
                                                    TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                                }
                                            }
                                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_RST))
                                            {
                                                client.Status = TCPStatus.Closed;
                                            }
                                            else if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_FIN))
                                            {
                                                client.RcvNxt++;
                                                TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                                client.Status = TCPStatus.CloseWait;
                                                TCPSend(client, client.SndNxt, TCPFlags.TCP_FIN, null, 0);
                                                client.Status = TCPStatus.LastAcknowledge;
                                            }
                                        }
                                        break;
                                    case TCPStatus.FinWait1:
                                        {
                                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_ACK))
                                            {
                                                if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_FIN))
                                                {
                                                    client.RcvNxt++;
                                                    TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                                    client.Status = TCPStatus.TimeWait;
                                                    client.Status = TCPStatus.Closed;
                                                }
                                                else
                                                {
                                                    client.Status = TCPStatus.FinWait2;
                                                }
                                            }
                                            else if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_FIN))
                                            {
                                                client.RcvNxt++;
                                                TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                                client.Status = TCPStatus.Closing;
                                            }
                                        }
                                        break;
                                    case TCPStatus.FinWait2:
                                        {
                                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_FIN))
                                            {
                                                client.RcvNxt++;
                                                TCPSend(client, client.SndNxt, TCPFlags.TCP_ACK, null, 0);
                                                client.Status = TCPStatus.TimeWait;
                                                client.Status = TCPStatus.Closed;
                                            }
                                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_RST))
                                            {
                                                client.Status = TCPStatus.Closed;
                                            }
                                        }
                                        break;
                                    case TCPStatus.Closing:
                                        {
                                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_ACK))
                                            {
                                                client.Status = TCPStatus.CloseWait;
                                                client.Status = TCPStatus.Closed;
                                            }
                                        }
                                        break;
                                    case TCPStatus.CloseWait:
                                    case TCPStatus.LastAcknowledge:
                                        {
                                            if (tcp_hdr->Flags.HasFlag(TCPFlags.TCP_ACK))
                                            {
                                                client.Status = TCPStatus.Closed;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
        }

        public static void TCPConnect(TCPClient client)
        {
            uint seq = 54188;

            client.SndUna = seq;
            client.SndNxt = seq;
            client.SndWnd = TCP_WINDOW_SIZE;
            client.RcvWnd = TCP_WINDOW_SIZE;
            client.ISS = seq;

            client.Status = TCPStatus.SynSent;

            if (NETv4.TCPClients.IndexOf(client) == -1)
            {
                NETv4.TCPClients.Add(client);
            }

            TCPSend(client, client.SndNxt, TCPFlags.TCP_SYN, null, 0);
        }

        public static void TCPClose(TCPClient client)
        {
            if (client.Status == TCPStatus.Established)
            {
                TCPSend(client, client.SndNxt, TCPFlags.TCP_FIN | TCPFlags.TCP_ACK, null, 0);
                client.SndNxt++;
                client.Status = TCPStatus.FinWait1;
            }
        }

        public static void TCPRemove(TCPClient client)
        {
            NETv4.TCPClients.Remove(client);
        }

        public static void TCPListen(TCPListener listener)
        {
            listener.Status = TCPStatus.Listen;

            if (NETv4.TCPClients.IndexOf(listener) == -1)
            {
                NETv4.TCPClients.Add(listener);
            }
        }

        public static void TCPSendData(TCPClient client, byte* data, int length)
        {
            if (client.Status == TCPStatus.Established)
            {
                for (int i = 0; i < length; i += MSS)
                {
                    int count = i + MSS > length ? length % MSS : MSS;

                    TCPSend(client, client.SndNxt, count < MSS ? (TCPFlags.TCP_ACK | TCPFlags.TCP_PSH) : (TCPFlags.TCP_ACK), data, count);
                    client.SndNxt += (uint)count;
                }
            }
        }

        public static void TCPSend(TCPClient client, uint seq, TCPFlags flags, void* data, int length)
        {
            int totalLength = 0;

            byte* buffer = stackalloc byte[1024];
            byte* ptr = buffer;
            MakeEthernetPacket(ptr, ARPLookup(client.Dest,true), 0x0800);
            ptr += sizeof(EthernetHeader);
            byte* ip_ptr = ptr;
            ptr += sizeof(IPv4Header);
            totalLength += MakeTCPPacket(ptr, client.LocalPort, client.Port, seq, client.RcvNxt, flags, client.SndWnd, client.Dest, data, length);
            MakeIPPacket(ip_ptr, client.Dest, 6, totalLength);
            totalLength += sizeof(IPv4Header);
            totalLength += sizeof(EthernetHeader);

            if (flags.HasFlag(TCPFlags.TCP_ACK) && flags != TCPFlags.TCP_ACK)
            {
                client.IsDestAcknowledged = false;
            }

            if (Sender != null)
            {
                Sender(buffer, totalLength);
            }

            if (flags.HasFlag(TCPFlags.TCP_SYN) || flags.HasFlag(TCPFlags.TCP_FIN))
            {
                client.SndNxt++;
            }
        }

        //Make sure there is sizeof(PseudoIPv4Header) bytes available before the address of buffer
        internal static int MakeTCPPacket
            (byte* buffer,
            ushort localPort, ushort remotePort,
            uint seq, uint ack, TCPFlags flags, ushort windowSize,
            IPAddress dest,
            void* data, int length)
        {
            TCPHeader* tcp_hdr = (TCPHeader*)buffer;
            tcp_hdr->SourcePort = localPort;
            tcp_hdr->DestPort = remotePort;
            tcp_hdr->Seq = seq;
            tcp_hdr->Ack = ack;
            tcp_hdr->WindowSize = windowSize;
            tcp_hdr->Checksum = 0;
            tcp_hdr->Urgent = 0;
            tcp_hdr->Flags = flags;
            tcp_hdr->Off = 0;
            SwapLeftRight(ref tcp_hdr->SourcePort);
            SwapLeftRight(ref tcp_hdr->DestPort);
            SwapLeftRight(ref tcp_hdr->Seq);
            SwapLeftRight(ref tcp_hdr->Ack);
            SwapLeftRight(ref tcp_hdr->WindowSize);
            SwapLeftRight(ref tcp_hdr->Urgent);
            byte* p = buffer + sizeof(TCPHeader);
            if (flags.HasFlag(TCPFlags.TCP_SYN))
            {
                p[0] = (byte)TCPOption.MSS;
                p[1] = 4;
                *(ushort*)(p + 2) = SwapLeftRight(MSS);
                p += p[1];
            }
            while (((p - buffer) & 3) != 0)
            {
                *p++ = 0;
            }
            tcp_hdr->Off = (byte)((p - buffer) << 2);
            if (data != null)
            {
                Allocator.MemoryCopy((IntPtr)p, (IntPtr)data, (ulong)length);
            }
            p += length;
            PseudoIPv4Header* phdr = (PseudoIPv4Header*)(buffer - sizeof(PseudoIPv4Header));
            phdr->Source = NETv4.IP;
            phdr->Dest = dest;
            phdr->Reserved = 0;
            phdr->Protocol = 6;
            phdr->Length = ((ushort)((uint)p - (uint)buffer));
            phdr->Bits1 = 0;
            phdr->Bits2 = 0;
            SwapLeftRight(ref phdr->Length);
            ushort checksum = IPChecksum(buffer - sizeof(PseudoIPv4Header), (int)(p - (buffer - sizeof(PseudoIPv4Header))));
            tcp_hdr->Checksum = checksum;
            return (int)p - (int)buffer;
        }
        #endregion

        #region IP
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct IPv4Header
        {
            public byte VersionAndIHL;
            public byte DSCPAndECN;
            public ushort TotalLength;
            public ushort ID;
            public ushort FlagAndFragmentOffset;
            public byte TimeToLive;
            public byte Protocol;
            public ushort HeaderChecksum;
            public IPAddress SourceIP;
            public IPAddress DestIP;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct PseudoIPv4Header
        {
            public IPAddress Source;
            public uint Bits1;
            public IPAddress Dest;
            public uint Bits2;
            public byte Reserved;
            public byte Protocol;
            public ushort Length;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IPAddress
        {
            internal byte P1;
            internal byte P2;
            internal byte P3;
            internal byte P4;

            public IPAddress(byte p1, byte p2, byte p3, byte p4)
            {
                this.P1 = p1;
                this.P2 = p2;
                this.P3 = p3;
                this.P4 = p4;
            }

            public static bool operator ==(IPAddress a, IPAddress b)
            {
                return
                    a.P1 == b.P1 &&
                    a.P2 == b.P2 &&
                    a.P3 == b.P3 &&
                    a.P4 == b.P4;
            }

            public static bool operator !=(IPAddress a, IPAddress b)
            {
                return !(a == b);
            }

            public bool IsInSameSubnet(IPAddress dest,IPAddress mask)
            {
                if(
                    (dest.P1 & mask.P1) == (this.P1 & mask.P1) &&
                    (dest.P2 & mask.P2) == (this.P2 & mask.P2) &&
                    (dest.P3 & mask.P3) == (this.P3 & mask.P3) &&
                    (dest.P4 & mask.P4) == (this.P4 & mask.P4)
                    )
                {
                    return true;
                }
                return false;
            }
        }

        public static void IPOnData(byte* buffer)
        {
            IPv4Header* hdr = (IPv4Header*)buffer;
            buffer += sizeof(IPv4Header);
            SwapLeftRight(ref hdr->TotalLength);

            if (hdr->DestIP == NETv4.IP)
            {
                switch (hdr->Protocol)
                {
                    case 17: UDPOnData(hdr, buffer); break;
                    case 6: TCPOnData(hdr, buffer); break;
                    case 1: ICMPOnData(hdr, buffer); break;
                }
            }
            //For DHCP
            else if(NETv4.IP == default && hdr->Protocol == 17)
            {
                UDPOnData(hdr, buffer);
            }
        }

        public static void MakeIPPacket(byte* buffer, IPAddress dest, byte protocol, int overloadLength)
        {
            IPv4Header* ip_hdr = (IPv4Header*)buffer;
            ip_hdr->VersionAndIHL = 0x45;
            ip_hdr->TotalLength = (ushort)(sizeof(IPv4Header) + overloadLength);
            ip_hdr->TimeToLive = 188; //Sounds like my name in Chinese
            ip_hdr->Protocol = protocol;
            ip_hdr->SourceIP = NETv4.IP;
            ip_hdr->DestIP = dest;
            ip_hdr->HeaderChecksum = 0;
            ip_hdr->DSCPAndECN = 0;
            ip_hdr->ID = 0;
            ip_hdr->FlagAndFragmentOffset = 0;
            SwapLeftRight(ref ip_hdr->TotalLength);
            ip_hdr->HeaderChecksum = IPChecksum((byte*)ip_hdr, sizeof(IPv4Header));
        }

        public static ushort IPChecksum(byte* addr, int count)
        {
            uint sum = 0;
            ushort* ptr = (ushort*)addr;

            while (count > 1)
            {
                sum += *ptr++;
                count -= 2;
            }

            if (count > 0)
            {
                sum += *(byte*)ptr;
            }

            while ((sum >> 16) != 0)
            {
                sum = (sum & 0xffff) + (sum >> 16);
            }

            return (ushort)~sum;
        }
        #endregion

        #region ICMP
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ICMPHeader
        {
            public byte Type;
            public byte Code;
            public ushort Checksum;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ICMPEchoHeader
        {
            public ushort ID;
            public ushort Seq;
        };

        public static bool IsICMPRespond;
        public static int ICMPReplyTTL;
        public static int ICMPReplyBytes;

        public const int ICMPPingBytes = 7;

        public static void ICMPOnData(IPv4Header* ip_hdr, byte* buffer)
        {
            ICMPHeader* icmp_hdr = (ICMPHeader*)buffer;
            if (icmp_hdr->Type == 8)
            {
                icmp_hdr->Type = 0;
                icmp_hdr->Checksum = 0;
                int length = ip_hdr->TotalLength - sizeof(IPv4Header);
                icmp_hdr->Checksum = IPChecksum(buffer, length);
                byte* ptr = stackalloc byte[length + sizeof(IPv4Header) + sizeof(EthernetHeader)];
                byte* p = ptr;
                MakeEthernetPacket(p, ARPLookup(ip_hdr->SourceIP,true), 0x0800);
                p += sizeof(EthernetHeader);
                MakeIPPacket(p, ip_hdr->SourceIP, 1, length);
                p += sizeof(IPv4Header);
                Allocator.MemoryCopy((IntPtr)p, (IntPtr)buffer, (ulong)length);
                if (Sender != null)
                {
                    Sender(ptr, length + sizeof(IPv4Header) + sizeof(EthernetHeader));
                }
            }
            else if (icmp_hdr->Type == 0)
            {
                IsICMPRespond = true;
                ICMPReplyTTL = ip_hdr->TimeToLive;
                ICMPReplyBytes = ip_hdr->TotalLength - sizeof(IPv4Header) - sizeof(ICMPHeader) - sizeof(ICMPEchoHeader);
            }
        }

        public static void ICMPPing(IPAddress dest)
        {
            byte* ptr = stackalloc byte[ICMPPingBytes + sizeof(ICMPHeader) + sizeof(ICMPEchoHeader) + sizeof(IPv4Header) + sizeof(EthernetHeader)];
            byte* p = ptr;
            MakeEthernetPacket(p, ARPLookup(dest,true), 0x0800);
            p += sizeof(EthernetHeader);
            MakeIPPacket(p, dest, 1, ICMPPingBytes + sizeof(ICMPHeader) + sizeof(ICMPEchoHeader));
            p += sizeof(IPv4Header);
            ICMPHeader* icmp_hdr = (ICMPHeader*)p;
            icmp_hdr->Type = 8;
            icmp_hdr->Code = 0;
            icmp_hdr->Checksum = 0;
            p += sizeof(ICMPHeader);
            ICMPEchoHeader* echo_hdr = (ICMPEchoHeader*)p;
            echo_hdr->ID = 1;
            echo_hdr->Seq = 1234;
            SwapLeftRight(echo_hdr->ID);
            SwapLeftRight(echo_hdr->Seq);
            p += sizeof(ICMPEchoHeader);

            *p++ = (byte)'n';
            *p++ = (byte)'i';
            *p++ = (byte)'f';
            *p++ = (byte)'a';
            *p++ = (byte)'n';
            *p++ = (byte)'f';
            *p++ = (byte)'a';

            icmp_hdr->Checksum = IPChecksum((byte*)icmp_hdr, ICMPPingBytes + sizeof(ICMPHeader) + sizeof(ICMPEchoHeader));

            IsICMPRespond = false;
            ICMPReplyTTL = 0;
            ICMPReplyBytes = 0;

            if (Sender != null)
            {
                Sender(ptr, ICMPPingBytes + sizeof(ICMPHeader) + sizeof(ICMPEchoHeader) + sizeof(IPv4Header) + sizeof(EthernetHeader));
            }
        }
        #endregion

        #region ARP
        public const int MaximumARPRequireTimes = 4;

        public static MACAddress ARPLookup(IPAddress iPAddress, bool requireIfNotExist = false)
        {
            if (!NETv4.IP.IsInSameSubnet(iPAddress,NETv4.Mask))
            {
                iPAddress = GatewayIP;
            }
            int times = 0;
        Retry:;
            for (int i = 0; i < ARPTable.Count; i++)
            {
                if (ARPTable[i].IP == iPAddress) return ARPTable[i].MAC;
            }
            NETv4.ARPRequire(iPAddress);
            ACPITimer.Sleep(10);
            times++;
            if (requireIfNotExist && times <= MaximumARPRequireTimes) goto Retry;
            return default;
        }

        public static void ARPOnData(ARPHeader* arp_hdr)
        {
            SwapLeftRight(ref arp_hdr->Operation);
            if (arp_hdr->Operation == 2)
            {
                if (ARPLookup(arp_hdr->SourceIP) == default)
                {
                    ARPTable.Add(new ARPCache() { MAC = arp_hdr->SourceMAC, IP = arp_hdr->SourceIP });
                }
            }
            else if (arp_hdr->Operation == 1 && arp_hdr->DestIP == NETv4.IP)
            {
                byte* ptr = stackalloc byte[sizeof(ARPHeader) + sizeof(EthernetHeader)];
                byte* buffer = ptr;
                MakeEthernetPacket(
                    buffer,
                    arp_hdr->SourceMAC,
                    0x0806);
                buffer += sizeof(EthernetHeader);
                MakeARPPacket(
                    buffer,
                    arp_hdr->SourceMAC,
                    arp_hdr->SourceIP,
                    2);
                if (Sender != null)
                {
                    Sender(ptr, sizeof(ARPHeader) + sizeof(EthernetHeader));
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ARPCache
        {
            public IPAddress IP;
            public MACAddress MAC;
        }

        public static List<ARPCache> ARPTable;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ARPHeader
        {
            public ushort HardwareType;
            public ushort Protocol;
            public byte HardwareAddrLen;
            public byte ProtocolAddrLen;
            public ushort Operation;
            public MACAddress SourceMAC;
            public IPAddress SourceIP;
            public MACAddress DestMAC;
            public IPAddress DestIP;
        }

        public static void MakeARPPacket(byte* buffer, MACAddress destMAC, IPAddress destIP, ushort opcode)
        {
            ARPHeader* arp_hdr = (ARPHeader*)buffer;
            arp_hdr->SourceMAC = NETv4.MAC;
            arp_hdr->DestMAC = destMAC;
            arp_hdr->SourceIP = NETv4.IP;
            arp_hdr->DestIP = destIP;
            arp_hdr->Operation = opcode;
            arp_hdr->HardwareAddrLen = 6;
            arp_hdr->ProtocolAddrLen = 4;
            arp_hdr->HardwareType = 1;
            arp_hdr->Protocol = 0x0800;
            SwapLeftRight(ref arp_hdr->Operation);
            SwapLeftRight(ref arp_hdr->HardwareType);
            SwapLeftRight(ref arp_hdr->Protocol);
        }

        public static void ARPRequire(IPAddress iP)
        {
            byte* ptr = stackalloc byte[sizeof(ARPHeader) + sizeof(EthernetHeader)];
            byte* buffer = ptr;
            MakeEthernetPacket(
                buffer,
                new MACAddress(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF),
                0x0806);
            buffer += sizeof(EthernetHeader);
            MakeARPPacket(
                buffer,
                new MACAddress(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF),
                iP,
                1);
            if (Sender != null)
            {
                Sender(ptr, sizeof(ARPHeader) + sizeof(EthernetHeader));
            }
        }
        #endregion

        #region Ethernet
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct EthernetHeader
        {
            public MACAddress DestMAC;
            public MACAddress SrcMAC;
            public ushort EthernetType;
        }

        public static void MakeEthernetPacket(byte* buffer, MACAddress dest, ushort type)
        {
            EthernetHeader* eth_hdr = (EthernetHeader*)buffer;
            eth_hdr->DestMAC = dest;
            eth_hdr->SrcMAC = NETv4.MAC;
            eth_hdr->EthernetType = type;
            SwapLeftRight(ref eth_hdr->EthernetType);
        }
        #endregion
    }
}
