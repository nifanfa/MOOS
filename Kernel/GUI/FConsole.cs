using Kernel.FS;
using Kernel.Misc;
using System.Drawing;

namespace Kernel.GUI
{
    internal class FConsole : Form
    {
        string Data;
        public Image ScreenBuf;

        public FConsole(int X, int Y) : base(X, Y, 640, 320)
        {
            Title = "Console";
            Data = string.Empty;
            BackgroundColor = 0x0;
            ScreenBuf = new Image(640, 320);

            Console.OnWrite += Console_OnWrite;
            PS2Keyboard.OnKeyChanged += PS2Keyboard_OnKeyChanged;
        }

        private void PS2Keyboard_OnKeyChanged(System.ConsoleKeyInfo key)
        {
            if(key.KeyState == System.ConsoleKeyState.Pressed)
            {
                if (key.Key == System.ConsoleKey.Backspace)
                {
                    if (Data.Length != 0)
                        Data.Length -= 1;
                }
                else if (key.KeyChar != '\0')
                {
                    Console_OnWrite(key.KeyChar);
                }
            }
        }

        public override void Update()
        {
            base.Update();
            int w = 0, h = 0;
            for(int i = 0; i < Data.Length; i++) 
            {

                if ((w + font.Width*2.5f) >= Width ||i %Width == 0 || Data[i]=='\n') { w = 0; h += (int)(font.Height); }
                if (Data[i] != '\n')
                {
                    w += font.Width;
                    font.DrawChar(X + w, Y + h, Data[i]);
                }
            }
            
            if(w == Width) { w = 0;h += (int)(font.Height); } else { w += font.Width; }
            font.DrawChar(X + w, Y + h, '_');
        }

        private void Console_OnWrite(char chr)
        {
            string cs = chr.ToString();
            string cache = Data;
            Data = cache + cs;
            cs.Dispose();
            cache.Dispose();
        }
    }
}
