using MOOS.Misc;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MOOS
{
    public static unsafe class VMwareTools
    {
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct vmware_cmd
        {
            [FieldOffset(0)]
            public uint ax;
            [FieldOffset(0)]
            public uint magic;

            [FieldOffset(4)]
            public uint bx;
            [FieldOffset(4)]
            public uint size;

            [FieldOffset(8)]
            public uint cx;
            [FieldOffset(8)]
            public ushort command;

            [FieldOffset(12)]
            public uint dx;
            [FieldOffset(12)]
            public ushort port;

            [FieldOffset(16)]
            public uint si;
            [FieldOffset(20)]
            public uint di;
        }

        const uint VMWARE_MAGIC = 0x564D5868;
        const ushort CMD_GETVERSION = 10;

        const ushort CMD_ABSPOINTER_DATA = 39;
        const ushort CMD_ABSPOINTER_STATUS = 40;
        const ushort CMD_ABSPOINTER_COMMAND = 41;

        const uint ABSPOINTER_ENABLE = 0x45414552;/* Q E A E */
        const ushort ABSPOINTER_RELATIVE = 0xF5;
        const uint ABSPOINTER_ABSOLUTE = 0x53424152; /* R A B S */

        public static bool Available = false;

        public static void Initialize()
        {
            if (!(Available = is_vmware_backdoor())) return;

            Console.WriteLine("[VMware tools] Initializing VMware tools");

            mouse_absolute();

            Interrupts.EnableInterrupt(0x2c, &vmware_handle_mouse);
        }

        public static void vmware_handle_mouse()
        {
            vmware_cmd cmd;
            /* Read the mouse status */
            cmd.bx = 0;
            cmd.command = CMD_ABSPOINTER_STATUS;
            vmware_send(&cmd);

            /* Mouse status is in EAX */
            if (cmd.ax == 0xFFFF0000)
            {
                /* An error has occured, let's turn the device off and back on */
                //mouse_off();
                mouse_absolute();
                return;
            }

            /* The status command returns a size we need to read, should be at least 4. */
            if ((cmd.ax & 0xFFFF) < 4) return;

            /* Read 4 bytes of mouse data */
            cmd.bx = 4;
            cmd.command = CMD_ABSPOINTER_DATA;
            vmware_send(&cmd);

            /* Mouse data is now stored in AX, BX, CX, and DX */
            uint flags = (cmd.ax & 0xFFFF0000) >> 16; /* Not important */
            uint buttons = cmd.ax & 0xFFFF; /* 0x10 = Right, 0x20 = Left, 0x08 = Middle */
            uint x = cmd.bx; /* Both X and Y are scaled from 0 to 0xFFFF */
            uint y = cmd.cx; /* You should map these somewhere to the actual resolution. */
            byte z = (byte)cmd.dx; /* Z is a single signed byte indicating scroll direction. */

            Control.MouseButtons = MouseButtons.None;
            if (BitHelpers.IsBitSet(buttons, 3)) Control.MouseButtons |= MouseButtons.Middle;
            if (BitHelpers.IsBitSet(buttons, 4)) Control.MouseButtons |= MouseButtons.Right;
            if (BitHelpers.IsBitSet(buttons, 5)) Control.MouseButtons |= MouseButtons.Left;
            //Control.MousePosition.X = (int)(((x * 100) / 65536) * (Framebuffer.Width / 100));
            //Control.MousePosition.Y = (int)(((y * 100) / 65536) * (Framebuffer.Height / 100));
            Control.MousePosition.X = (int)(x / 65536f * Framebuffer.Width);
            Control.MousePosition.Y = (int)(y / 65536f * Framebuffer.Height);

            /* TODO: Do something useful here with these values, such as providing them to userspace! */
        }

        public static void mouse_relative()
        {
            vmware_cmd cmd;
            cmd.bx = ABSPOINTER_RELATIVE;
            cmd.command = CMD_ABSPOINTER_COMMAND;
            vmware_send(&cmd);
        }

        public static void mouse_absolute()
        {
            vmware_cmd cmd;

            /* Enable */
            cmd.bx = ABSPOINTER_ENABLE;
            cmd.command = CMD_ABSPOINTER_COMMAND;
            vmware_send(&cmd);

            /* Status */
            cmd.bx = 0;
            cmd.command = CMD_ABSPOINTER_STATUS;
            vmware_send(&cmd);

            /* Read data (1) */
            cmd.bx = 1;
            cmd.command = CMD_ABSPOINTER_DATA;
            vmware_send(&cmd);

            /* Enable absolute */
            cmd.bx = ABSPOINTER_ABSOLUTE;
            cmd.command = CMD_ABSPOINTER_COMMAND;
            vmware_send(&cmd);
        }

        public static bool is_vmware_backdoor()
        {
            vmware_cmd cmd;
            cmd.bx = ~VMWARE_MAGIC;
            cmd.command = CMD_GETVERSION;
            vmware_send(&cmd);

            if (cmd.bx != VMWARE_MAGIC || cmd.ax == 0xFFFFFFFF)
            {
                /* Not a backdoor! */
                return false;
            }

            return true;
        }
        [DllImport("*")]
        public static extern void vmware_send(vmware_cmd* cmd);

        [DllImport("*")]
        public static extern void vmware_send_hb(vmware_cmd* cmd);

        [DllImport("*")]
        public static extern void vmware_get_hb(vmware_cmd* cmd);
    }
}