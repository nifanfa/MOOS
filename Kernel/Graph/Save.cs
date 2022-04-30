using System.Drawing;

namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public virtual Image Save()
        {
            Image image = new Image(Width, Height);
            fixed (uint* ptr = image.RawData)
            {
                Native.Movsd(ptr, Memory, (ulong)(Width * Height));
            }
            return image;
        }
    }
}
