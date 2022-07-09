using MOOS.Driver;

namespace MOOS.Misc
{
    public class USBDevice
    {
        public byte USBVersion;

        public byte NumPort;

        public uint Ring;
        public uint RingOffset;

        public int AssignedSloth;

        public byte Class;
        public byte SubClass;
        public byte Protocol;

        public uint SendMessage;
        public uint SendBulk;
        public uint RecieveBulk;
        public byte EndpointControl;
        public uint EndpointIn;
        public uint Localinringoffset;
        public uint EndpointOut;
        public uint Localoutringoffset;
    }

    public static unsafe class USB
    {
        public const uint TransmitError = 0xFFFFFFFF;

        public static void* SendAndReceive(USBDevice device, void* cmd, void* buffer)
        {
            if (device.USBVersion == 2)
            {
                return EHCI.SendAndReceive(device.NumPort, (EHCI.CMD*)cmd, buffer);
            }
            else
            {
                return (void*)USB.TransmitError;
            }
        }

        public static void DriveDevice(USBDevice device)
        {
            switch (device.Class)
            {
                case 3:
                    HID.Initialize(device);
                    break;

            }
        }
    }
}
