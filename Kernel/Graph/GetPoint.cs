namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public virtual unsafe uint GetPoint(int X, int Y)
        {
            if (X > 0 && Y > 0 && X < Width && Y < Height)
            {
                return Memory[Width * Y + X];
            }
            return 0;
        }
    }
}
