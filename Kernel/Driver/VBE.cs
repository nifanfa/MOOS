namespace Kernel.Driver
{
    public static unsafe class VBE
    {
        public static VBEInfo* Info;

        public static void Initialise(VBEInfo* info)
        {
            Info = info;
            ASC16.Initialise();
            if (info->PhysBase != 0) 
            {
                Framebuffer.VideoMemory = (uint*)(info->PhysBase & ~0xF);
                Framebuffer.SetVideoMode(info->ScreenWidth, info->ScreenHeight);
                Framebuffer.Clear(0x0);
            }
        }
    }
}
