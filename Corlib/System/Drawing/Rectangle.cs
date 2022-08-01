namespace System.Drawing
{
	public struct Rectangle
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int Bottom { get; private set; }
		public int Top { get; private set; }
		public int Left { get; private set; }
		public int Right { get; private set; }
		public bool isEmpty => X == 0 && Y == 0 && Width == 0 && Height == 0;

		public static readonly Rectangle Empty = new(0, 0, 0, 0);

		public Rectangle(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			Bottom = y + height;
			Right = x + width;
			Top = y;
			Left = x;
		}

		public Rectangle(Point position, Size dimentions)
		{
			X = position.X;
			Y = position.Y;
			Width = dimentions.Width;
			Height = dimentions.Height;
			Bottom = position.Y + dimentions.Height;
			Right = position.X + dimentions.Width;
			Top = position.Y;
			Left = position.X;
		}
	}
}