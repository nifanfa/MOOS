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
        }

        public override void Update()
        {
            base.Update();
            int w = 0, h = 0;
            for(int i = 0; i < Data.Length; i++) 
            {

                if (w == Width ||i %Width == 0) { w = 0; h += 16; }
                w += 8;
                ASC16.DrawChar(Data[i], X + w, Y + h, 0xFFFFFFFF);
            }
            
            if(w == Width) { w = 0;h += 16; } else { w += 8; }
            ASC16.DrawChar('_', X + w, Y + h, 0xFFFFFFFF);
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
