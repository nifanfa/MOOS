using MOOS.Misc;

namespace MOOS.Driver
{
    public static unsafe class Hub
    {
        struct Desc
        {
            public byte len;
            public byte type;
            public byte portCount;
            public ushort chars;
            public byte portPowerTime;
            public byte current;
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
                Console.WriteLine("Can't get Hub descriptor");
            }

            Console.WriteLine($"This hub has {desc->portCount} ports");

            for(int i = 0; i < desc->portCount; i++) 
            {
                (*req).Clean();
                req->RequestType = 0x23;
                req->Request = 0x03;
                req->Value = 8;
                req->Index = ((ushort)(i + 1));
                req->Length = 0;
                b = USB.SendAndReceive(device, req, null, null);
                ACPITimer.Sleep(100000);


                (*req).Clean();
                req->RequestType = 0x23;
                req->Request = 0x03;
                req->Value = 4;
                req->Index = ((ushort)(i + 1));
                req->Length = 0;
                b= USB.SendAndReceive(device, req, null, null);
                ACPITimer.Sleep(100000);

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
                        Console.WriteLine($"Can't get Hub port {i} status");
                        return;
                    }

                    if (status != 0) break;
                    ACPITimer.Sleep(100000);
                }

                if(status & 2) 
                {
                    USB.InitPort(i, device);
                }
            }
        }
    }
}
