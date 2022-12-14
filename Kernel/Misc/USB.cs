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

        public int Speed;

        public byte Address;

        public uint Ring;
        public uint RingOffset;

        public int AssignedSloth;

        public byte Class;
        public byte SubClass;
        public byte Protocol;

        public uint EndpointIn;
        public uint EndpointOut;

        public uint Localoutringoffset;
        internal int Port;
        internal USBDevice Parent;
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

        public static bool SendAndReceive(USBDevice device, USBRequest* cmd, void* buffer,USBDevice parent)
        {
            if (device.USBVersion == 2)
            {
                return EHCI.SendAndReceive(device.Address, cmd, buffer, parent,device.Speed);
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
                Keyboard.KeyInfo.KeyState = Key != ConsoleKey.None ? ConsoleKeyState.Pressed : ConsoleKeyState.Released;

                if(Key != ConsoleKey.None)
                {
                    Keyboard.KeyInfo.ScanCode = ScanCode;
                    Keyboard.KeyInfo.Key = Key;
                }

                Keyboard.InvokeOnKeyChanged(Keyboard.KeyInfo);
            }

            if (!VMwareTools.Available && HID.Mouse != null)
            {
                HID.GetMouseThings(HID.Mouse, out sbyte AxisX, out sbyte AxisY, out MouseButtons buttons);

                Control.MousePosition.X = Math.Clamp(Control.MousePosition.X + AxisX, 0, Framebuffer.Width);
                Control.MousePosition.Y = Math.Clamp(Control.MousePosition.Y + AxisY, 0, Framebuffer.Height);

                Control.MouseButtons = buttons;
            }
        }

        public static bool InitPort(int port, USBDevice parent,int version,int speed) 
        {
            if(version == 2)
            {
                EHCI.InitPort(port, parent, speed);
            }
            return false;
        }

        public static void DriveDevice(USBDevice device)
        {
            switch (device.Class)
            {
                case 3:
                    HID.Initialize(device);
                    break;
                case 9:
                    Hub.Initialize(device);
                    break;
                default:
                    Console.WriteLine($"[USB] Unrecognized device class:{device.Class} subClass:{device.SubClass}");
                    break;

            }
        }

        public static void Reset()
        {
            USB.NumDevice = 0;
            USB.DeviceAddr = 0;
        }

        public static void StartPolling()
        {
            new Thread(&LoopPoll).Start();
        }

        static void LoopPoll()
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
        }
    }
}
