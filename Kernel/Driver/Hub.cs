using MOOS.Misc;

namespace MOOS.Driver
{
    public static unsafe class Hub
    {
        struct Desc
        {
            public byte Length;
            public byte Type;
            public byte PortCount;
            public ushort Characteristic;
            public byte PortPowerTime;
            public byte Current;
        }

        static USBRequest* req;

        public static void Initialize() 
        {
            req = (USBRequest*)Allocator.Allocate((ulong)sizeof(USBRequest));
        }

        internal static void Initialize(USBDevice device)
        {
            Desc* desc = (Desc*)Allocator.Allocate((ulong)sizeof(Desc));
            bool b= USB.SendAndReceive(device, desc,0xA0,0x06,0x2900,0, sizeof(Desc));
            if (!b) 
            {
                Console.WriteLine("[USB Hub] Can't get Hub descriptor");
            }

            Console.WriteLine($"[USB Hub] This hub has {desc->PortCount} ports");

            for(int i = 0; i < desc->PortCount; i++) 
            {
                if((desc->Characteristic & 0x03) == 0x01)
                {
                    b = USB.SendAndReceive(device, null, 0x23, 0x03, 8, ((ushort)(i + 1)), 0);
                    if (!b)
                    {
                        Console.WriteLine($"[USB Hub] Can't set power for Hub port {i}");
                        continue;
                    }
                    ACPITimer.Sleep(100000);
                }

                b = USB.SendAndReceive(device, null, 0x23, 0x03, 4, ((ushort)(i + 1)), 0);
                if (!b)
                {
                    Console.WriteLine($"[USB Hub] Can't reset Hub port {i}");
                    continue;
                }
                ACPITimer.Sleep(100000);

                uint status = 0;
                for(int k = 0; k < 8; k++) 
                {
                    b = USB.SendAndReceive(device, &status, 0xA3, 0, 0, ((ushort)(i + 1)), sizeof(uint));
                    if (!b)
                    {
                        Console.WriteLine($"[USB Hub] Can't get Hub port {i} status");
                        return;
                    }

                    if (status != 0) break;
                    ACPITimer.Sleep(100000);
                }

                int speed = (int)((status & ((3 << 9))) >> 9);

                if (status & 2) 
                {
                    USB.InitPort(i, device, 2, speed);
                }
            }
        }
    }
}
