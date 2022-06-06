/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
//https://github.com/pdoane/osdev/blob/master/net/tcp.c

using MOOS.Driver;
using System.Net;
using System.Runtime.InteropServices;

namespace MOOS.NET
{
    public unsafe class TcpClient
    {
        public TCPStatus State;

        public bool Connected => State != TCPStatus.SynSent;

        public byte[] localAddr = new byte[4];
        public byte[] nextAddr = new byte[4];
        public byte[] remoteAddr = new byte[4];

        public ushort localPort;
        public ushort remotePort;

        // send state
        public uint sndUna;                         // send unacknowledged
        public uint sndNxt;                         // send next
        public uint sndWnd;                         // send window
        public uint sndUP;                          // send urgent pointer
        public uint sndWl1;                         // segment sequence number used for last window update
        public uint sndWl2;                         // segment acknowledgment number used for last window update
        public uint iss;                            // initial send sequence number

        // receive state
        public uint rcvNxt;                        // receive next
        public uint rcvWnd;                        // receive window
        public uint rcvUP;                         // receive urgent pointer

        public void Send(byte[] buffer)
        {
            if (Connected)
            {
                fixed (byte* p = buffer)
                    TCP.Send(this, p, buffer.Length);
            }
            else
            {
                Console.WriteLine("Connection havn't established yet");
            }
        }

        public TcpClient()
        {
            OnData = null;
        }
        
        public static TcpClient Connect(IPAddress address,ushort port) 
        {
            return TCP.Connect(address.Address, port, port);
        }

        public event Network.OnDataHandler OnData;

