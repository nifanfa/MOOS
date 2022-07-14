using MOOS.Driver;
using MOOS.FS;
using MOOS.Misc;

namespace MOOS.Graph
{
    internal unsafe class VMWareSVGAIIGraphics : Graphics
    {
        VMWareSVGAII svga;

        public VMWareSVGAIIGraphics(ushort Width = 1280,ushort Height = 768) : base(Width,Height,null)
        {
            svga = new VMWareSVGAII();
            svga.SetMode(Width,Height);
            Framebuffer.Initialize(Width, Height, svga.Video_Memory);
            base.VideoMemory = Framebuffer.FirstBuffer;

#if HasGUI
            //Image from unsplash
            Program.Wallpaper = new PNG(File.Instance.ReadAllBytes("Images/Wallpaper.png"));
#endif
        }

        public override void Update()
        {
            svga.Update();
        }
    }
}