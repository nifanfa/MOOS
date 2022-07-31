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

        public const int FrameSize = 1024;

        public static void Initialize()
        {
            qh = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            qh1 = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            qh2 = (QH*)Allocator.Allocate((ulong)sizeof(QH));
            td = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            trans = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            trans1 = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            sts = (TD*)Allocator.Allocate((ulong)sizeof(TD));
            cmd = (USBRequest*)Allocator.Allocate((ulong)sizeof(USBRequest));

            PCIDevice device = PCI.GetDevice(0x0C, 0x03, 0x20);
            if (device == null) return;

            Console.WriteLine("[EHCI] EHCI controller found!");

            device.WriteRegister(0x04, 0x04 | 0x02 | 0x01);

            uint bar0 = device.Bar0;
            Console.WriteLine($"[EHCI] Bar0: {bar0.ToString("x2")}");
            BaseAddr = bar0 + *(byte*)bar0;
            ushort ver = *(ushort*)(bar0 + 0x02);
            if (ver != 0x100)
            {
                Panic.Error("This controller is not supported!");
                return;
            }
            uint hcsparams = *(uint*)(bar0 + 0x04);
            AvailablePorts = (byte)(hcsparams & 0xF);
            Console.WriteLine($"[EHCI] {AvailablePorts} Ports available");
            uint hccparams = *(uint*)(bar0 + 0x08);

            uint eecp = (hccparams & (255 << 8)) >> 8;
            if (eecp >= 0x40)
            {
                Console.WriteLine("[EHCI] Disabling BIOS EHCI Hand-off");
                uint legsup = PCI.ReadRegister32(device.Bus, device.Slot, device.Function, (byte)eecp);

                if (legsup & 0x00010000)
                {
                    PCI.WriteRegister32(device.Bus, device.Slot, device.Function, (byte)eecp, legsup | 0x01000000);
                    for (; ; )
                    {
                        Console.WriteLine("[EHCI] Waitting for BIOS ready");
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
                Console.WriteLine("[EHCI] Stopping this controller");
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
                Console.WriteLine("[EHCI] Waitting for controller ready");
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
            *(uint*)CMDReg |= 0x400001;
            *(uint*)(BaseAddr + 0x40) |= 1;

            ScanPorts();

            Console.WriteLine("[EHCI] EHCI controller initialized");
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

            public void Clean()
            {
                fixed (void* p = &this)
                    Native.Stosb(p, 0, (ulong)sizeof(TD));
            }
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

            public void Clean()
            {
                fixed (void* p = &this)
                    Native.Stosb(p, 0, (ulong)sizeof(QH));
            }
        }

        static QH* qh;
        static QH* qh1;
        static QH* qh2;
        static TD* td;
        static TD* trans;
        static TD* trans1;
        static TD* sts;
        static USBRequest* cmd;

        public static bool SendAndReceive(byte port, USBRequest* cmd, void* buffer,USBDevice parent, int speed)
        {
            (*qh).Clean();
            (*qh1).Clean();
            (*qh2).Clean();
            (*td).Clean();
            (*trans).Clean();
            (*sts).Clean();

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

            if (speed != 2 && parent != null)
            {
                qh->Capabilities |= (uint)(parent.Port << 23);
                qh->Capabilities |= (uint)(parent.Address << 16);

                qh1->Capabilities |= (uint)(parent.Port << 23);
                qh1->Capabilities |= (uint)(parent.Address << 16);

                qh2->Capabilities |= (uint)(parent.Port << 23);
                qh2->Capabilities |= (uint)(parent.Address << 16);
            }

            *(uint*)AsyncListReg = (uint)qh;
            *(uint*)CMDReg |= 0x20;

            byte res = WaitForComplete(sts);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;

            if (res == 0)
            {
                return false;
            }

            return true;
        }

        static byte SetDeviceAddr(byte addr, USBDevice parent, int speed)
        {
            (*cmd).Clean();
            (*trans).Clean();
            (*sts).Clean();
            (*qh).Clean();
            (*qh1).Clean();

            cmd->Request = 0x05;
            cmd->RequestType |= 0;
            cmd->RequestType |= 0 << 5;
            cmd->RequestType |= 0;
            cmd->Index = 0;
            cmd->Length = 0;
            cmd->Value = addr;

            trans->NextLink = (uint)sts;
            trans->AltLink = 1;
            trans->Token |= 8 << 16;
            trans->Token |= 1 << 7;
            trans->Token |= 0x2 << 8;
            trans->Token |= 0x3 << 10;
            *trans->Buffer = (uint)cmd;

            sts->NextLink = 1;
            sts->AltLink = 1;
            sts->Token |= 1 << 8;
            sts->Token |= 1u << 31;
            sts->Token |= 1 << 7;
            sts->Token |= 0x3 << 10;

            qh1->AltLink = 1;
            qh1->NextLink = (uint)trans;
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

            if (speed != 2 && parent != null)
            {
                qh->Capabilities |= (uint)(parent.Port << 23);
                qh->Capabilities |= (uint)(parent.Address << 16);

                qh1->Capabilities |= (uint)(parent.Port << 23);
                qh1->Capabilities |= (uint)(parent.Address << 16);

                qh2->Capabilities |= (uint)(parent.Port << 23);
                qh2->Capabilities |= (uint)(parent.Address << 16);
            }

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
                //Console.WriteLine("[EHCI] Transmission failed");
            }
            return lsts;
        }

        static byte* GetDesc(byte addr, byte size, USBDevice parent, int speed)
        {
            (*qh).Clean();
            (*qh1).Clean();
            (*qh2).Clean();
            (*td).Clean();
            (*trans).Clean();
            (*sts).Clean();
            (*cmd).Clean();

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

            trans->NextLink = (uint)sts;
            trans->AltLink = 1;
            trans->Token |= (uint)(size << 16);
            trans->Token |= 1u << 31;
            trans->Token |= 1 << 7;
            trans->Token |= 1 << 8;
            trans->Token |= 0x3 << 10;
            *trans->Buffer = (uint)buffer;

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
            qh1->Characteristics |= addr;
            qh1->Capabilities = 0x40000000;

            qh->AltLink = 1;
            qh->NextLink = 1;
            qh->HorizontalLink = ((uint)qh1) | 2;
            qh->CurrentLink = 0;
            qh->Characteristics = 1 << 15;
            qh->Token = 0x40;

            if (speed != 2 && parent != null)
            {
                qh->Capabilities |= (uint)(parent.Port << 23);
                qh->Capabilities |= (uint)(parent.Address << 16);

                qh1->Capabilities |= (uint)(parent.Port << 23);
                qh1->Capabilities |= (uint)(parent.Address << 16);

                qh2->Capabilities |= (uint)(parent.Port << 23);
                qh2->Capabilities |= (uint)(parent.Address << 16);
            }

            *(uint*)AsyncListReg = (uint)qh;
            *(uint*)CMDReg |= 0x20;

            byte result = WaitForComplete(sts);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;
            if (result == 0)
            {
                return null;
            }

            return buffer;
        }

        static byte* GetConfig(byte addr, byte size, USBDevice parent, int speed)
        {
            (*qh).Clean();
            (*qh1).Clean();
            (*qh2).Clean();
            (*td).Clean();
            (*trans).Clean();
            (*trans1).Clean();
            (*sts).Clean();
            (*cmd).Clean();

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
            td->NextLink = (uint)trans;
            td->AltLink = 1;
            *td->Buffer = (uint)cmd;
            byte toggle = 0;
            toggle ^= 1;

            trans->NextLink = (uint)sts;
            trans->AltLink = 1;
            trans->Token |= (uint)(size << 16);
            trans->Token |= (uint)(toggle << 31);
            trans->Token |= 1 << 7;
            trans->Token |= 1 << 8;
            trans->Token |= 0x3 << 10;
            *trans->Buffer = (uint)buffer;

            toggle ^= 1;

            trans1->NextLink = (uint)sts;
            trans1->AltLink = 1;
            trans1->Token |= (uint)(size << 16);
            trans1->Token |= (uint)(toggle << 31);
            trans1->Token |= 1 << 7;
            trans1->Token |= 1 << 8;
            trans1->Token |= 0x3 << 10;
            *trans1->Buffer = ((uint)buffer) + 8;

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
            qh1->Characteristics |= addr;
            qh1->Capabilities = 0x40000000;

            qh->AltLink = 1;
            qh->NextLink = 1;
            qh->HorizontalLink = ((uint)qh1) | 2;
            qh->CurrentLink = 0;
            qh->Characteristics = 1 << 15;
            qh->Token = 0x40;

            if (speed != 2 && parent != null)
            {
                qh->Capabilities |= (uint)(parent.Port << 23);
                qh->Capabilities |= (uint)(parent.Address << 16);

                qh1->Capabilities |= (uint)(parent.Port << 23);
                qh1->Capabilities |= (uint)(parent.Address << 16);

                qh2->Capabilities |= (uint)(parent.Port << 23);
                qh2->Capabilities |= (uint)(parent.Address << 16);
            }

            *(uint*)AsyncListReg = (uint)qh;
            *(uint*)CMDReg |= 0x20;

            byte res = WaitForComplete(sts);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;
            if (res == 0)
            {
                return null;
            }

            return buffer;
        }

        static byte SetConfig(byte addr, byte config, USBDevice parent,int speed)
        {
            (*cmd).Clean();
            (*td).Clean();
            (*sts).Clean();
            (*qh).Clean();
            (*qh1).Clean();

            cmd->Request = 0x09;
            cmd->RequestType |= 0;
            cmd->RequestType |= 0 << 5;
            cmd->RequestType |= 0;
            cmd->Index = 0;
            cmd->Length = 0;
            cmd->Value = config;

            td->NextLink = (uint)sts;
            td->AltLink = 1;
            td->Token |= 8 << 16;
            td->Token |= 1 << 7;
            td->Token |= 0x2 << 8;
            td->Token |= 0x3 << 10;
            *td->Buffer = (uint)cmd;

            sts->NextLink = 1;
            sts->AltLink = 1;
            sts->Token |= 1 << 8;
            sts->Token |= 1u << 31;
            sts->Token |= 1 << 7;
            sts->Token |= 0x3 << 10;

            qh1->AltLink = 1;
            qh1->NextLink = (uint)td;
            qh1->HorizontalLink = ((uint)qh) | 2;
            qh1->CurrentLink = 0;
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

            if (speed != 2 && parent != null)
            {
                qh->Capabilities |= (uint)(parent.Port << 23);
                qh->Capabilities |= (uint)(parent.Address << 16);

                qh1->Capabilities |= (uint)(parent.Port << 23);
                qh1->Capabilities |= (uint)(parent.Address << 16);

                qh2->Capabilities |= (uint)(parent.Port << 23);
                qh2->Capabilities |= (uint)(parent.Address << 16);
            }

            *(uint*)AsyncListReg = (uint)qh;
            *(uint*)CMDReg |= 0x20;

            byte lstatus = WaitForComplete(sts);

            *(uint*)CMDReg &= ~0x20u;
            *(uint*)AsyncListReg = 1;
            return lstatus;
        }

        public static bool InitPort(int port,USBDevice parent,int speed)
        {
            USBDevice device = new USBDevice();
            device.USBVersion = 2;
            device.Speed = speed;

            USB.DeviceAddr++;
            Console.WriteLine($"[EHCI] Next device address is {USB.DeviceAddr}");

            if (parent == null) 
            {
                uint reg_port = (uint)(BaseAddr + 0x44 + (port * 4));
                uint portinfo = *(uint*)reg_port;

                *(uint*)reg_port |= 0x100;
                Timer.Sleep(60);
                *(uint*)reg_port &= ~0x100u;
                Timer.Sleep(20);
                portinfo = *(uint*)reg_port;

                if ((portinfo & 4) == 0)
                {
                    Console.WriteLine($"[EHCI] Port {port} Is not enabled");

                    device.Dispose();
                    return false;
                }
            }

            byte addr = SetDeviceAddr(USB.DeviceAddr, parent,device.Speed);
            if (addr == 0)
            {
                Console.WriteLine($"[EHCI] Port {port} Failed to set device address");

                device.Dispose();
                return false;
            }
            device.AssignedSloth = USB.DeviceAddr;
            device.Address = USB.DeviceAddr;
            device.Port = port;

            byte* _desc = GetDesc(USB.DeviceAddr, 8, parent, device.Speed);
            if (_desc == 0)
            {
                Console.WriteLine($"[EHCI] Port {port} Failed to get descriptor");

                device.Dispose();
                return false;
            }

            if (!(_desc[0] == 0x12 && _desc[1] == 0x1))
            {
                Console.WriteLine($"[EHCI] Port {port} Invalid magic number");

                device.Dispose();
                return false;
            }
            byte max_packet_size = _desc[7];
            Console.WriteLine($"[EHCI] Port {port} Max Packet Size {max_packet_size}");

            ConfigDesc* cdesc = (ConfigDesc*)GetConfig(USB.DeviceAddr, (byte)(sizeof(InterfaceDesc) + sizeof(ConfigDesc) + (sizeof(EndPoint) * 2)), parent, device.Speed);
            if (cdesc == 0)
            {
                Console.WriteLine($"[EHCI] [ECHI] Port {port} Failed to get descriptor");

                device.Dispose();
                return false;
            }
            InterfaceDesc* idesc = (InterfaceDesc*)(((uint)cdesc) + sizeof(ConfigDesc));
            byte Class = idesc->InterfaceClass;
            byte SubClass = idesc->InterfaceSubClass;
            byte Protocol = idesc->InterfaceProtocol;
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

            if (Class == 0x00)
            {
                device.Dispose();
                return false;
            }
            Console.WriteLine($"[EHCI] Port{port} Class: {Class}");

            byte config_res = SetConfig(USB.DeviceAddr, 1, parent, device.Speed);
            if (config_res == 0)
            {
                Console.WriteLine($"[EHCI] Port {port} failed to set configuration");

                device.Dispose();
                return false;
            }

            device.Parent = parent;

            USB.DriveDevice(device);

            return true;
        }


        public static void ScanPorts()
        {
            USB.Reset();

            for (int i = 0; i < AvailablePorts; i++)
            {
                uint reg_port = (uint)(BaseAddr + 0x44 + (i * 4));
                Console.WriteLine($"[EHCI] Port {i} {((*(uint*)reg_port & 3)?"Present" : "Not present")}");
                if (*(uint*)reg_port & 3)
                {
                    USB.InitPort(i, null, 2, 2);
                }
            }
        }
    }
}
