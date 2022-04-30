/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
using Kernel;
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
            lock(this)
            {
                fixed (uint* ptr = image.RawData)
                {
                    Native.Stosd(ptr, XColor.ToArgb(), (ulong)(image.Width * image.Height));
                }

                int w = 0;
                int h = 0;

                for (int i = 0; i < byteToWrite.Length; i += 4)
                {
                    Color color = Color.FromArgb(byteToWrite[i + 3], byteToWrite[i + 2], byteToWrite[i + 1], byteToWrite[i + 0]);
                    if (color.A != 0)
                    {
                        image.RawData[image.Width * h + w] = color.ToArgb();
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
        }

        public GameRender(NES formObject)
        {
            NES = formObject;
            InitializeGame();
        }
    }
}
