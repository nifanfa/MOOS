namespace System.Drawing
{
    public class Pen
    {
        public Brush Brush;
        public Color Color;
        public int Width;
        public Pen(Brush brush)
        {
            Brush = brush;
        }
        public Pen(Brush brush, int width)
        {
            Brush = brush;
            Width = width;
        }
        public Pen(Color color)
        {
            Color = color;
        }
        public Pen(Color color, int width)
        {
            Color = color;
            Width = width;
        }
    }
}