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
        }

        public unsafe void WriteBitmap(byte[] byteToWrite, Color XColor)
        {
            lock (this)
            {
                fixed (int* ptr = Program.ScreenBuf.RawData)
                {
                    for (int i = 0; i < Program.ScreenBuf.Width * Program.ScreenBuf.Height; i++) ptr[i] = (int)XColor.ToArgb();
                }

                int w = 0;
                int h = 0;

                for (int i = 0; i < byteToWrite.Length; i += 4)
                {
                    Color color = Color.FromArgb(byteToWrite[i + 3], byteToWrite[i + 2], byteToWrite[i + 1], byteToWrite[i + 0]);
                    if (color.A != 0)
                    {
                        Program.ScreenBuf.RawData[Program.ScreenBuf.Width * h + w] = (int)color.ToArgb();
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