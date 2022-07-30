using System;
using System.Drawing;

namespace NES
{
    public class GameRender
    {
        NES NES;

        // Setup background color to use with Alpha
        Color colorBG;

        public void InitializeGame()
        {
            colorBG = Color.Blue;
            image = new Image(256, 240);
        }

        public Image image;

        public unsafe void WriteBitmap(byte[] byteToWrite, Color XColor)
        {
            Program.Clear(XColor.ToArgb());
            lock (this)
            {
                fixed (int* ptr = image.RawData)
                {
                    for (int i = 0; i < image.Width * image.Height; i++) ptr[i] = (int)XColor.ToArgb();
                }

                int w = 0;
                int h = 0;

                for (int i = 0; i < byteToWrite.Length; i += 4)
                {
                    Color color = Color.FromArgb(byteToWrite[i + 3], byteToWrite[i + 2], byteToWrite[i + 1], byteToWrite[i + 0]);
                    if (color.A != 0)
                    {
                        image.RawData[image.Width * h + w] = (int)color.ToArgb();
                    }
                    //
                    w++;
                    //256*240
                    if (w == 256)
                    {
                        w = 0;
                        h++;
                    }
                }
            }
            Program.DrawImage((int)((Program.Width()/2)- (image.Width/2)), (int)((Program.Height() / 2) - (image.Height / 2)), image);
            Program.Update();
        }

        public GameRender(NES formObject)
        {
            NES = formObject;
            InitializeGame();
        }
    }
}