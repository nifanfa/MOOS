namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public virtual unsafe void Clear(uint color) 
        {
            Native.Stosd(Memory, color, (ulong)(Width * Height));
        }
    }
}
