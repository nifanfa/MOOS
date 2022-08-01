using Cosmos.System;
using nifanfa.CosmosDrawString;
using System.Drawing;

namespace CosmosKernel1
{
    class Notepad : App
    {
        int textEachLine;
        public string text = string.Empty;

        public Notepad(uint width, uint height, uint x = 0, uint y = 0) : base(width, height, x, y)
        {
            //ASC16 = 16*8
            textEachLine = (int)width / 8;
            name = "*New File - Notepad";
        }

        public override void _Update()
        {
            KeyEvent keyEvent;
            if (KeyboardManager.TryReadKey(out keyEvent))
            {
                switch (keyEvent.Key)
                {
                    case ConsoleKeyEx.Enter:
                        this.text += "\n";
                        break;
                    case ConsoleKeyEx.Backspace:
                        if (this.text.Length != 0)
                        {
                            this.text = this.text.Remove(this.text.Length - 1);
                        }
                        break;
                    default:
                        this.text += keyEvent.KeyChar;
                        break;
                }
            }

            Kernel.vMWareSVGAII.DrawFillRectangle(x, y, width, height, (uint)Color.Black.ToArgb());

            if (text.Length != 0)
            {
                string s = string.Empty;
                int i = 0;
                for(int k = 0; k < text.Length; k++) 
                {
                    char c = text[k];

                    s += c;
                    i++;
                    if (i + 1 == textEachLine || c == '\n')
                    {
                        if (c != '\n')
                        {
                            s += "\n";
                        }
                        i = 0;
                    }
                }

                Kernel.vMWareSVGAII._DrawACSIIString(s, (uint)Color.White.ToArgb(), x, y);
            }
            else
            {
                Kernel.vMWareSVGAII._DrawACSIIString("Edit anything you want", (uint)Color.Gray.ToArgb(), x, y);
            }
        }
    }
}
