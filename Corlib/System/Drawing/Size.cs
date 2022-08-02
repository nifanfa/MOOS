namespace System.Drawing
{
	public struct Size
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public bool IsEmpty => Width == 0 && Height == 0;
		public static readonly Size Empty = new(0, 0);
		public Size(int width, int height)
		{
			Width = width;
			Height = height;
		}
		public Size(Point point)
		{
			Width = point.X;
			Height = point.Y;
		}
		public override void Dispose()
		{
			Width.Dispose();
			Height.Dispose();
			base.Dispose();
		}
		public static Size Addition(Size size1, Size size2)
		{
			return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
		}
		public static Size Subtraction(Size size1, Size size2)
		{
			return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
		}
		public override bool Equals(object obj)
		{
			if (obj == null || obj is not Size)
			{
				return false;
			}

			Size other = (Size)obj;
			return Width == other.Width && Height == other.Height;
		}

		public bool Equals(Size size)
		{
			Size other = size;
			return Width == other.Width && Height == other.Height;
		}

		public static bool operator ==(Size left, Size right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Size left, Size right)
		{
			return !left.Equals(right);
		}
		public static Size operator +(Size left, Size right)
		{
			return Addition(left, right);
		}
		public static Size operator -(Size left, Size right)
		{
			return Subtraction(left, right);
		}
		public static Size operator *(Size left, Size right)
		{
			return new Size(left.Width * right.Width, left.Height * right.Height);
		}
		public static Size operator /(Size left, Size right)
		{
			return new Size(left.Width / right.Width, left.Height / right.Height);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}