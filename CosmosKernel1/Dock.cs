using Cosmos.Core;
using Cosmos.System;
using nifanfa.CosmosDrawString;
using System.Drawing;

namespace CosmosKernel1
{
    class Dock
    {
        uint Width = 200;
        uint Height = 30;
        uint Devide = 20;

        public void Update()
        {
            Width = (uint)(Kernel.apps.Count * Kernel.programlogo.Width + Kernel.apps.Count * Devide);

            Kernel.vMWareSVGAII.DrawFillRectangle(0, 0, Kernel.screenWidth, 20, (uint)Kernel.avgCol.ToArgb());
            string text = "PowerOFF";
            uint strX = 2;
            uint strY = (20 - 16) / 2;
            Kernel.vMWareSVGAII._DrawACSIIString("PowerOFF", (uint)Color.White.ToArgb(), strX, strY);
            if (Kernel.Pressed)
            {
                if (MouseManager.X > strX && MouseManager.X < strX + (text.Length * 8) && MouseManager.Y > strY && MouseManager.Y < strY + 16)
                {
                    ACPI.Shutdown();
                }
            }

            Kernel.vMWareSVGAII.DrawFillRectangle((Kernel.screenWidth - Width) / 2, Kernel.screenHeight - Height, Width, Height, (uint)Kernel.avgCol.ToArgb());

            for (int i = 0; i < Kernel.apps.Count; i++)
            {
                Kernel.apps[i].dockX = (uint)(Devide / 2 + ((Kernel.screenWidth - Width) / 2) + (Kernel.programlogo.Width * i) + (Devide * i));
                Kernel.apps[i].dockY = (uint)(Kernel.screenHeight - Kernel.programlogo.Height - Devide / 2);
                Kernel.vMWareSVGAII.DrawImage(Kernel.programlogo, (int)Kernel.apps[i].dockX, (int)Kernel.apps[i].dockY);
            }
        }
    }
}