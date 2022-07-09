#if HasGUI
using MOOS.Driver;
using MOOS.FS;
using MOOS.Graph;
using MOOS.Misc;
using System;
using System.Diagnostics;
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

        [DllImport("*")]
        public static extern void addKeyToQueue(int pressed, byte keyCode);

        public static byte[] gb;

        public static Image di;
        public static Graphics dg;
        #endregion

        public Doom(int X, int Y) : base(X, Y, 640, 400)
        {
#if Chinese
            Title = "毁灭战士";
#else
            Title = "DOOM Shareware";
#endif

            di = new Image(640, 400);
            dg = Graphics.FromImage(di);

            gb = File.Instance.ReadAllBytes("DOOM1.WAD");

            Keyboard.OnKeyChanged += PS2Keyboard_OnKeyChanged;

#if Chinese
            System.Windows.Forms.MessageBox.Show("键位: WASD Ctrl Shift ESC Enter");
#else
            System.Windows.Forms.MessageBox.Show("Keymap: WASD Ctrl Shift ESC Enter");
#endif

            new Thread(new Action(() =>
            {
                fixed (byte* ptr = gb)
                    doommain(ptr, gb.Length);
            })).Start();
        }

        private void PS2Keyboard_OnKeyChanged(ConsoleKeyInfo key)
        {
            addKeyToQueue(key.KeyState != ConsoleKeyState.Released ? 1 : 0, (byte)key.Key);
        }

        public override void OnDraw()
        {
            base.OnDraw();
            Framebuffer.Graphics.DrawImage(X, Y, di, false);
        }
    }
}
#endif