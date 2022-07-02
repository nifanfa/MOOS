#if HasGUI
using MOOS.Driver;
using MOOS.FS;
using MOOS.Graph;
using MOOS.Misc;
using System;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;

namespace MOOS.GUI
{
    internal unsafe class Doom : Window
    {
        #region Doom
        [DllImport("*")]
        public static extern int doommain(byte* gb, long gl);

        [RuntimeExport("GetTickCount")]
        public static uint GetTickCount()
        {
            return (uint)Timer.Ticks;
        }

        [RuntimeExport("Sleep")]
        public static void Sleep(uint ms)
        {
            Thread.Sleep(ms);
        }

        [RuntimeExport("DrawPoint")]
        public static void DrawPoint(int x, int y, uint color)
        {
            dg.DrawPoint(x, y, color);
        }

        [RuntimeExport("getkbdkey")]
        public static uint getkbdkey()
        {
            if (PS2Keyboard.KeyInfo.Key == 0) return 0;
            else
            {
                var key = (uint)PS2Keyboard.KeyInfo.Key;
                if (PS2Keyboard.KeyInfo.KeyState == ConsoleKeyState.Pressed) key |= (1u << 31);
                PS2Keyboard.KeyInfo.Key = 0;
                return key;
            }
        }

        public static byte[] gb;

        public static Image di;
        public static Graphics dg;
        #endregion

        public Doom(int X, int Y) : base(X, Y, 640, 400)
        {
            Title = "DOOM Shareware";

            di = new Image(640, 400);
            dg = Graphics.FromImage(di);

            gb = File.Instance.ReadAllBytes("DOOM1.WAD");

            new Thread(&dm).Start();
        }

        public override void OnDraw()
        {
            base.OnDraw();
            Framebuffer.Graphics.DrawImage(X, Y, di, false);
        }

        public static void dm()
        {
            fixed (byte* ptr = gb)
                doommain(ptr, gb.Length);
        }
    }
}
#endif