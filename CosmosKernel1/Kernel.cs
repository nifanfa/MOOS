//TO-DO this project just simply ported Cosmos-GUI-Sample which still have protential
//memory leak risk. I'll try to find and dispose it later

//static constructor is not supported you have to notice that!
//things like: static string str = "hello world"; is not supported! str will be null

using Cosmos.Core;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using MOOS.Driver;
using nifanfa.CosmosDrawString;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        static extern void Main();

        [RuntimeExport("KMain")]
        static void KMain()
        {
            MOOS.Console.WriteLine("Entering Cosmos Kernel");
            Kernel kernel = new Kernel();
            kernel.BeforeRun();
            for (; ; ) kernel.Run();
        }

        public static uint screenWidth = 640;
        public static uint screenHeight = 480;
        public static Canvas vMWareSVGAII;
        Bitmap bitmap;
        public static Bitmap programlogo;
        Bitmap bootBitmap;

        int[] cursor = new int[]
            {
                1,0,0,0,0,0,0,0,0,0,0,0,
                1,1,0,0,0,0,0,0,0,0,0,0,
                1,2,1,0,0,0,0,0,0,0,0,0,
                1,2,2,1,0,0,0,0,0,0,0,0,
                1,2,2,2,1,0,0,0,0,0,0,0,
                1,2,2,2,2,1,0,0,0,0,0,0,
                1,2,2,2,2,2,1,0,0,0,0,0,
                1,2,2,2,2,2,2,1,0,0,0,0,
                1,2,2,2,2,2,2,2,1,0,0,0,
                1,2,2,2,2,2,2,2,2,1,0,0,
                1,2,2,2,2,2,2,2,2,2,1,0,
                1,2,2,2,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,1,1,1,1,1,
                1,2,2,2,1,2,2,1,0,0,0,0,
                1,2,2,1,0,1,2,2,1,0,0,0,
                1,2,1,0,0,1,2,2,1,0,0,0,
                1,1,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,1,2,2,1,0,0,
                0,0,0,0,0,0,0,1,1,0,0,0
            };

        LogView logView;
        Clock Clock;
        Notepad notepad;
        Dock dock;
        public static bool Pressed;

        public static List<App> apps;

        public static Color avgCol;

        protected override void BeforeRun()
        {
            ASC16.Initialise();

            apps = new List<App>();

            CosmosVFS cosmosVFS = new CosmosVFS();
            VFSManager.RegisterVFS(cosmosVFS);

            bootBitmap = new Bitmap(@"0:\boot.bmp");

            vMWareSVGAII = FullScreenCanvas.GetFullScreenCanvas();

            vMWareSVGAII.Clear(0x0);
            vMWareSVGAII.DrawImage(bootBitmap, 0, 0);
            vMWareSVGAII.Update();

            Timer.Sleep(1000);

            bitmap = new Bitmap(@"0:\timg.bmp");
            programlogo = new Bitmap(@"0:\program.bmp");

            uint r = 0;
            uint g = 0;
            uint b = 0;
            for (uint i = 0; i < bitmap.rawData.Length; i++)
            {
                Color color = Color.FromArgb((uint)bitmap.rawData[i]);
                r += color.R;
                g += color.G;
                b += color.B;
            }
            avgCol = Color.FromArgb((byte)(r / bitmap.rawData.Length), (byte)(g / bitmap.rawData.Length), (byte)(b / bitmap.rawData.Length));

            MouseManager.ScreenWidth = (int)screenWidth;
            MouseManager.ScreenHeight = (int)screenHeight;
            MouseManager.X = (int)(screenWidth / 2);
            MouseManager.Y = (int)(screenHeight / 2);

            logView = new LogView(300, 200, 10, 30);
            Clock = new Clock(200, 200, 400, 200);
            notepad = new Notepad(200, 100, 10, 300);
            dock = new Dock();

            apps.Add(logView);
            apps.Add(Clock);
            apps.Add(notepad);
        }

        protected override void Run()
        {
            switch (MouseManager.MouseState)
            {
                case MouseState.Left:
                    Pressed = true;
                    break;
                case MouseState.None:
                    Pressed = false;
                    break;
            }

            vMWareSVGAII.Clear(avgCol.ToArgb());
            vMWareSVGAII.DrawImage(bitmap, 0, 0);
            logView.text = $"Time: ?? \nInstall RAM: {CPU.GetAmountOfRAM()}MB, Video RAM: ?? Bytes";

            for(int i = 0; i < apps.Count; i++) 
            {
                apps[i].Update();
            }

            dock.Update();

            DrawCursor(vMWareSVGAII, (uint)MouseManager.X, (uint)MouseManager.Y);

            end:

            vMWareSVGAII.Update();
        }

        public void DrawCursor(Canvas vMWareSVGAII, uint x, uint y)
        {
            for (uint h = 0; h < 19; h++)
            {
                for (uint w = 0; w < 12; w++)
                {
                    if (cursor[h * 12 + w] == 1)
                    {
                        vMWareSVGAII.DrawPoint(w + x, h + y, (uint)Color.Black.ToArgb());
                    }
                    if (cursor[h * 12 + w] == 2)
                    {
                        vMWareSVGAII.DrawPoint(w + x, h + y, (uint)Color.White.ToArgb());
                    }
                }
            }
        }
    }
}
