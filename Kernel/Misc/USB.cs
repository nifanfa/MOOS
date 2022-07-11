using MOOS.Driver;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct USBRequest
    {
        public byte RequestType;
        public byte Request;

        public ushort Value;
        public ushort Index;
        public ushort Length;

        public void Clean() 
        {
            fixed (void* p = &this)
                Native.Stosb(p, 0, (ulong)sizeof(USBRequest));
        }
    }

    public static unsafe class USB
    {
        public static byte NumDevice;
        public static byte DeviceAddr;

        public static bool SendAndReceive(USBDevice device, USBRequest* cmd, void* buffer)
        {
            if (device.USBVersion == 2)
            {
                return EHCI.SendAndReceive(device.NumPort, cmd, buffer);
            }
            else
            {
                return false;
            }
        }

        public static void OnInterrupt() 
        {
            if(HID.Keyboard != null)
            {
                HID.GetKeyboardThings(HID.Keyboard, out byte ScanCode, out ConsoleKey Key);
                Keyboard.KeyInfo.KeyState = ScanCode >= 4 ? ConsoleKeyState.Pressed : ConsoleKeyState.Released;

                if (Keyboard.KeyInfo.KeyState == ConsoleKeyState.Pressed)
                {
                    Keyboard.KeyInfo.ScanCode = ScanCode;
                    Keyboard.KeyInfo.Key = Key;
                }

                Keyboard.InvokeOnKeyChanged(Keyboard.KeyInfo);
            }

            if(HID.Mouse != null)
            {
                HID.GetMouseThings(HID.Mouse, out sbyte AxisX, out sbyte AxisY, out MouseButtons buttons);

                Control.MousePosition.X = Math.Clamp(Control.MousePosition.X + AxisX, 0, Framebuffer.Width);
                Control.MousePosition.Y = Math.Clamp(Control.MousePosition.Y + AxisY, 0, Framebuffer.Height);

                Control.MouseButtons = buttons;
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

        public static void Initialize()
        {
            USB.NumDevice = 0;
            USB.DeviceAddr = 1;
        }

        public static void StartPoll()
        {
            new Thread(() =>
            {
                for (; ; )
                {
                    if (USB.NumDevice != 0)
                    {
                        USB.OnInterrupt();
                    }
                    else
                    {
                        ThreadPool.Schedule_Next();
                    }
                }
            }).Start();
        }
    }
}
