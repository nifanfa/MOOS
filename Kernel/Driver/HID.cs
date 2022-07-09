using MOOS.Misc;

namespace MOOS.Driver
{
    public static unsafe class HID
    {
        public static byte GetHIDPacket(USBDevice device, uint devicedesc)
        {
            EHCI.CMD* cmd = (EHCI.CMD*)Allocator.Allocate((ulong)sizeof(EHCI.CMD));
            cmd->Request = 1;
            cmd->RequestType = 0xA1;
            cmd->Index = 0;
            cmd->Length = 3;
            cmd->Value = 0x0100;
            byte* res = (byte*)USB.SendAndReceive(device, cmd, (void*)devicedesc);
            Allocator.Free((System.IntPtr)cmd);
            return *res;
        }

        public static byte GetKeyboardKey(USBDevice device)
        {
            byte* desc = stackalloc byte[10];
            byte res = GetHIDPacket(device, (uint)desc);
            if (res != (USB.TransmitError & 0xFF))
            {
                if (desc[2] != 0)
                {
                    byte scancode = desc[2];
                    return scancode;
                }
            }
            return 0;
        }

        public static void Initialize(USBDevice device)
        {
            EHCI.CMD* cmd = (EHCI.CMD*)Allocator.Allocate((ulong)sizeof(EHCI.CMD));
            cmd->Request = 0x0B;
            cmd->RequestType = 0x21;
            cmd->Index = 0;
            cmd->Length = 0;
            cmd->Value = 0;
            byte* res = (byte*)USB.SendAndReceive(device, cmd, null);
            Allocator.Free((System.IntPtr)cmd);
            if ((uint)res == USB.TransmitError)
            {
                Console.WriteLine("Unable to set protocol");
                return;
            }

            if (device.Protocol == 1)
            {
                Console.WriteLine($"Port {device.NumPort} is a keyboard");
                InitializeKeyboard(device);
            }
            else if (device.Protocol == 2)
            {
                Console.WriteLine($"Port {device.NumPort} is a mouse");
            }
        }

        public static USBDevice Keyboard;

        static void InitializeKeyboard(USBDevice device)
        {
            Keyboard = device;

            return;
        }
    }
}
