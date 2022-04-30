namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public virtual void FillRectangle(int X, int Y, int Width, int Height, uint Color, bool hasAlpha = false)
        {
            for (int w = 0; w < Width; w++)
            {
                for (int h = 0; h < Height; h++)
                {
                    DrawPoint(X + w, Y + h, Color, hasAlpha);
                }
            }
        }
    }
}
