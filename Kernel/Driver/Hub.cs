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

            (*req).Clean();
            req->RequestType = 0xA0;
            req->Request = 0x06;
            req->Value = 0x2900;
            req->Index = 0;
            req->Length = (ushort)sizeof(Desc);
            bool b= USB.SendAndReceive(device, req, desc, null);
            if (!b) 
            {
                Console.WriteLine("[USB Hub] Can't get Hub descriptor");
            }

            Console.WriteLine($"[USB Hub] This hub has {desc->PortCount} ports");

            for(int i = 0; i < desc->PortCount; i++) 
            {
                (*req).Clean();
                req->RequestType = 0x23;
                req->Request = 0x03;
                req->Value = 8;
                req->Index = ((ushort)(i + 1));
                req->Length = 0;
                b = USB.SendAndReceive(device, req, null, null);
                ACPITimer.SleepMicroseconds(100000);


                (*req).Clean();
                req->RequestType = 0x23;
                req->Request = 0x03;
                req->Value = 4;
                req->Index = ((ushort)(i + 1));
                req->Length = 0;
                b= USB.SendAndReceive(device, req, null, null);
                ACPITimer.SleepMicroseconds(100000);

                uint status = 0;
                for(int k = 0; k < 8; k++) 
                {
                    (*req).Clean();
                    req->RequestType = 0xA3;
                    req->Request = 0;
                    req->Value = 0;
                    req->Index = ((ushort)(i + 1));
                    req->Length = sizeof(uint);
                    b = USB.SendAndReceive(device, req, &status, null);
                    if (!b)
                    {
                        Console.WriteLine($"[USB Hub] Can't get Hub port {i} status");
                        return;
                    }

                    if (status != 0) break;
                    ACPITimer.SleepMicroseconds(100000);
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
