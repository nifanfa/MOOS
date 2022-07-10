using MOOS.Misc;
using System;
using static System.ConsoleKey;
using System.Drawing;
using System.Windows.Forms;

namespace MOOS.Driver
{
    public static unsafe class HID
    {
        public static void Initialize() 
        {
            Mouse = null;
            Keyboard = null;

            cmd = (USBRequest*)Allocator.Allocate((ulong)sizeof(USBRequest));
        }

        static USBRequest* cmd;

        public static byte GetHIDPacket(USBDevice device, uint devicedesc)
        {
            (*cmd).Clean();
            cmd->Request = 1;
            cmd->RequestType = 0xA1;
            cmd->Index = 0;
            cmd->Length = 3;
            cmd->Value = 0x0100;
            byte* res = (byte*)USB.SendAndReceive(device, cmd, (void*)devicedesc);
            return *res;
        }

        public static void GetKeyboardThings(USBDevice device, out byte ScanCode, out ConsoleKey Key)
        {
            Key = None;
            ScanCode = 0;

            byte* desc = stackalloc byte[10];
            byte res = GetHIDPacket(device, (uint)desc);
            if (res != (USB.TransmitError & 0xFF))
            {
                if (desc[2] != 0)
                {
                    ScanCode = desc[2];
                    if(ScanCode < ConsoleKeys.Length)
                    {
                        Key = ConsoleKeys[ScanCode];
                    }
                }
            }
        }

        public static void GetMouseThings(USBDevice device,out sbyte AxisX,out sbyte AxisY,out MouseButtons buttons)
        {
            AxisX = 0;
            AxisY = 0;
            buttons = MouseButtons.None;

            byte* desc = stackalloc byte[10];
            byte res = GetHIDPacket(device, (uint)desc);
            if (res != (USB.TransmitError & 0xFF))
            {
                AxisX = (sbyte)desc[1];
                AxisY = (sbyte)desc[2];

                if (desc[0] & 0x01) buttons |= MouseButtons.Left;
                if (desc[0] & 0x02) buttons |= MouseButtons.Right;
                if (desc[0] & 0x04) buttons |= MouseButtons.Middle;
            }
        }

        public static void Initialize(USBDevice device)
        {
            (*cmd).Clean();
            cmd->Request = 0x0B;
            cmd->RequestType = 0x21;
            cmd->Index = 0;
            cmd->Length = 0;
            cmd->Value = 0;
            byte* res = (byte*)USB.SendAndReceive(device, cmd, null);
            if ((uint)res == USB.TransmitError)
            {
                Console.WriteLine("Unable to set protocol");
                return;
            }

            if (device.Protocol == 1)
            {
                Console.WriteLine($"USB Keyboard at port:{device.NumPort}");
                InitializeKeyboard(device);
            }
            else if (device.Protocol == 2)
            {
                Console.WriteLine($"USB Mouse at port:{device.NumPort}");
                InitializeMouse(device);
            }
        }

        public static ConsoleKey[] ConsoleKeys;

        public static USBDevice Mouse;
        public static USBDevice Keyboard;

        static void InitializeMouse(USBDevice device)
        {
            Mouse = device;
        }

        static void InitializeKeyboard(USBDevice device)
        {
            Keyboard = device;
            ConsoleKeys = new ConsoleKey[]
            {
                None,
                None,
                None,
                None,
                A,
                B,
                C,
                D,
                E,
                F,
                G,
                H,
                I,
                J,
                K,
                L,
                M,
                N,
                O,
                P,
                Q,
                R,
                S,
                T,
                U,
                V,
                W,
                X,
                Y,
                Z,
                D1,
                D2,
                D3,
                D4,
                D5,
                D6,
                D7,
                D8,
                D9,
                D0,
                Enter,
                Escape,
                Backspace,
                Tab,
                Space,
            };
        }
    }
}
