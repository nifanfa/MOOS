//https://github.com/pdoane/osdev/blob/master/net/tcp.c

using System.Runtime.InteropServices;

namespace Kernel.NET
{
    public unsafe class TCPConnection
    {
        public void* link;
        public uint state;
        public void* intf;

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
        public uint irs;                            // initial receive sequence number

        // queues
        public void* resequence;

        // timers
        public uint mslWait;                       // when does the 2MSL time wait expire?

        // callbacks
        public void* ctx;
        public void* onError;
        public void* onState;
        public void* onData;
    }

    internal unsafe class TCP
    {
        const byte TCP_CLOSED = 0;
        const byte TCP_LISTEN = 1;
        const byte TCP_SYN_SENT = 2;
        const byte TCP_SYN_RECEIVED = 3;
        const byte TCP_ESTABLISHED = 4;
        const byte TCP_FIN_WAIT_1 = 5;
        const byte TCP_FIN_WAIT_2 = 6;
        const byte TCP_CLOSE_WAIT = 7;
        const byte TCP_CLOSING = 8;
        const byte TCP_LAST_ACK = 9;
        const byte TCP_TIME_WAIT = 10;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct TcpOptions
        {
            public ushort mss;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NetBuf
        {
            public byte* start;          // offset to data start
            public byte* end;            // offset to data end exclusive
            public uint refCount;
            public uint seq;            // Data from TCP header used for out-of-order/retransmit
            public byte flags;          // Data from TCP header used for out-of-order/retransmit
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TcpHeader
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

        const ushort TCP_WINDOW_SIZE = 8192;

        const byte TCP_FIN = (1 << 0);
        const byte TCP_SYN = (1 << 1);
        const byte TCP_RST = (1 << 2);
        const byte TCP_PSH = (1 << 3);
        const byte TCP_ACK = (1 << 4);
        const byte TCP_URG = (1 << 5);

        const byte OPT_END = 0;
        const byte OPT_NOP = 1;
        const byte OPT_MSS = 2;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ChecksumHeader
        {
            public fixed byte src[4];
            public uint bits1;
            public fixed byte dst[4];
            public uint bits2;
            public byte reserved;
            public byte protocol;
            public ushort len;
        }

        static ulong s_baseIsn = 0;

        public static void Init()
        {
            ulong t =
                (ulong)(RTC.Second +
        RTC.Minute * 60 +
        RTC.Hour * 3600 +
        RTC.Day * 86400 +
        (RTC.Year - 70) * 31536000 +
        ((RTC.Year - 69) / 4) * 86400 -
        ((RTC.Year - 1) / 100) * 86400 +
        ((RTC.Year + 299) / 400) * 86400);
            s_baseIsn = (t * 1000 - PIT.Tick) * 250;
        }

        internal static void TcpRecv(byte* buffer, int length)
        {
            TcpHeader* hdr = (TcpHeader*)buffer;
            buffer += sizeof(TcpHeader);
            length -= sizeof(TcpHeader);
            Swap(hdr);

            if (currConn == null)
            {
                Console.WriteLine("TCP connection havn't established yet");
                return;
            }

            if (currConn.state == TCP_CLOSED)
            {
                //TO-DO send close ack
                return;
            }

            if (currConn.state == TCP_LISTEN)
            {
            }
            else if (currConn.state == TCP_SYN_SENT)
            {
                RecvSynSent(currConn, hdr);
            }
            else
            {
                RecvGeneral(currConn, hdr, buffer, length);
            }
        }

        static void RecvGeneral(TCPConnection conn, TcpHeader* hdr, byte* buffer, int length)
        {
            // Process segments not in the CLOSED, LISTEN, or SYN-SENT states.

            uint flags = hdr->flags;
            uint dataLen = (uint)length;

            // Check that sequence and segment data is acceptable
            if (!((conn.rcvNxt - hdr->seq) <= 0 && (hdr->seq + dataLen - conn.rcvNxt + conn.rcvWnd) <= 0))
            {
                Console.WriteLine("Unacceptable segment");

                for (int i = 0; i < length; i++)
                    Console.Write((char)buffer[i]);
                Console.WriteLine();

                // Unacceptable segment
                if ((~flags & TCP_RST) != 0)
                {
                    SendPacket(conn, conn.sndNxt, TCP_ACK, null, 0);
                }

                return;
            }

            // TODO - trim segment data?

            // Check RST bit
            if ((flags & TCP_RST) != 0)
            {
                RecvRst(conn, hdr);
                return;
            }

            // Check SYN bit
            if ((flags & TCP_SYN) != 0)
            {
                SendPacket(conn, 0, TCP_RST | TCP_ACK, (void*)0, 0);
                Console.WriteLine("Error: connection reset");
            }

            // Check ACK
            if ((~flags & TCP_ACK) != 0)
            {
                return;
            }

            RecvAck(conn, hdr);

            // TODO - check URG

            // Process segment data
            if (dataLen != 0)
            {
                RecvData(conn, buffer, length);
            }

            // Check FIN - TODO, needs to handle out of sequence
            if ((flags & TCP_FIN) != 0)
            {
                RecvFin(conn, hdr);
                Console.WriteLine("Buffer Received");

                for (int i = 0; i < bufferLength; i++)
                    Console.Write((char)bufferReceived[i]);
                Console.WriteLine();
            }
        }

        static void RecvFin(TCPConnection conn, TcpHeader* hdr)
        {
            // TODO - signal the user "connection closing" and return any pending receives

            conn.rcvNxt = hdr->seq + 1;
            SendPacket(conn, conn.sndNxt, TCP_ACK, (void*)0, 0);

            switch (conn.state)
            {
                case TCP_SYN_RECEIVED:
                case TCP_ESTABLISHED:
                    conn.state = TCP_CLOSE_WAIT;
                    break;

                case TCP_FIN_WAIT_1:
                    if ((hdr->ack - conn.sndNxt) >= 0)
                    {
                        // TODO - is this the right way to detect that our FIN has been ACK'd?

                        // TODO - turn off the other timers
                        conn.state = TCP_TIME_WAIT;
                        conn.mslWait = (uint)(PIT.Tick + 2 * 120000);
                    }
                    else
                    {
                        conn.state = TCP_CLOSING;
                    }
                    break;

                case TCP_FIN_WAIT_2:
                    // TODO - turn off the other timers
                    conn.state = TCP_TIME_WAIT;
                    conn.mslWait = (uint)(PIT.Tick + 2 * 120000);
                    break;

                case TCP_CLOSE_WAIT:
                case TCP_CLOSING:
                case TCP_LAST_ACK:
                    break;

                case TCP_TIME_WAIT:
                    conn.mslWait = (uint)(PIT.Tick + 2 * 120000);
                    break;
            }
        }

        static void RecvData(TCPConnection conn, byte* buffer, int length)
        {
            switch (conn.state)
            {
                case TCP_SYN_RECEIVED:
                    // TODO - can this happen? ACK processing would transition to ESTABLISHED state.
                    break;

                case TCP_ESTABLISHED:
                case TCP_FIN_WAIT_1:
                case TCP_FIN_WAIT_2:
                    // Insert packet on to input queue sorted by sequence

                    if (bufferLength > 8192) Console.WriteLine("Error: packet cache is full");
                    else
                    {
                        Native.Movsb(bufferReceived + bufferLength, buffer, (ulong)length);
                        bufferLength += length;
                    }

                    // Acknowledge receipt of data
                    SendPacket(conn, conn.sndNxt, TCP_ACK, (void*)0, 0);
                    break;

                default:
                    // FIN has been received from the remote side - ignore the segment data.
                    break;
            }
        }

        public static byte* bufferReceived;
        public static int bufferLength;

        static void RecvAck(TCPConnection conn, TcpHeader* hdr)
        {
            switch (conn.state)
            {
                case TCP_SYN_RECEIVED:
                    if (conn.sndUna <= hdr->ack && hdr->ack <= conn.sndNxt)
                    {
                        conn.sndWnd = hdr->windowSize;
                        conn.sndWl1 = hdr->seq;
                        conn.sndWl2 = hdr->ack;
                        conn.state = TCP_ESTABLISHED;
                    }
                    else
                    {
                        SendPacket(conn, hdr->ack, TCP_RST, (void*)0, 0);
                    }
                    break;

                case TCP_ESTABLISHED:
                case TCP_FIN_WAIT_1:
                case TCP_FIN_WAIT_2:
                case TCP_CLOSE_WAIT:
                case TCP_CLOSING:
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
                        SendPacket(conn, conn.sndNxt, TCP_ACK, (void*)0, 0);
                        return;
                    }

                    // Check for ack of FIN
                    if ((hdr->ack - conn.sndNxt) >= 0)
                    {
                        // TODO - is this the right way to detect that our FIN has been ACK'd?
                        if (conn.state == TCP_FIN_WAIT_1)
                        {
                            conn.state = TCP_FIN_WAIT_2;
                        }
                        else if (conn.state == TCP_CLOSING)
                        {
                            conn.state = TCP_TIME_WAIT;
                            conn.mslWait = (uint)(PIT.Tick + 2 * 120000);
                        }
                    }

                    break;

                case TCP_LAST_ACK:
                    // Check for ack of FIN
                    if ((hdr->ack - conn.sndNxt) >= 0)
                    {
                        // TODO - is this the right way to detect that our FIN has been ACK'd?

                        //TcpFree(conn);
                    }
                    break;

                case TCP_TIME_WAIT:
                    // This case is handled in the FIN processing step.
                    break;
            }
        }

        static void RecvRst(TCPConnection conn, TcpHeader* hdr)
        {
            switch (conn.state)
            {
                case TCP_SYN_RECEIVED:
                    // TODO - All segments on the retransmission queue should be removed

                    // TODO - If initiated with a passive open, go to LISTEN state
                    Console.WriteLine("Error: tcp connection refused");
                    //TcpError(conn, TCP_CONN_REFUSED);
                    break;

                case TCP_ESTABLISHED:
                case TCP_FIN_WAIT_1:
                case TCP_FIN_WAIT_2:
                case TCP_CLOSE_WAIT:
                    // TODO - All outstanding sends should receive "reset" responses

                    Console.WriteLine("Error: tcp reset");
                    break;

                case TCP_CLOSING:
                case TCP_LAST_ACK:
                case TCP_TIME_WAIT:
                    //TcpFree(conn);
                    break;
            }
        }

        const byte TCP_CONN_RESET = 1;
        const byte TCP_CONN_REFUSED = 2;
        const byte TCP_CONN_CLOSING = 3;

        static void RecvSynSent(TCPConnection conn, TcpHeader* hdr)
        {
            byte flags = hdr->flags;

            // Check for bad ACK first.
            if ((flags & TCP_ACK) != 0)
            {
                if ((hdr->ack - conn.iss) <= 0 || (hdr->ack - conn.sndNxt) > 0)
                {
                    if ((~flags & TCP_RST) != 0)
                    {
                        SendPacket(conn, hdr->ack, TCP_RST, null, 0);
                    }

                    Console.WriteLine("Bad ACK");
                    return;
                }
            }

            // Check for RST
            if ((flags & TCP_RST) != 0)
            {
                if ((flags & TCP_ACK) != 0)
                {
                    conn.state = TCP_CONN_RESET;
                    Console.WriteLine("TCP Reset");
                }

                return;
            }

            // Check SYN
            if ((flags & TCP_SYN) != 0)
            {
                // SYN is set.  ACK is either ok or there was no ACK.  No RST.

                conn.irs = hdr->seq;
                conn.rcvNxt = hdr->seq + 1;

                if ((flags & TCP_ACK) != 0)
                {
                    conn.sndUna = hdr->ack;
                    conn.sndWnd = hdr->windowSize;
                    conn.sndWl1 = hdr->seq;
                    conn.sndWl2 = hdr->ack;

                    // TODO - Segments on the retransmission queue which are ack'd should be removed

                    conn.state = TCP_ESTABLISHED;
                    SendPacket(conn, conn.sndNxt, TCP_ACK, null, 0);
                    Console.WriteLine("Connection established");


                    // TODO - Data queued for transmission may be included with the ACK.

                    // TODO - If there is data in the segment, continue processing at the URG phase.
                }
                else
                {
                    Console.WriteLine("No response");

                    conn.state = TCP_SYN_RECEIVED;

                    // Resend ISS
                    --conn.sndNxt;
                    SendPacket(conn, conn.sndNxt, TCP_SYN | TCP_ACK, null, 0);
                }
            }
        }

        static TCPConnection currConn;

        public static TCPConnection Connect(byte[] addr, ushort port, ushort localPort)
        {
            bufferReceived = (byte*)Platform.kmalloc(8192);

            TCPConnection conn = new TCPConnection();

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

            var isn = s_baseIsn + PIT.Tick * 250;

            conn.sndUna = (uint)isn;
            conn.sndNxt = (uint)isn;
            conn.sndWnd = TCP_WINDOW_SIZE;
            conn.sndUP = 0;
            conn.sndWl1 = 0;
            conn.sndWl2 = 0;
            conn.iss = (uint)isn;

            conn.rcvNxt = 0;
            conn.rcvWnd = TCP_WINDOW_SIZE;
            conn.rcvUP = 0;
            conn.irs = 0;

            // Issue SYN segment
            SendPacket(conn, conn.sndNxt, TCP_SYN);
            //TcpSetState(conn, TCP_SYN_SENT);
            conn.state = TCP_SYN_SENT;

            ulong t = PIT.Tick + 3000;
            while(PIT.Tick < t) 
            {
                Native.Hlt();
            }
            if (conn.state == TCP_SYN_SENT) Console.WriteLine("Failed to connect");

            return conn;
        }

        public static void SendPacket(TCPConnection conn, uint seq, byte flags)
        {
            SendPacket(conn, conn.sndNxt, (byte)TCP_SYN, (void*)0, 0);
        }

        public static void SendPacket(TCPConnection conn, uint seq, byte flags, void* data, uint count)
        {
            NetBuf* pkt = NetAllocBuf();

            // Header
            TcpHeader* hdr = (TcpHeader*)pkt->start;
            hdr->srcPort = conn.localPort;

            hdr->dstPort = conn.remotePort;
            hdr->seq = seq;
            hdr->ack = ((flags & TCP_ACK) != 0) ? conn.rcvNxt : 0;
            hdr->off = 0;
            hdr->flags = flags;
            hdr->windowSize = TCP_WINDOW_SIZE;
            hdr->checksum = 0;
            hdr->urgent = 0;
            Swap(hdr);

            byte* p = pkt->start + sizeof(TcpHeader);

            if ((flags & TCP_SYN) != 0)
            {
                // Maximum Segment Size
                p[0] = OPT_MSS;
                p[1] = 4;
                *(ushort*)(p + 2) = Ethernet.SwapLeftRight(1460);
                p += p[1];
            }

            // Option End
            while (((p - pkt->start) & 3) != 0)
            {
                *p++ = 0;
            }

            hdr->off = (byte)((p - pkt->start) << 2);

            // Data
            if (data != null)
                Native.Movsb(p, data, count);
            pkt->end = p + count;

            // Pseudo Header
            ChecksumHeader* phdr = (ChecksumHeader*)(pkt->start - sizeof(ChecksumHeader));
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
            phdr->len = Ethernet.SwapLeftRight((ushort)((uint)pkt->end - (uint)pkt->start));

            // Checksum
            //ushort checksum = NetChecksum(pkt->start - sizeof(ChecksumHeader), pkt->end);
            ushort checksum = NetChecksum(pkt->start - sizeof(ChecksumHeader), pkt->end);
            hdr->checksum = Ethernet.SwapLeftRight(checksum);

            // Transmit
            IPv4.SendPacket(new byte[]
            {
                conn.remoteAddr [0],
                conn.remoteAddr [1],
                conn.remoteAddr [2],
                conn.remoteAddr [3]
            }, (byte)IPv4.IPv4Protocol.TCP, pkt->start, (int)pkt->end - (int)pkt->start);

            // Update State
            conn.sndNxt += count;
            if ((flags & (TCP_SYN | TCP_FIN)) != 0)
            {
                ++conn.sndNxt;
            }
        }

        private static NetBuf* NetAllocBuf()
        {
            NetBuf* buf = (NetBuf*)Platform.kmalloc(2048);
            buf->start = (byte*)buf + 256;
            buf->end = (byte*)buf + 256;
            return buf;
        }

        static void Swap(TcpHeader* hdr)
        {
            hdr->srcPort = Ethernet.SwapLeftRight(hdr->srcPort);
            hdr->dstPort = Ethernet.SwapLeftRight(hdr->dstPort);

            hdr->seq = Ethernet.SwapLeftRight32(hdr->seq);
            hdr->ack = Ethernet.SwapLeftRight32(hdr->ack);

            hdr->windowSize = Ethernet.SwapLeftRight(hdr->windowSize);
            hdr->checksum = Ethernet.SwapLeftRight(hdr->checksum);
            hdr->urgent = Ethernet.SwapLeftRight(hdr->urgent);
        }

        public static void Send(TCPConnection conn, byte* data, int count)
        {
            SendPacket(conn, conn.sndNxt, TCP_ACK | TCP_PSH, data, (uint)count);
        }

        public static ushort NetChecksum(byte* data, byte* end)
        {
            uint sum = NetChecksumAcc(data, end, 0);
            return NetChecksumFinal(sum);
        }

        public static ushort NetChecksum(byte* data, int count)
        {
            uint sum = NetChecksumAcc(data, data + count, 0);
            return NetChecksumFinal(sum);
        }

        static uint NetChecksumAcc(byte* data, byte* end, uint sum)
        {
            uint len = (uint)(end - data);
            ushort* p = (ushort*)data;

            while (len > 1)
            {
                sum += *p++;
                len -= 2;
            }

            if ((len) != 0)
            {
                sum += *(byte*)p;
            }

            return sum;
        }

        static ushort NetChecksumFinal(uint sum)
        {
            sum = (sum & 0xffff) + (sum >> 16);
            sum += (sum >> 16);

            ushort temp = (ushort)~sum;
            return (ushort)(((temp & 0x00ff) << 8) | ((temp & 0xff00) >> 8)); // TODO - shouldn't swap this twice
        }
    }
}
