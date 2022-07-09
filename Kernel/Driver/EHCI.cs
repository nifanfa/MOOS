using MOOS.Misc;
using System.Runtime.InteropServices;

namespace MOOS.Driver
{
    public static unsafe class EHCI
    {
        public static uint BaseAddr;

        public static uint CMDReg;
        public static uint AsyncListReg;

        public static byte AvailablePorts;

        public static byte DeviceAddr;

        public const int FrameSize = 1024;

        public static void Initialize()
        {
            PCIDevice device = PCI.GetDevice(0x0C, 0x03, 0x20);
            if (device == null) return;

            Console.WriteLine("EHCI controller found!");

            device.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            uint bar0 = device.Bar0;
            Console.WriteLine($"Bar0: {bar0.ToString("x2")}");
            BaseAddr = bar0 + *(byte*)bar0;
            ushort ver = *(ushort*)(bar0 + 0x02);
            if (ver != 0x100)
            {
                Panic.Error("This controller is not supported!");
                return;
            }
            uint hcsparams = *(uint*)(bar0 + 0x04);
            AvailablePorts = (byte)(hcsparams & 0xF);
            Console.WriteLine($"{AvailablePorts} Ports available");
            uint hccparams = *(uint*)(bar0 + 0x08);
            byte cap = (byte)((hccparams & 0xFF00) >> 8);
            if (cap)
            {
                uint eecp = (hccparams & (255 << 8)) >> 8;
                if (eecp >= 0x40)
                {
                    Console.WriteLine("Disabling BIOS EHCI Hand-off");
                    uint legsup = PCI.ReadRegister32(device.Bus, device.Slot, device.Function, (byte)eecp);

                    if (legsup & 0x00010000)
                    {
                        PCI.WriteRegister32(device.Bus, device.Slot, device.Function, (byte)eecp, legsup | 0x01000000);
                        for (; ; )
                        {
                            Console.WriteLine("Waitting for BIOS ready");
                            legsup = PCI.ReadRegister32(device.Bus, device.Slot, device.Function, (byte)eecp);
                            if ((~legsup & 0x00010000) != 0 && (legsup & 0x01000000) != 0)
                            {
                                break;
                            }
                        }
                    }
                }

                CMDReg = BaseAddr + 0x00;
                AsyncListReg = BaseAddr + 0x18;

                uint default_cmd = *(uint*)CMDReg;
                if (default_cmd & 1)
                {
                    Console.WriteLine("Stopping this controller");
                    *(uint*)CMDReg &= ~1u;
                    while (1)
                    {
                        if ((*(uint*)CMDReg & 1) == 0)
                        {
                            break;
                        }
                    }
                }

                *(uint*)CMDReg |= 2;
                while (1)
                {
                    Console.WriteLine("Waitting for controller ready");
                    if ((*(uint*)CMDReg & 2) == 0)
                    {
                        break;
                    }
                }

                uint* framelist = (uint*)Allocator.Allocate(FrameSize * sizeof(uint));

                for (int i = 0; i < FrameSize; i++)
                {
                    framelist[i] |= 1;
                }

                *(uint*)(BaseAddr + 0x08) = 0;

                *(uint*)(BaseAddr + 0x10) = 0;
                *(uint*)(BaseAddr + 0x14) = (uint)&framelist;
                *(uint*)CMDReg |= 0x40 << 16;
                *(uint*)CMDReg |= 0 << 2;
                *(uint*)CMDReg |= 1;
                *(uint*)(BaseAddr + 0x40) |= 1;

                ScanPorts();
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EndPoint
        {
            public byte Length;
            public byte DescriptorType;
            public byte EndpointAddress;
            public byte Attributes;
            public ushort MaxPacketSize;
            public byte Interval;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ConfigDesc
        {
            public byte Length;
            public byte DescriptorType;

            public ushort TotalLength;
            public byte NumInterfaces;
            public byte ConfigurationValue;
            public byte Configuration;
            public byte Attributes;
            public byte MaxPower;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct InterfaceDesc
        {
            public byte Length;
            public byte DescriptorType;

            public byte InterfaceNumber;
            public byte AlternateSetting;
            public byte NumEndpoints;
            public byte InterfaceClass;
            public byte InterfaceSubClass;
            public byte InterfaceProtocol;
            public byte Interface;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TD
        {
            public uint NextLink;
            public uint AltLink;
            public uint Token;
            public fixed uint Buffer[5];
            public fixed uint ExtendedBuffer[5];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct QH
        {
            public uint HorizontalLink;

            public uint Characteristics;
            public uint Capabilities;

            public uint CurrentLink;
            public uint NextLink;
            public uint AltLink;

            public uint Token;
            public fixed uint Buffer[5];
            public fixed uint ExtendedBuffer[5];
        }

        public static byte* SendAndReceive(byte port, USBRequest* cmd, void* buffer)
        {
            QH* qh = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            QH* qh1 = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            QH* qh2 = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            TD* td = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* trans = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* sts = (TD*)Allocator.Allocate((ulong)sizeof(TD));

            td->Token |= 2 << 8;
            td->Token |= 3 << 10;
            td->Token |= 8 << 16;
            td->Token |= 1 << 7;
            td->NextLink = cmd->Length ? (uint)trans : (uint)sts;
            td->AltLink = 1;
            *td->Buffer = (uint)cmd;

            if (cmd->Length)
            {
                trans->NextLink = (uint)sts;
                trans->AltLink = 1;
                trans->Token |= (uint)(cmd->Length << 16);
                trans->Token |= 1u << 31;
                trans->Token |= 1 << 7;
                trans->Token |= 1 << 8;
                trans->Token |= 0x3 << 10;
                *trans->Buffer = (uint)buffer;
            }

            sts->NextLink = 1;
            sts->AltLink = 1;
            sts->Token |= 0 << 8;
            sts->Token |= 1u << 31;
            sts->Token |= 1 << 7;
            sts->Token |= 0x3 << 10;


            qh1->AltLink = 1;
            qh1->NextLink = (uint)td;
            qh1->HorizontalLink = ((uint)qh) | 2;
            qh1->CurrentLink = (uint)qh2;
            qh1->Characteristics |= 1 << 14;
            qh1->Characteristics |= 64 << 16;
            qh1->Characteristics |= 2 << 12;
            qh1->Characteristics |= port;
            qh1->Capabilities = 0x40000000;

            qh->AltLink = 1;
            qh->NextLink = 1;
            qh->HorizontalLink = ((uint)qh1) | 2;
            qh->CurrentLink = 0;
            qh->Characteristics = 1 << 15;
            qh->Token = 0x40;

            *(uint*)AsyncListReg = (uint)qh;
            *(uint*)CMDReg |= 0x20;

            byte res = WaitForComplete(sts);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;

            Allocator.Free((System.IntPtr)qh);
            Allocator.Free((System.IntPtr)qh1);
            Allocator.Free((System.IntPtr)qh2);
            Allocator.Free((System.IntPtr)td);
            Allocator.Free((System.IntPtr)trans);
            Allocator.Free((System.IntPtr)sts);

            if (res == 0)
            {
                return (byte*)USB.TransmitError;
            }

            return (byte*)buffer;
        }

        static byte SetDeviceAddr(byte addr)
        {
            USBRequest* cmd = (USBRequest*)Allocator.Allocate((ulong)sizeof(USBRequest));
            cmd->Request = 0x05;
            cmd->RequestType |= 0;
            cmd->RequestType |= 0 << 5;
            cmd->RequestType |= 0;
            cmd->Index = 0;
            cmd->Length = 0;
            cmd->Value = addr;

            TD* tcmd = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* sts = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            QH* qh = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            QH* qh1 = (QH*)Allocator.Allocate((ulong)sizeof(QH));

            tcmd->NextLink = (uint)sts;
            tcmd->AltLink = 1;
            tcmd->Token |= 8 << 16;
            tcmd->Token |= 1 << 7;
            tcmd->Token |= 0x2 << 8;
            tcmd->Token |= 0x3 << 10;
            *tcmd->Buffer = (uint)cmd;

            sts->NextLink = 1;
            sts->AltLink = 1;
            sts->Token |= 1 << 8;
            sts->Token |= 1u << 31;
            sts->Token |= 1 << 7;
            sts->Token |= 0x3 << 10;

            qh1->AltLink = 1;
            qh1->NextLink = (uint)tcmd;
            qh1->HorizontalLink = ((uint)qh) | 2;
            qh1->CurrentLink = 0;
            qh1->Characteristics |= 1 << 14;
            qh1->Characteristics |= 64 << 16;
            qh1->Characteristics |= 2 << 12;
            qh1->Capabilities = 0x40000000;

            qh->AltLink = 1;
            qh->NextLink = 1;
            qh->HorizontalLink = ((uint)qh1) | 2;
            qh->CurrentLink = 0;
            qh->Characteristics = 1 << 15;
            qh->Token = 0x40;

            *(uint*)AsyncListReg = (uint)qh;
            *(uint*)CMDReg |= 0x20;

            byte lsts = WaitForComplete(sts);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;
            return lsts;
        }

        static byte WaitForComplete(TD* status)
        {
            Native.Hlt();

            byte lsts = 1;

            ulong lastTick = Timer.Ticks;

            while (1)
            {
                uint tsts = status->Token;

                if (
                    !(tsts & (1 << 4)) &&
                    !(tsts & (1 << 3)) &&
                    !(tsts & (1 << 6)) &&
                    !(tsts & (1 << 5)) &&
                    !(tsts & (1 << 7)) &&
                    //500ms
                    !(Timer.Ticks > (lastTick + 500))
                    )
                {
                    lsts = 1;
                    break;
                }
                else
                {
                    lsts = 0;
                    break;
                }
            }
            if (lsts == 0)
            {
                Console.WriteLine("Transmission failed");
            }
            return lsts;
        }

        static byte* GetDesc(byte addr, byte size)
        {
            QH* qh = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            QH* qh1 = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            QH* qh2 = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            TD* td = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* trans = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* status = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            USBRequest* cmd = (USBRequest*)Allocator.Allocate((ulong)sizeof(USBRequest));

            byte* buffer = (byte*)Allocator.Allocate(size);

            cmd->Request = 0x06;
            cmd->RequestType |= 0x80;
            cmd->Index = 0;
            cmd->Length = size;
            cmd->Value = 1 << 8;

            td->Token |= 2 << 8;
            td->Token |= 3 << 10;
            td->Token |= (uint)(size << 16);
            td->Token |= 1 << 7;
            td->NextLink = (uint)trans;
            td->AltLink = 1;
            *td->Buffer = (uint)cmd;

            trans->NextLink = (uint)status;
            trans->AltLink = 1;
            trans->Token |= (uint)(size << 16);
            trans->Token |= 1u << 31;
            trans->Token |= 1 << 7;
            trans->Token |= 1 << 8;
            trans->Token |= 0x3 << 10;
            *trans->Buffer = (uint)buffer;

            status->NextLink = 1;
            status->AltLink = 1;
            status->Token |= 0 << 8;
            status->Token |= 1u << 31;
            status->Token |= 1 << 7;
            status->Token |= 0x3 << 10;


            qh1->AltLink = 1;
            qh1->NextLink = (uint)td;
            qh1->HorizontalLink = ((uint)qh) | 2;
            qh1->CurrentLink = (uint)qh2;
            qh1->Characteristics |= 1 << 14;
            qh1->Characteristics |= 64 << 16;
            qh1->Characteristics |= 2 << 12;
            qh1->Characteristics |= addr;
            qh1->Capabilities = 0x40000000;

            qh->AltLink = 1;
            qh->NextLink = 1;
            qh->HorizontalLink = ((uint)qh1) | 2;
            qh->CurrentLink = 0;
            qh->Characteristics = 1 << 15;
            qh->Token = 0x40;

            *(uint*)AsyncListReg = (uint)qh;
            *(uint*)CMDReg |= 0x20;

            byte result = WaitForComplete(status);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;
            if (result == 0)
            {
                return null;
            }

            return buffer;
        }

        static byte* GetConfig(byte addr, byte size)
        {
            QH* qh = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            QH* qh1 = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            QH* qh2 = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            TD* td = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* trans1 = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* trans2 = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* status = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            USBRequest* cmd = (USBRequest*)Allocator.Allocate((ulong)sizeof(USBRequest));

            byte* buffer = (byte*)Allocator.Allocate(size);

            cmd->Request = 0x06;
            cmd->RequestType |= 0x80;
            cmd->Index = 0;
            cmd->Length = size;
            cmd->Value = 2 << 8;

            td->Token |= 2 << 8;
            td->Token |= 3 << 10;
            td->Token |= 8 << 16;
            td->Token |= 1 << 7;
            td->NextLink = (uint)trans1;
            td->AltLink = 1;
            *td->Buffer = (uint)cmd;
            byte toggle = 0;
            toggle ^= 1;

            trans1->NextLink = (uint)status;
            trans1->AltLink = 1;
            trans1->Token |= (uint)(size << 16);
            trans1->Token |= (uint)(toggle << 31);
            trans1->Token |= 1 << 7;
            trans1->Token |= 1 << 8;
            trans1->Token |= 0x3 << 10;
            *trans1->Buffer = (uint)buffer;

            toggle ^= 1;

            trans2->NextLink = (uint)status;
            trans2->AltLink = 1;
            trans2->Token |= (uint)(size << 16);
            trans2->Token |= (uint)(toggle << 31);
            trans2->Token |= 1 << 7;
            trans2->Token |= 1 << 8;
            trans2->Token |= 0x3 << 10;
            *trans2->Buffer = ((uint)buffer) + 8;

            status->NextLink = 1;
            status->AltLink = 1;
            status->Token |= 0 << 8;
            status->Token |= 1u << 31;
            status->Token |= 1 << 7;
            status->Token |= 0x3 << 10;


            qh1->AltLink = 1;
            qh1->NextLink = (uint)td;
            qh1->HorizontalLink = ((uint)qh) | 2;
            qh1->CurrentLink = (uint)qh2;
            qh1->Characteristics |= 1 << 14;
            qh1->Characteristics |= 64 << 16;
            qh1->Characteristics |= 2 << 12;
            qh1->Characteristics |= addr;
            qh1->Capabilities = 0x40000000;

            qh->AltLink = 1;
            qh->NextLink = 1;
            qh->HorizontalLink = ((uint)qh1) | 2;
            qh->CurrentLink = 0;
            qh->Characteristics = 1 << 15;
            qh->Token = 0x40;

            *(uint*)AsyncListReg = (uint)qh;
            *(uint*)CMDReg |= 0x20;

            byte res = WaitForComplete(status);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;
            if (res == 0)
            {
                return null;
            }

            return buffer;
        }

        static byte SetConfig(byte addr, byte config)
        {
            USBRequest* cmd = (USBRequest*)Allocator.Allocate((ulong)sizeof(USBRequest));
            cmd->Request = 0x09;
            cmd->RequestType |= 0;
            cmd->RequestType |= 0 << 5;
            cmd->RequestType |= 0;
            cmd->Index = 0;
            cmd->Length = 0;
            cmd->Value = config;

            TD* tcmd = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            TD* status = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            QH* head = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            QH* head1 = (QH*)Allocator.Allocate((ulong)sizeof(QH));

            tcmd->NextLink = (uint)status;
            tcmd->AltLink = 1;
            tcmd->Token |= 8 << 16;
            tcmd->Token |= 1 << 7;
            tcmd->Token |= 0x2 << 8;
            tcmd->Token |= 0x3 << 10;
            *tcmd->Buffer = (uint)cmd;

            status->NextLink = 1;
            status->AltLink = 1;
            status->Token |= 1 << 8;
            status->Token |= 1u << 31;
            status->Token |= 1 << 7;
            status->Token |= 0x3 << 10;

            head1->AltLink = 1;
            head1->NextLink = (uint)tcmd;
            head1->HorizontalLink = ((uint)head) | 2;
            head1->CurrentLink = 0;
            head1->Characteristics |= 1 << 14;
            head1->Characteristics |= 64 << 16;
            head1->Characteristics |= 2 << 12;
            head1->Characteristics |= addr;
            head1->Capabilities = 0x40000000;

            head->AltLink = 1;
            head->NextLink = 1;
            head->HorizontalLink = ((uint)head1) | 2;
            head->CurrentLink = 0;
            head->Characteristics = 1 << 15;
            head->Token = 0x40;

            *(uint*)AsyncListReg = (uint)head;
            *(uint*)CMDReg |= 0x20;

            byte lstatus = WaitForComplete(status);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;
            return lstatus;
        }

        static void InitPort(int port)
        {
            USBDevice device = new USBDevice();
            device.USBVersion = 2;
            uint reg_port = (uint)(BaseAddr + 0x44 + (port * 4));
            uint portinfo = *(uint*)reg_port;

            *(uint*)reg_port |= 0x100;
            Timer.Wait(60);
            *(uint*)reg_port &= ~0x100u;
            Timer.Wait(20);
            portinfo = *(uint*)reg_port;

            if ((portinfo & 4) == 0)
            {
                Console.WriteLine($"Port {port} Is not enabled");
                return;
            }

            byte addr = SetDeviceAddr(DeviceAddr);
            if (addr == 0)
            {
                Console.WriteLine($"Port {port} Failed to set device address");
                return;
            }
            device.AssignedSloth = DeviceAddr;
            device.NumPort = DeviceAddr;

            byte* _desc = GetDesc(DeviceAddr, 8);
            if (_desc == 0)
            {
                Console.WriteLine($"Port {port} Failed to get descriptor");
                return;
            }

            if (!(_desc[0] == 0x12 && _desc[1] == 0x1))
            {
                Console.WriteLine($"Port {port} Invalid magic number");
                return;
            }
            byte max_packet_size = _desc[7];
            Console.WriteLine($"Port {port} Max Packet Size {max_packet_size}");

            byte Class = _desc[4];
            byte SubClass = 0;
            byte Protocol = 0;
            if (Class == 0x00)
            {
                ConfigDesc* cdesc = (ConfigDesc*)GetConfig(DeviceAddr, (byte)(sizeof(InterfaceDesc) + sizeof(ConfigDesc) + (sizeof(EndPoint) * 2)));
                if (cdesc == 0)
                {
                    Console.WriteLine($"[ECHI] Port {port} Failed to get descriptor");
                    return;
                }
                InterfaceDesc* idesc = (InterfaceDesc*)(((uint)cdesc) + sizeof(ConfigDesc));
                Class = idesc->InterfaceClass;
                SubClass = idesc->InterfaceSubClass;
                Protocol = idesc->InterfaceProtocol;
                device.Class = Class;
                device.SubClass = SubClass;
                device.Protocol = Protocol;

                if (idesc->NumEndpoints == 2)
                {
                    EndPoint* ep = (EndPoint*)(((uint)cdesc) + sizeof(ConfigDesc) + sizeof(InterfaceDesc));
                    EndPoint* ep1 = (EndPoint*)(((uint)cdesc) + sizeof(ConfigDesc) + sizeof(InterfaceDesc) + 7);
                    device.EndpointIn = (uint)(ep->EndpointAddress & 0x80 ? ep->EndpointAddress & 0xF : ep1->EndpointAddress & 0xF);
                    device.EndpointOut = (uint)((ep->EndpointAddress & 0x80) == 0 ? ep->EndpointAddress & 0xF : ep1->EndpointAddress & 0xF);
                }
                else if (idesc->NumEndpoints == 1)
                {
                    EndPoint* ep = (EndPoint*)(((uint)cdesc) + sizeof(ConfigDesc) + sizeof(InterfaceDesc));
                    device.EndpointIn = (uint)(ep->EndpointAddress & 0xF);
                }
            }

            if (Class == 0x00)
            {
                return;
            }
            Console.WriteLine($"Port{port} Class: {Class}");

            byte config_res = SetConfig(DeviceAddr, 1);
            if (config_res == 0)
            {
                Console.WriteLine($"Port {port} failed to set configuration");
                return;
            }

            DeviceAddr++;

            USB.DriveDevice(device);
        }


        public static void ScanPorts()
        {
            DeviceAddr = 1;

            for (int i = 0; i < AvailablePorts; i++)
            {
                uint reg_port = (uint)(BaseAddr + 0x44 + (i * 4));
                if (*(uint*)reg_port & 3)
                {
                    Console.WriteLine($"Port {i} Present");
                    InitPort(i);
                }
            }
        }
    }
}
