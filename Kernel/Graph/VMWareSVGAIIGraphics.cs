using Kernel.Driver;

namespace Kernel.Graph
{
    internal unsafe class VMWareSVGAIIGraphics : Graphics
    {
        VMWareSVGAII svga;

        public VMWareSVGAIIGraphics() : base(Framebuffer.Width, Framebuffer.Height, Framebuffer.FirstBuffer)
        {
            svga = new VMWareSVGAII();
            svga.SetMode(Framebuffer.Width, Framebuffer.Height);
            Framebuffer.VideoMemory = svga.Video_Memory;
        }

        public override void Update()
        {
            svga.Update();
        }
    }
}
