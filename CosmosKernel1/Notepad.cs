using System.Drawing;
using Cosmos.System;
using nifanfa.CosmosDrawString;

namespace CosmosKernel1
{
    internal class Notepad : App
    {
        private int textEachLine;
        public string text = string.Empty;

        public Notepad(uint width, uint height, uint x = 0, uint y = 0) : base(width, height, x, y)
        {
            //ASC16 = 16*8
            textEachLine = (int)width / 8;
            name = "* Untitled - Notepad";
        }

        public override void _Update()
        {
            if (KeyboardManager.TryReadKey(out KeyEvent keyEvent))
            {
                switch (keyEvent.Key)
                {
                    case ConsoleKeyEx.Enter:
                        text += "\n";
                        break;
                    case ConsoleKeyEx.Backspace:
                        if (text.Length != 0)
                        {
                            text = text.Remove(text.Length - 1);
                        }
                        break;
                    default:
                        text += keyEvent.KeyChar;
                        break;
                }
            }

            Kernel.vMWareSVGAII.FillRectangle(Color.Black, (int)x, (int)y, (int)width, (int)height);

            if (text.Length != 0)
            {
                string s = string.Empty;
                int i = 0;
                for (int k = 0; k < text.Length; k++)
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

                Kernel.vMWareSVGAII._DrawACSIIString(s, Color.White.ToArgb(), x, y);
            } else
            {
                Kernel.vMWareSVGAII._DrawACSIIString("Edit anything you want", Color.Gray.ToArgb(), x, y);
            }
        }
    }
}
