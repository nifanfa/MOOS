namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public virtual void Copy(int dX, int dY, int sX, int sY, int Width, int Height)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    DrawPoint(dX + w, dY + h, GetPoint(sX + w, sY + h));
                }
            }
        }
    }
}
