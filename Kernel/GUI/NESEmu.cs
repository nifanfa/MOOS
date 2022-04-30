using Kernel.Misc;

namespace Kernel.GUI
{
    internal class NESEmu : Window
    {
        public static NES.NES nes;

        public NESEmu(int X, int Y) : base(X, Y, 256,240)
        {
            Title = "NES Emulator";

            if(nes == null)
                nes = new NES.NES();
        }

        public override void OnSetVisible(bool value)
        {
            if (value)
            {
                PS2Keyboard.OnKeyChanged += nes.PS2Keyboard_OnKeyChangedHandler;
            }
            else
            {
                Program.FConsole?.Rebind();
            }
        }

        public unsafe void OpenROM(byte[] buffer) 
        {
            nes.openROM(buffer);
            new Thread(&RunGame);
        }

        public override void OnDraw()
        {
            base.OnDraw();
            Framebuffer.DrawImage(X, Y, nes.gameRender.image, false);
        }

        public static void RunGame() 
        {
            for (; ; ) nes.runGame();
        }
    }
}
