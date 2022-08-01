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

        public static bool GetHIDPacket(USBDevice device, uint devicedesc)
        {
            (*cmd).Clean();
            cmd->Request = 1;
            cmd->RequestType = 0xA1;
            cmd->Index = 0;
            cmd->Length = 3;
            cmd->Value = 0x0100;
            bool res = USB.SendAndReceive(device, cmd, (void*)devicedesc, device.Parent);
            return res;
        }

        public static void GetKeyboardThings(USBDevice device, out byte ScanCode, out ConsoleKey Key)
        {
            Key = None;
            ScanCode = 0;

            byte* desc = stackalloc byte[10];
            bool res = GetHIDPacket(device, (uint)desc);
            if (res)
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
            bool res = GetHIDPacket(device, (uint)desc);
            if (res)
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
            if (device.Protocol == 1)
            {
                USB.NumDevice++;

                Console.WriteLine($"[HID] USB Keyboard at port:{device.Address}");
                InitializeKeyboard(device);
            }
            else if (device.Protocol == 2)
            {
                USB.NumDevice++;

                Console.WriteLine($"[HID] USB Mouse at port:{device.Address}");
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
