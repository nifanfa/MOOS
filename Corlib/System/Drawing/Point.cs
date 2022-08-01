namespace System.Drawing
{
	public struct Point
	{
		public int X { get; set; }
		public int Y { get; set; }
		public static readonly Point Empty = new(0, 0);

		public Point(int dw)
		{
			X = dw & 0xFFFF;
			Y = dw >> 16;
		}
		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}
		public Point(Size size)
		{
			X = size.Width;
			Y = size.Height;
		}
		public override void Dispose()
		{
			X.Dispose();
			Y.Dispose();
			base.Dispose();
		}
		public static Point Add(Point point, Size size)
		{
			return new Point(point.X + size.Width, point.Y + size.Height);
		}
		public static Point Subtract(Point point, Size size)
		{
			return new Point(point.X - size.Width, point.Y - size.Height);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj is not Point)
			{
				return false;
			}

			Point other = (Point)obj;
			return other.X == X && other.Y == Y;
		}

		public static bool operator ==(Point left, Point right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right)
		{
			return !left.Equals(right);
		}
		public static Point operator +(Point point, Size size)
		{
			return Add(point, size);
		}
		public static Point operator -(Point point, Size size)
		{
			return Subtract(point, size);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}