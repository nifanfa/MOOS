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