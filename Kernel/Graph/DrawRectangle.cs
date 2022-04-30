namespace Kernel.Graph
{
    public unsafe partial class Graphics
    {
        public virtual void DrawRectangle(int X, int Y, int Width, int Height, uint Color, int Weight = 1)
        {
            FillRectangle(X, Y, Width, Weight, Color);

            FillRectangle(X, Y, Weight, Height, Color);
            FillRectangle(X + (Width - Weight), Y, Weight, Height, Color);

            FillRectangle(X, Y + (Height - Weight), Width, Weight, Color);
        }
    }
}
