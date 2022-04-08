/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Kernel.FS;
using System.Collections.Generic;

namespace Kernel.Driver
{
    public unsafe class IDE : Disk
    {
        public const byte ReadSectorsWithRetry = 0x20;
        public const byte WriteSectorsWithRetry = 0x30;
        public const byte IdentifyDrive = 0xEC;
        public const byte CacheFlush = 0xE7;

        public const byte Busy = 1 << 7;
        public const byte DataRequest = 1 << 3;
        public const byte Error = 1 << 0;

        public const uint ModelNumber = 0x1B * 2;
        public const uint MaxLBA28 = 60 * 2;

        public const uint DrivesPerConroller = 2;

        ushort DataPort;
        ushort FeaturePort;
        ushort ErrorPort;
        ushort SectorCountPort;

        public static List<IDE> Controllers;

        public static void Initialize()
        {
            Controllers = new List<IDE>(2);
            IDE primary = new IDE(Channels.Primary);
            IDE secondary = new IDE(Channels.Secondary);
            Controllers.Add(primary);
            Controllers.Add(secondary);
            if(primary.Available() || secondary.Available())
            Console.WriteLine("[IDE] IDE controller Initialized");
        }

        ushort LBALowPort;
        ushort LBAMidPort;
        ushort LBAHighPort;
        ushort DeviceHeadPort;
        ushort StatusPort;
        ushort CommandPort;
        ushort AltStatusPort;

        private struct DriveInfo
        {
            public bool Present;
            public ulong Size;
        }

        private DriveInfo[] Ports;

        public enum Drive
        {
            Master = 0,
            Slave = 1
        }

        public const uint SectorSize = 512;

        public enum Channels
        {
            Primary,
            Secondary
        }

        public IDE(Channels index)
        {
            ushort BasePort = 0, ControlPort = 0;

            switch (index)
            {
                case Channels.Primary:
                    BasePort = 0x1F0;
                    ControlPort = 0x3F6;
                    break;
                case Channels.Secondary:
                    BasePort = 0x170;
                    ControlPort = 0x376;
                    break;
            }

            DataPort = (ushort)(BasePort + 0);
            ErrorPort = (ushort)(BasePort + 1);
            FeaturePort = (ushort)(BasePort + 1);
            SectorCountPort = (ushort)(BasePort + 2);
            LBALowPort = (ushort)(BasePort + 3);
            LBAMidPort = (ushort)(BasePort + 4);
            LBAHighPort = (ushort)(BasePort + 5);
            DeviceHeadPort = (ushort)(BasePort + 6);
            CommandPort = (ushort)(BasePort + 7);
            StatusPort = (ushort)(BasePort + 7);
            AltStatusPort = (ushort)(ControlPort + 6);

            Ports = new DriveInfo[DrivesPerConroller];

            for (var drive = 0; drive < DrivesPerConroller; drive++)
            {
                Ports[drive].Present = false;
                Ports[drive].Size = 0;
            }

            if (!Available()) return;

            //Start Device
            Native.Out8(ControlPort, 0);

            for (byte port = 0; port < 2; port++)
            {

                Ports[port].Present = false;

                Native.Out8(DeviceHeadPort, (byte)((port == 0) ? 0xA0 : 0xB0));
                Native.Out8(SectorCountPort, 0);
                Native.Out8(LBALowPort, 0);
                Native.Out8(LBAMidPort, 0);
                Native.Out8(LBAHighPort, 0);
                Native.Out8(CommandPort, IdentifyDrive);

                if (
                    Native.In8(StatusPort) == 0 ||
                    !WaitForReadyStatus() ||
                    Native.In8(LBAMidPort) != 0 && Native.In8(LBAHighPort) != 0 || //ATAPI
                    !WaitForIdentifyData()
                    )
                {
                    continue;
                }
                Ports[port].Present = true;

                var DriveInfo = new byte[4096];
                fixed (byte* p = DriveInfo)
                {
                    Native.Insw(DataPort, (ushort*)p, (ulong)(DriveInfo.Length / 2));

                    Ports[port].Size = (*(uint*)(p + MaxLBA28)) * SectorSize;

                    Console.Write("[IDE] ");
                    byte* pName = ((byte*)p) + ModelNumber;
                    for (int i = 0; i < 40; i++)
                    {
                        Console.Write((char)pName[i]);
                        if (pName[i + 1] == 0x20 && pName[i + 2] == 0x20) break;
                    }
                    Console.Write(' ');

                    Console.Write("Size: ");
                    Console.Write((Ports[port].Size / (1024 * 1024)).ToString());
                    Console.WriteLine("MiB");
                }
                DriveInfo.Dispose();
            }
        }

        public bool Available()
        {
            Native.Out8(LBALowPort, 0x88);
            return Native.In8(LBALowPort) == 0x88;
        }

        private bool WaitForReadyStatus()
        {
            byte status;
            do
            {
                status = Native.In8(StatusPort);
            }
            while ((status & Busy) == Busy);

            return true;
        }

        private bool WaitForIdentifyData()
        {
            byte status;
            do
            {
                status = Native.In8(StatusPort);
            }
            while ((status & DataRequest) != DataRequest && (status & Error) != Error);
            return ((status & Error) != Error);
        }

        public bool ReadOrWrite(Drive adrive, uint sector, byte* data, bool write)
        {
            uint drive = (uint)adrive;
            if (drive >= 2 || !Ports[drive].Present)
                return false;

            Native.Out8(DeviceHeadPort, (byte)(0xE0 | (drive << 4) | ((sector >> 24) & 0x0F)));
            //Native.Out8(FeaturePort, 0);
            Native.Out8(SectorCountPort, 1);
            Native.Out8(LBAHighPort, (byte)((sector >> 16) & 0xFF));
            Native.Out8(LBAMidPort, (byte)((sector >> 8) & 0xFF));
            Native.Out8(LBALowPort, (byte)(sector & 0xFF));

            Native.Out8(CommandPort, (write) ? WriteSectorsWithRetry : ReadSectorsWithRetry);

            if (!WaitForReadyStatus())
                return false;

            while ((Native.In8(StatusPort) & 0x80) != 0) ;

            if (write)
            {
                Native.Outsw(DataPort, (ushort*)data, SectorSize / 2);

                Native.Out8(CommandPort, CacheFlush);

                WaitForReadyStatus();
            }
            else
            {
                Native.Insw(DataPort, (ushort*)data, SectorSize / 2);
            }

            if ((Native.In8(StatusPort) & 0x1) != 0)
            {
                Console.WriteLine($"IDE bad status");
                return false;
            }

            return true;
        }

        public override bool Read(ulong sector, uint count, byte* p) 
        {
            bool b = false;
            for (int i = 0; i < (count * 512); i += 512)
            {
                b = ReadOrWrite(Drive.Master, (uint)sector, p + i, false);
                sector++;
            }
            return b;
        }

        public override bool Write(ulong sector, uint count, byte* p)
        {
            bool b = false;
            for (int i = 0; i < (count*512); i += 512)
            {
                b = ReadOrWrite(Drive.Master, (uint)sector, p + i, true);
                sector++;
            }
            return b;
        }
    }
}
