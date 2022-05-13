using Kernel.Misc;

namespace Kernel.GUI
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

            if (nes == null)
                nes = new NES.NES();
        }

        public override void OnSetVisible(bool value)
        {
            if(GameStarted)
            {
                nesThread.Terminated = !value;
                if (value)
                {
                    PS2Keyboard.OnKeyChanged += nes.PS2Keyboard_OnKeyChangedHandler;
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
                nesThread = new Thread(&RunGame);
                GameStarted = true;
            }
        }

        public override void OnDraw()
        {
            base.OnDraw();
            Framebuffer.Graphics.DrawImage(X, Y, nes.gameRender.image, false);
        }

        public static void RunGame() 
        {
            for (; ; ) nes.runGame();
        }
    }
}
