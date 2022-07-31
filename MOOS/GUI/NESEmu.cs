#if HasGUI
using MOOS.Misc;

namespace MOOS.GUI
{
    internal class NESEmu : Window
    {
        public static Thread nesThread;
        public bool GameStarted = false;
        public static NES.NES nes = new();
        public NESEmu(int X, int Y) : base(X, Y, 256, 240)
        {
#if Chinese
            Title = "红白机模拟器";
#else
            Title = "NES Emulator";
#endif
#if Chinese
            System.Windows.Forms.MessageBox.Show("键位: WASD ZC QE");
#else
            System.Windows.Forms.MessageBox.Show("Keymap: WASD ZC QE");
#endif
        }

        public override void OnSetVisible(bool value)
        {
            if (GameStarted)
            {
                if (value)
                {
                    Keyboard.OnKeyChanged += nes.PS2Keyboard_OnKeyChangedHandler;
                } else
                {
                    Program.FConsole?.Rebind();
                }
            }
        }

        public unsafe void OpenROM(byte[] buffer)
        {
            if (!nes.bolRunGame)
            {
                nes.openROM(buffer);
                nesThread = new Thread(&RunGame).Start(1);
                GameStarted = true;
            }
        }

        public override void OnDraw()
        {
            base.OnDraw();
            Framebuffer.Graphics.DrawImage(nes.gameRender.image, X, Y, false);
        }

        public static void RunGame()
        {
            while (nes.bolRunGame)
            {
                nes.runGame();
            }
        }
    }
}
#endif