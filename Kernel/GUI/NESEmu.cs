#if HasGUI
using MOOS.Misc;
using System.Windows;

namespace MOOS.GUI
{
    internal class NESEmu : Window
    {
        public static NES.NES nes;
        public static Thread nesThread;
        public bool GameStarted = false;

        public NESEmu(int X, int Y) : base(X, Y, 256,240)
        {
#if Chinese
            Title = "红白机模拟器";
#else
            Title = "NES Emulator";
#endif
            nes = new NES.NES();

#if Chinese
            System.Windows.Forms.MessageBox.Show("键位: WASD ZC QE");
#else
            System.Windows.MessageBox.Show("Keymap: WASD ZC QE", "Information");
#endif
        }

        public override void OnSetVisible(bool value)
        {
            if(GameStarted)
            {
                nesThread.Terminated = !value;
                if (value)
                {
                    Keyboard.OnKeyChanged += nes.PS2Keyboard_OnKeyChangedHandler;
                }
                else
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

        public override void Draw()
        {
            base.Draw();
            Framebuffer.Graphics.DrawImage(X, Y, nes.gameRender.image, false);
        }

        public static void RunGame() 
        {
            for (; ; ) nes.runGame();
        }
    }
}
#endif