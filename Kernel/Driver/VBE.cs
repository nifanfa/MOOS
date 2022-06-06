/*
 * Copyright(c) 2022 nifanfa, This code is part of the Moos licensed under the MIT licence.
 */
namespace MOOS.Driver
{
    public static unsafe class VBE
    {
        public static VBEInfo* Info;

        public static void Initialise(VBEInfo* info)
        {
            Info = info;
            if (info->PhysBase != 0) 
            {
                Framebuffer.VideoMemory = (uint*)info->PhysBase;
                Framebuffer.SetVideoMode(info->ScreenWidth, info->ScreenHeight);
                Framebuffer.Graphics.Clear(0x0);
            }
        }
    }
}