        internal unsafe void _OnData(byte* buffer, int length)
        {
            byte[] Data = new byte[length];
            fixed (byte* dest = Data)
            {
                Native.Movsb(dest, buffer, (ulong)length);
            }
            OnData?.Invoke(Data);
            Data.Dispose();
        }
    }

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
        MSS
    }

    public enum TCPFlags
    {
        TCP_FIN = (1 << 0),
        TCP_SYN = (1 << 1),
        TCP_RST = (1 << 2),
        TCP_PSH = (1 << 3),
        TCP_ACK = (1 << 4),
        TCP_URG = (1 << 5),
    }

    /// <summary>
    /// Incompleted TCP
    /// </summary>
    internal unsafe class TCP
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct TCPHeader
        {
            public ushort srcPort;
            public ushort dstPort;
            public uint seq;
            public uint ack;
            public byte off;
            public byte flags;
            public ushort windowSize;
            public ushort checksum;
            public ushort urgent;
        }

        private const ushort TCP_WINDOW_SIZE = 8192;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ChecksumHeader
        {
            public fixed byte src[4];
            public uint bits1;
            public fixed byte dst[4];
            public uint bits2;
            public byte reserved;
            public byte protocol;
            public ushort len;
        }

        internal static void HandlePacket(byte* buffer, int length)
        {
            TCPHeader* hdr = (TCPHeader*)buffer;
            buffer += hdr->off >> 2;
            length -= hdr->off >> 2;
            length -= 4;

            Swap(hdr);

            if (currConn == null)
            {
                Console.WriteLine("TCP connection havn't established yet");
                return;
            }

            if (hdr->dstPort != hdr->srcPort)
            {
                return;
            }

            if (currConn.State == TCPStatus.Closed)
            {
                //TO-DO send close ack
                return;
            }

            if (currConn.State == TCPStatus.Listen)
            {
            }
            else if (currConn.State == TCPStatus.SynSent)
            {
                RecvSynSent(currConn, hdr);
            }
            else
            {
                RecvGeneral(currConn, hdr, buffer, length);
            }
        }

        private static void RecvGeneral(TcpClient conn, TCPHeader* hdr, byte* buffer, int length)
        {
            // Process segments not in the CLOSED, LISTEN, or SYN-SENT states.

            uint flags = hdr->flags;

            // Check RST bit
            if ((flags & (byte)TCPFlags.TCP_RST) != 0)
            {
                SendPacket(conn, conn.sndNxt, (byte)TCPFlags.TCP_ACK, null, 0);
                RecvRst(conn, hdr);
                return;
            }

            // Check SYN bit
            if ((flags & (byte)TCPFlags.TCP_SYN) != 0)
            {
                SendPacket(conn, 0, (byte)TCPFlags.TCP_RST | (byte)TCPFlags.TCP_ACK, null, 0);
                Console.WriteLine("Error: connection reset");
            }

            // Check ACK
            if ((~flags & (byte)TCPFlags.TCP_ACK) != 0)
            {
                return;
            }

            RecvAck(conn, hdr);

            // TODO - check URG

            // Process segment data
            if (length != 0)
            {
                RecvData(conn, hdr->seq, buffer, length);
            }

            // Check FIN - TODO, needs to handle out of sequence
            if ((flags & (byte)TCPFlags.TCP_FIN) != 0)
            {
                RecvFin(conn, hdr);
            }
        }

        private static void RecvFin(TcpClient conn, TCPHeader* hdr)
        {
            // TODO - signal the user "connection closing" and return any pending receives

            conn.rcvNxt = hdr->seq + 1;
            SendPacket(conn, conn.sndNxt, (byte)TCPFlags.TCP_ACK, null, 0);

            switch (conn.State)
            {
                case TCPStatus.SynReceived:
                case TCPStatus.Established:
                    conn.State = TCPStatus.CloseWait;
                    break;

                case TCPStatus.FinWait1:
                    if ((hdr->ack - conn.sndNxt) >= 0)
                    {
                        // TODO - is this the right way to detect that our FIN has been ACK'd?

                        // TODO - turn off the other timers
                        conn.State = TCPStatus.TimeWait;
                    }
                    else
                    {
                        conn.State = TCPStatus.Closing;
                    }
                    break;

                case TCPStatus.FinWait2:
                    // TODO - turn off the other timers
                    conn.State = TCPStatus.TimeWait;
                    break;

                case TCPStatus.CloseWait:
                case TCPStatus.Closing:
                case TCPStatus.LastAcknowledge:
                    break;

                case TCPStatus.TimeWait:
                    break;
            }
        }

        private static void RecvData(TcpClient conn, uint seq, byte* buffer, int length)
        {
            switch (conn.State)
            {
                case TCPStatus.SynReceived:
                    // TODO - can this happen? ACK processing would transition to ESTABLISHED state.
                    break;

                case TCPStatus.Established:
                case TCPStatus.FinWait1:
                case TCPStatus.FinWait2:

                    //Maybe bug here?
                    if (conn.rcvNxt == seq)
                    {
                        conn._OnData(buffer, length);
                        conn.rcvNxt += (uint)length;
                    }

                    // Acknowledge receipt of data
                    SendPacket(conn, conn.sndNxt, (byte)TCPFlags.TCP_ACK);
                    break;

                default:
                    // FIN has been received from the remote side - ignore the segment data.
                    break;
            }
        }

        private static void RecvAck(TcpClient conn, TCPHeader* hdr)
        {
            switch (conn.State)
            {
                case TCPStatus.SynReceived:
                    if (conn.sndUna <= hdr->ack && hdr->ack <= conn.sndNxt)
                    {
                        conn.sndWnd = hdr->windowSize;
                        conn.sndWl1 = hdr->seq;
                        conn.sndWl2 = hdr->ack;
                        conn.State = TCPStatus.Established;
                    }
                    else
                    {
                        SendPacket(conn, hdr->ack, (byte)TCPFlags.TCP_RST, null, 0);
                    }
                    break;

                case TCPStatus.Established:
                case TCPStatus.FinWait1:
                case TCPStatus.FinWait2:
                case TCPStatus.CloseWait:
                case TCPStatus.Closing:
                    //Accept ack
                    if (hdr->seq == PacketAck) PacketSent = true;
                    // Handle expected acks
                    if ((conn.sndUna - hdr->ack) <= 0 && (hdr->ack - conn.sndNxt) <= 0)
                    {
                        // Update acknowledged pointer
                        conn.sndUna = hdr->ack;

                        // Update send window
                        if ((conn.sndWl1 - hdr->seq) < 0 ||
                            (conn.sndWl1 == hdr->seq && (conn.sndWl2 - hdr->ack) <= 0))
                        {
                            conn.sndWnd = hdr->windowSize;
                            conn.sndWl1 = hdr->seq;
                            conn.sndWl2 = hdr->ack;
                        }

                        // TODO - remove segments on the retransmission queue which have been ack'd
                        // TODO - acknowledge buffers which have sent to user
                    }

                    // Check for duplicate ack
                    if ((hdr->ack - conn.sndUna) <= 0)
                    {
                        // TODO - anything to do here?
                    }

                    // Check for ack of unsent data
                    if ((hdr->ack - conn.sndNxt) > 0)
                    {
                        SendPacket(conn, conn.sndNxt, (byte)TCPFlags.TCP_ACK, null, 0);
                        return;
                    }

                    // Check for ack of FIN
                    if ((hdr->ack - conn.sndNxt) >= 0)
                    {
                        // TODO - is this the right way to detect that our FIN has been ACK'd?
                        if (conn.State == TCPStatus.FinWait1)
                        {
                            conn.State = TCPStatus.FinWait2;
                        }
                        else if (conn.State == TCPStatus.Closing)
                        {
                            conn.State = TCPStatus.TimeWait;
                        }
                    }

                    break;

                case TCPStatus.LastAcknowledge:
                    // Check for ack of FIN
                    if ((hdr->ack - conn.sndNxt) >= 0)
                    {
                        // TODO - is this the right way to detect that our FIN has been ACK'd?

                        //TcpFree(conn);
                    }
                    break;

                case TCPStatus.TimeWait:
                    // This case is handled in the FIN processing step.
                    break;
            }
        }

        private static void RecvRst(TcpClient conn, TCPHeader* hdr)
        {
            switch (conn.State)
            {
                case TCPStatus.SynReceived:
                    // TODO - All segments on the retransmission queue should be removed

                    // TODO - If initiated with a passive open, go to LISTEN state
                    Console.WriteLine("Error: tcp connection refused");
                    //TcpError(conn, TCP_CONN_REFUSED);
                    break;

                case TCPStatus.Established:
                case TCPStatus.FinWait1:
                case TCPStatus.FinWait2:
                case TCPStatus.CloseWait:
                    // TODO - All outstanding sends should receive "reset" responses

                    Console.WriteLine("Error: tcp reset");
                    break;

                case TCPStatus.Closing:
                case TCPStatus.LastAcknowledge:
                case TCPStatus.TimeWait:
                    //TcpFree(conn);
                    break;
            }
        }

        private const byte TCP_CONN_RESET = 1;
        private const byte TCP_CONN_REFUSED = 2;
        private const byte TCP_CONN_CLOSING = 3;

        private static void RecvSynSent(TcpClient conn, TCPHeader* hdr)
        {
            byte flags = hdr->flags;

            // Check for bad ACK first.
            if ((flags & (byte)TCPFlags.TCP_ACK) != 0)
            {
                if ((hdr->ack - conn.iss) <= 0 || (hdr->ack - conn.sndNxt) > 0)
                {
                    if ((~flags & (byte)TCPFlags.TCP_RST) != 0)
                    {
                        SendPacket(conn, hdr->ack, (byte)TCPFlags.TCP_RST, null, 0);
                    }

                    Console.WriteLine("Bad ACK");
                    return;
                }
            }

            // Check for RST
            if ((flags & (byte)TCPFlags.TCP_RST) != 0)
            {
                if ((flags & (byte)TCPFlags.TCP_ACK) != 0)
                {
                    Console.WriteLine("TCP Reset");
                }

                return;
            }

            // Check SYN
            if ((flags & (byte)TCPFlags.TCP_SYN) != 0)
            {
                // SYN is set.  ACK is either ok or there was no ACK.  No RST.

                conn.rcvNxt = hdr->seq + 1;

                if ((flags & (byte)TCPFlags.TCP_ACK) != 0)
                {
                    conn.sndUna = hdr->ack;
                    conn.sndWnd = hdr->windowSize;
                    conn.sndWl1 = hdr->seq;
                    conn.sndWl2 = hdr->ack;

                    // TODO - Segments on the retransmission queue which are ack'd should be removed

                    conn.State = TCPStatus.Established;
                    SendPacket(conn, conn.sndNxt, (byte)TCPFlags.TCP_ACK, null, 0);
                    Console.WriteLine("Connection established");


                    // TODO - Data queued for transmission may be included with the ACK.

                    // TODO - If there is data in the segment, continue processing at the URG phase.
                }
                else
                {
                    Console.WriteLine("No response");

                    conn.State = TCPStatus.SynReceived;

                    // Resend ISS
                    --conn.sndNxt;
                    SendPacket(conn, conn.sndNxt, (byte)TCPFlags.TCP_SYN | (byte)TCPFlags.TCP_ACK, null, 0);
                }
            }
        }

        private static TcpClient currConn;

        public static TcpClient Connect(byte[] addr, ushort port, ushort localPort)
        {
            TcpClient conn = new();

            currConn = conn;

            // Initialize connection
            conn.localAddr[0] = Network.IP[0];
            conn.localAddr[1] = Network.IP[1];
            conn.localAddr[2] = Network.IP[2];
            conn.localAddr[3] = Network.IP[3];

            conn.nextAddr[0] = addr[0];
            conn.nextAddr[1] = addr[1];
            conn.nextAddr[2] = addr[2];
            conn.nextAddr[3] = addr[3];

            conn.remoteAddr[0] = addr[0];
            conn.remoteAddr[1] = addr[1];
            conn.remoteAddr[2] = addr[2];
            conn.remoteAddr[3] = addr[3];

            conn.localPort = localPort;
            conn.remotePort = port;

            conn.sndUna = 0;
            conn.sndNxt = 0;
            conn.sndWnd = TCP_WINDOW_SIZE;
            conn.sndUP = 0;
            conn.sndWl1 = 0;
            conn.sndWl2 = 0;
            conn.iss = 0;

            conn.rcvNxt = 0;
            conn.rcvWnd = TCP_WINDOW_SIZE;
            conn.rcvUP = 0;

            // Issue SYN segment
            SendPacket(conn, conn.sndNxt, (byte)TCPFlags.TCP_SYN);
            //TcpSetState(conn, TCP_SYN_SENT);
            conn.State = TCPStatus.SynSent;

            ulong t = Timer.Ticks + 3000;
            while ((Timer.Ticks < t) && !conn.Connected)
            {
                Native.Hlt();
            }
            if (conn.State == TCPStatus.SynSent)
            {
                Console.WriteLine("Connection timeout");
            }

            return conn;
        }

        private static void SendPacket(TcpClient conn, uint seq, byte flags)
        {
            SendPacket(conn, conn.sndNxt, flags, null, 0);
        }

        private static void SendPacket(TcpClient conn, uint seq, byte flags, void* data, uint count)
        {
            byte* buffer = (byte*)Allocator.Allocate(TCP_WINDOW_SIZE);

            // Header
            TCPHeader* hdr = (TCPHeader*)buffer;
            hdr->srcPort = conn.localPort;

            hdr->dstPort = conn.remotePort;
            hdr->seq = seq;
            hdr->ack = ((flags & (byte)TCPFlags.TCP_ACK) != 0) ? conn.rcvNxt : 0;
            hdr->off = 0;
            hdr->flags = flags;
            hdr->windowSize = TCP_WINDOW_SIZE;
            hdr->checksum = 0;
            hdr->urgent = 0;
            Swap(hdr);

            byte* p = buffer + sizeof(TCPHeader);

            if ((flags & (byte)TCPFlags.TCP_SYN) != 0)
            {
                // Maximum Segment Size
                p[0] = (byte)TCPOption.MSS;
                p[1] = 4;
                *(ushort*)(p + 2) = Ethernet.SwapLeftRight(1460);
                p += p[1];
            }

            // Option End
            while (((p - buffer) & 3) != 0)
            {
                *p++ = 0;
            }

            hdr->off = (byte)((p - buffer) << 2);

            // Data
            if (data != null)
            {
                Native.Movsb(p, data, count);
            }

            byte* end = p + count;

            // Pseudo Header
            ChecksumHeader* phdr = (ChecksumHeader*)(buffer - sizeof(ChecksumHeader));
            phdr->src[0] = conn.localAddr[0];
            phdr->src[1] = conn.localAddr[1];
            phdr->src[2] = conn.localAddr[2];
            phdr->src[3] = conn.localAddr[3];
            phdr->dst[0] = conn.remoteAddr[0];
            phdr->dst[1] = conn.remoteAddr[1];
            phdr->dst[2] = conn.remoteAddr[2];
            phdr->dst[3] = conn.remoteAddr[3];

            phdr->reserved = 0;
            phdr->protocol = (byte)IPv4.IPv4Protocol.TCP;
            phdr->len = Ethernet.SwapLeftRight((ushort)((uint)end - (uint)buffer));

            // Checksum
            //ushort checksum = NetChecksum(pkt->start - sizeof(ChecksumHeader), pkt->end);
            ushort checksum = TCPChecksum(buffer - sizeof(ChecksumHeader), end);
            hdr->checksum = Ethernet.SwapLeftRight(checksum);

            // Transmit
            IPv4.SendPacket(new byte[]
            {
                conn.remoteAddr [0],
                conn.remoteAddr [1],
                conn.remoteAddr [2],
                conn.remoteAddr [3]
            }, (byte)IPv4.IPv4Protocol.TCP, buffer, (int)end - (int)buffer);

            Allocator.Free((System.IntPtr)buffer);

            // Update State
            conn.sndNxt += count;
            if ((flags & ((byte)TCPFlags.TCP_SYN | (byte)TCPFlags.TCP_FIN)) != 0)
            {
                ++conn.sndNxt;
            }
        }

        private static void Swap(TCPHeader* hdr)
        {
            hdr->srcPort = Ethernet.SwapLeftRight(hdr->srcPort);
            hdr->dstPort = Ethernet.SwapLeftRight(hdr->dstPort);

            hdr->seq = Ethernet.SwapLeftRight32(hdr->seq);
            hdr->ack = Ethernet.SwapLeftRight32(hdr->ack);

            hdr->windowSize = Ethernet.SwapLeftRight(hdr->windowSize);
            hdr->checksum = Ethernet.SwapLeftRight(hdr->checksum);
            hdr->urgent = Ethernet.SwapLeftRight(hdr->urgent);
        }

        private static bool PacketSent = false;
        private static uint PacketAck = 0;

        public static void Send(TcpClient conn, byte* data, int count)
        {
            if (conn.Connected)
            {
                PacketSent = false;
                PacketAck = conn.rcvNxt;
                uint sndNxt = conn.sndNxt;
                for (; ; )
                {
                    SendPacket(conn, sndNxt, (byte)TCPFlags.TCP_ACK | (byte)TCPFlags.TCP_PSH, data, (uint)count);
                    Timer.Wait(1000);
                    if (PacketSent) break;
                    Console.WriteLine("Packet may not accpeted. resending");
                }
                Console.WriteLine("Packet sent");
            }
        }

        private static ushort TCPChecksum(byte* data, byte* end)
        {
            uint len = (uint)(end - data);
            ushort* p = (ushort*)data;

            uint sum = 0;

            while (len > 1)
            {
                sum += *p++;
                len -= 2;
            }

            if ((len) != 0)
            {
                sum += *(byte*)p;
            }

            sum = (sum & 0xffff) + (sum >> 16);
            sum += (sum >> 16);

            ushort temp = (ushort)~sum;
            return (ushort)(((temp & 0x00ff) << 8) | ((temp & 0xff00) >> 8)); // TODO - shouldn't swap this twice
        }
    }
}
