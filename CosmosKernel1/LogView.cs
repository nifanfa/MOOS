using System.Drawing;
using nifanfa.CosmosDrawString;

namespace CosmosKernel1
{
    internal class LogView : App
    {
        private int textEachLine;
        public string text = string.Empty;

        public LogView(uint width, uint height, uint x = 0, uint y = 0) : base(width, height, x, y)
        {
            //ASC16 = 16*8
            textEachLine = (int)width / 8;
            name = "LogView";
        }

        public override void _Update()
        {
            Kernel.vMWareSVGAII.FillRectangle(Color.Black, (int)x, (int)y, (int)width, (int)height);

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
        }
    }
}
