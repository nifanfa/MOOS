namespace System.Drawing
{
	public class Image
	{
		public uint[] RawData;
		public int Bpp;
		public int Width;
		public int Height;

		public Image(int width, int height)
		{
			Width = width;
			Height = height;
			Bpp = 4;
			RawData = new uint[width * height];
		}

		public Image()
		{

		}

		public Color GetPixel(int X, int Y)
		{
			return Color.FromArgb(RawData[(Y * Width) + X]);
		}

		private static unsafe void Resample(void* input, void* output, int oldw, int oldh, int neww, int newh)
		{
			for (int i = 0; i < newh; i++)
			{
				for (int j = 0; j < neww; j++)
				{

					float tmp = i / (float)(newh - 1) * (oldh - 1);
					int l = (int)Math.Floor(tmp);
					if (l < 0)
					{
						l = 0;
					} else
					{
						if (l >= oldh - 1)
						{
							l = oldh - 2;
						}
					}

					float u = tmp - l;
					tmp = j / (float)(neww - 1) * (oldw - 1);
					int c = (int)Math.Floor(tmp);
					if (c < 0)
					{
						c = 0;
					} else
					{
						if (c >= oldw - 1)
						{
							c = oldw - 2;
						}
					}
					float t = tmp - c;

					float d1 = (1 - t) * (1 - u);
					float d2 = t * (1 - u);
					float d3 = t * u;
					float d4 = (1 - t) * u;

					uint p1 = *((uint*)input + (l * oldw) + c);
					uint p2 = *((uint*)input + (l * oldw) + c + 1);
					uint p3 = *((uint*)input + ((l + 1) * oldw) + c + 1);
					uint p4 = *((uint*)input + ((l + 1) * oldw) + c);

					byte blue = (byte)(((byte)p1 * d1) + ((byte)p2 * d2) + ((byte)p3 * d3) + ((byte)p4 * d4));
					byte green = (byte)(((byte)(p1 >> 8) * d1) + ((byte)(p2 >> 8) * d2) + ((byte)(p3 >> 8) * d3) + ((byte)(p4 >> 8) * d4));
					byte red = (byte)(((byte)(p1 >> 16) * d1) + ((byte)(p2 >> 16) * d2) + ((byte)(p3 >> 16) * d3) + ((byte)(p4 >> 16) * d4));

					*((uint*)output + (i * neww) + j) = (uint)((red << 16) | (green << 8) | (blue));
				}
			}
		}
		public unsafe Image ResizeImage(int NewWidth, int NewHeight)
		{
			if (NewWidth == 0 || NewHeight == 0)
			{
				return new Image();
			}

			int w1 = Width, h1 = Height;
			uint[] temp = new uint[NewWidth * NewHeight];

			fixed (uint* output = temp)
			{
				fixed (uint* input = RawData)
				{
					lock (null)
					{
						Resample(input, output, Width, Height, NewWidth, NewHeight);
					}
				}
			}

			Image image = new()
			{
				Width = NewWidth,
				Height = NewHeight,
				Bpp = Bpp,
				RawData = temp
			};

			return image;
		}

		public override void Dispose()
		{
			RawData.Dispose();
			base.Dispose();
		}
	}
}