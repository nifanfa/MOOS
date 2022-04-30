namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public uint* Memory;
        public int Width;
        public int Height;

        public Graphics(int width,int height,uint* ptr)
        {
            this.Width = width;
            this.Height = height;
            this.Memory = ptr;
        }
    }
}
