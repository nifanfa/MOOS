using System.Drawing;
using Cosmos.Core;
using Cosmos.System;
using nifanfa.CosmosDrawString;

namespace CosmosKernel1
{
    internal class Dock
    {
        private uint Width = 200;
        private uint Height = 30;
        private uint Devide = 20;

        public void Update()
        {
            Width = (uint)(Kernel.apps.Count + (Kernel.apps.Count * Devide));

            Kernel.vMWareSVGAII.FillRectangle(Color.FromArgb(0x222222), 0, 0, (int)Kernel.screenWidth, 20);
            string text = "PowerOFF";
            uint strX = 2;
            uint strY = (20 - 16) / 2;
            Kernel.vMWareSVGAII._DrawACSIIString("PowerOFF", Color.White.ToArgb(), strX, strY);
            if (Kernel.Pressed)
            {
                if (MouseManager.X > strX && MouseManager.X < strX + (text.Length * 8) && MouseManager.Y > strY && MouseManager.Y < strY + 16)
                {
                    ACPI.Shutdown();
                }
            }

            Kernel.vMWareSVGAII.FillRectangle(Kernel.color, (int)((Kernel.screenWidth - Width) / 2), (int)(Kernel.screenHeight - Height), (int)Width, (int)Height);

            for (int i = 0; i < Kernel.apps.Count; i++)
            {
                Kernel.apps[i].dockX = (uint)((Devide / 2) + ((Kernel.screenWidth - Width) / 2) + i + (Devide * i));
                Kernel.apps[i].dockY = Kernel.screenHeight - (Devide / 2);
            }
        }
    }
}