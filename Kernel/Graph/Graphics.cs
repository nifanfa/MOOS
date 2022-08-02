//#define AllowNonAntiAliased

using System;
using System.Collections.Generic;
using System.Drawing;

namespace MOOS.Graph
{
	public unsafe class Graphics
	{
		public uint* VideoMemory;
		public int Width;
		public int Height;

		public Graphics(int width, int height, void* vm)
		{
			Width = width;
			Height = height;
			VideoMemory = (uint*)vm;
		}

		public virtual void Update() { }

		public static Graphics FromImage(Image img)
		{
			fixed (int* ptr = img.RawData)
			{
				return new Graphics(img.Width, img.Height, ptr);
			}
		}

		public void Clear(Color color)
		{
			Native.Stosd(VideoMemory, color.ToArgb(), (ulong)(Width * Height));
			color.Dispose();
		}

		public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
		{
			for (int X = 0; X < blockRegionSize.Width; X++)
			{
				for (int Y = 0; Y < blockRegionSize.Height; Y++)
				{
					DrawPoint(GetPoint(X + sourceX, Y + sourceY), X + destinationX, Y + destinationY);
				}
			}
			sourceX.Dispose();
			sourceY.Dispose();
			destinationX.Dispose();
			destinationY.Dispose();
			blockRegionSize.Dispose();
		}

		public void CopyFromScreen(Point source, Point destination, Size blockRegionSize)
		{
			for (int X = 0; X < blockRegionSize.Width; X++)
			{
				for (int Y = 0; Y < blockRegionSize.Height; Y++)
				{
					DrawPoint(GetPoint(X + source.X, Y + source.Y), X + destination.X, Y + destination.Y);
				}
			}
			source.Dispose();
			destination.Dispose();
			blockRegionSize.Dispose();
		}

		public void DrawArc(Color color, int x, int y, double start_angle, double end_angle, int radius)
		{
			List<int> xs = new();
			List<int> ys = new();

			for (double i = start_angle; i < end_angle; i += 0.05)
			{
				xs.Add((int)(x + (Math.Cos(i) * radius)));
				ys.Add((int)(y + (Math.Sin(i) * radius)));
			}

			for (int i = 0; i < xs.Count - 1; i++)
			{
				DrawLine(color, xs[i], ys[i], xs[i + 1], ys[i + 1]);
			}
			xs.Dispose();
			ys.Dispose();
		}

#if AllowNonAntiAliased
		public void DrawBezier(Color color, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
		{
			for (double t = 0.0; t <= 1.0; t += 0.001)
			{
				int x = (int)((Math.Pow(1 - t, 3) * x1) + (3 * t * Math.Pow(1 - t, 2) * x2) + (3 * t * t * (1 - t) * x3) + (Math.Pow(t, 3) * x4));
				int y = (int)((Math.Pow(1 - t, 3) * y1) + (3 * t * Math.Pow(1 - t, 2) * y2) + (3 * t * t * (1 - t) * y3) + (Math.Pow(t, 3) * y4));
				DrawPoint(color, x, y);
			}
			x1.Dispose();
			y1.Dispose();
			x2.Dispose();
			y2.Dispose();
			x3.Dispose();
			y3.Dispose();
			x4.Dispose();
			y4.Dispose();
			color.Dispose();
		}
#endif

#if AllowNonAntiAliased
		public void DrawBezier(Color color, Point pt1, Point pt2, Point pt3, Point pt4)
		{
			for (double t = 0.0; t <= 1.0; t += 0.001)
			{
				int x = (int)((Math.Pow(1 - t, 3) * pt1.X) + (3 * t * Math.Pow(1 - t, 2) * pt2.X) + (3 * t * t * (1 - t) * pt3.X) + (Math.Pow(t, 3) * pt4.X));
				int y = (int)((Math.Pow(1 - t, 3) * pt1.Y) + (3 * t * Math.Pow(1 - t, 2) * pt2.Y) + (3 * t * t * (1 - t) * pt3.Y) + (Math.Pow(t, 3) * pt4.Y));
				DrawPoint(color, x, y);
			}
			pt1.Dispose();
			pt2.Dispose();
			pt3.Dispose();
			pt4.Dispose();
			color.Dispose();
		}
#endif

		// TODO: DrawBeziers
		// TODO: DrawClosedCurve
		// TODO: DrawCurve

#if AllowNonAntiAliased
		public void DrawEllipse(Color color, int x, int y, int width, int height)
		{
			int a = 2 * width;
			int b = 2 * height;
			int b1 = b & 1;
			int dx = 4 * (1 - a) * b * b;
			int dy = 4 * (b1 + 1) * a * a;
			int err = dx + dy + (b1 * a * a);
			int e2;
			int _y = 0;
			int _x = width;
			a *= 8 * a;
			b1 = 8 * b * b;

			while (_x >= 0)
			{
				DrawPoint(color, x + _x, y + _y);
				DrawPoint(color, x - _x, y + _y);
				DrawPoint(color, x - _x, y - _y);
				DrawPoint(color, x + _x, y - _y);
				e2 = 2 * err;
				if (e2 <= dy)
				{ _y++; err += dy += a; }
				if (e2 >= dx || 2 * err > dy)
				{ _x--; err += dx += b1; }
			}
		}
#endif

#if AllowNonAntiAliased
		public void DrawEllipse(Color color, Rectangle rect)
		{
			DrawEllipse(color, rect.X, rect.Y, rect.Width, rect.Height);
		}
#endif

#if AllowNonAntiAliased
		public void FillEllipse(Color color, int x, int y, int height, int width)
		{
			int hh = height * height;
			int ww = width * width;
			int hhww = hh * ww;
			int x0 = width;
			int dx = 0;

			// do the horizontal diameter
			for (int _x = -width; _x <= width; _x++)
			{
				DrawPoint(color, x + _x, y);
			}

			// now do both halves at the same time, away from the diameter
			for (int _y = 1; _y <= height; _y++)
			{
				int x1 = x0 - (dx - 1);  // try slopes of dx - 1 or more
				for (; x1 > 0; x1--)
				{
					if ((x1 * x1 * hh) + (_y * _y * ww) <= hhww)
					{
						break;
					}
				}

				dx = x0 - x1;  // current approximation of the slope
				x0 = x1;

				for (int _x = -x0; _x <= x0; _x++)
				{
					DrawPoint(color, x + _x, y - _y);
					DrawPoint(color, x + _x, y + _y);
				}
			}
		}
#endif

#if AllowNonAntiAliased
		public void FillEllipse(Color color, Rectangle rect)
		{
			FillEllipse(color, rect.X, rect.Y, rect.Width, rect.Height);
		}
#endif

		private void DrawImageInternal(Image image, int X, int Y, int cutWidth, int cutHeight, int startX, int startY, bool AlphaBlending)
		{
			if (AlphaBlending)
			{
				for (int h = startY; h < cutHeight; h++)
				{
					for (int w = startX; w < cutWidth; w++)
					{
						uint foreground = (uint)image.RawData[(cutWidth * h) + w];
						int fA = (byte)((foreground >> 24) & 0xFF);

						if (fA != 0)
						{
							//if (w > image.Width || h > image.Height || w <= image.Width || h <= image.Height)
							//{
							//                                foreground = 0xFFFFFF;
							//}
							DrawPoint(Color.FromArgb(foreground), X + w, Y + h, true);
						}
					}
				}
			} else
			{
				for (int h = startY; h < cutHeight; h++)
				{
					for (int w = startX; w < cutWidth; w++)
					{
						uint color = (uint)image.RawData[(cutWidth * h) + w];
						//if (w > image.Width || h > image.Height || w <= image.Width || h <= image.Height)
						//{
						//                            color = 0xFFFFFF;
						//}
						DrawPoint(Color.FromArgb(color), X + w, Y + h);
					}
				}
			}
		}

		public void DrawImage(Image image,int X, int Y, byte alpha)
		{
			for (int h = 0; h < image.Height; h++)
            {
                for (int w = 0; w < image.Width; w++)
				{
					uint foreground = (uint)image.RawData[image.Width * h + w];
					foreground &= ~0xFF000000;
					foreground |= (uint)alpha << 24;
					int fA = (byte)((foreground >> 24) & 0xFF);

					if (fA != 0)
					{
						DrawPoint(Color.FromArgb(foreground), X + w, Y + h, true);
					}
				}
            }
        }

		public void DrawImage(Image image, int x, int y, bool AlphaBlending = false)
		{
			DrawImageInternal(image, x, y, image.Width, image.Height, 0, 0, AlphaBlending);
		}

		public void DrawImage(Image image, int x, int y, int w, int h, bool AlphaBlending = false)
		{
			Image resized = image.ResizeImage(w, h);
			DrawImageInternal(resized, x, y, resized.Width, resized.Height, 0, 0, AlphaBlending);
		}

		public void DrawImage(Image image, int x, int y, Rectangle srcRect, bool AlphaBlending = false)
		{
			DrawImageInternal(image, x, y, srcRect.Width, srcRect.Height, srcRect.X, srcRect.Y, AlphaBlending);
		}

		public void DrawImage(Image image, Point position, bool AlphaBlending = false)
		{
			DrawImageInternal(image, position.X, position.Y, image.Width, image.Height, 0, 0, AlphaBlending);
		}

		public void DrawImage(Image image, Rectangle destRect, bool AlphaBlending = false)
		{
			Image resized = image.ResizeImage(destRect.Width, destRect.Height);
			DrawImageInternal(resized, destRect.X, destRect.Y, resized.Width, resized.Height, 0, 0, AlphaBlending);
		}

		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, bool AlphaBlending = false)
		{
			Image resized = image.ResizeImage(destRect.Width, destRect.Height);
			DrawImageInternal(resized, destRect.X, destRect.Y, srcWidth, srcHeight, srcX, srcY, AlphaBlending);
		}

		public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, bool AlphaBlending = false)
		{
			Image resized = image.ResizeImage(destRect.Width, destRect.Height);
			DrawImageInternal(resized, destRect.X, destRect.Y, srcRect.Width, srcRect.Height, srcRect.X, srcRect.Y, AlphaBlending);
		}

		private void DrawLineInternal(Color color, int x0, int y0, int x1, int y1)
		{
			DrawLine(x0, y0, x1, y1, color.ToArgb());

			#region Xiaolin Wu's line algorithm
			// swaps two numbers
			void Swap(int* a, int* b)
			{
				int temp = *a;
				*a = *b;
				*b = temp;
			}

			// returns absolute value of number
			float Absolute(float x)
			{
				if (x < 0) return -x;
				else return x;
			}

			//returns integer part of a floating point number
			int IPartOfNumber(float x)
			{
				return (int)x;
			}

			//rounds off a number
			int RoundNumber(float x)
			{
				return IPartOfNumber(x + 0.5f);
			}

			//returns fractional part of a number
			float FPartOfNumber(float x)
			{
				if (x > 0) return x - IPartOfNumber(x);
				else return x - (IPartOfNumber(x) + 1);

			}

			//returns 1 - fractional part of number
			float RFPartOfNumber(float x)
			{
				return 1 - FPartOfNumber(x);
			}

		// draws a pixel on screen of given brightness
		// 0<=brightness<=1. We can use your own library
		// to draw on screen
		void DrawPoint(int X, int Y, uint Color, float Brightness)
		{
			byte A = (byte)((Color >> 24) & 0xFF);
			byte R = (byte)((Color >> 16) & 0xFF);
			byte G = (byte)((Color >> 8) & 0xFF);
			byte B = (byte)((Color) & 0xFF);
			A = ((byte)(A * (1f - Brightness)));
				this.DrawPoint(System.Drawing.Color.FromArgb(A,R,G,B), X, Y, true);
		}

		void DrawLine(int x0, int y0, int x1, int y1, uint color)
		{
			bool steep = Absolute(y1 - y0) > Absolute(x1 - x0);

			// swap the co-ordinates if slope > 1 or we
			// draw backwards
			if (steep)
			{
				Swap(&x0, &y0);
				Swap(&x1, &y1);
			}
			if (x0 > x1)
			{
				Swap(&x0, &x1);
				Swap(&y0, &y1);
			}

			//compute the slope
			float dx = x1 - x0;
			float dy = y1 - y0;
			float gradient = dy / dx;
			if (dx == 0.0)
				gradient = 1;

			int xpxl1 = x0;
			int xpxl2 = x1;
			float intersectY = y0;

			// main loop
			if (steep)
			{
				int x;
				for (x = xpxl1; x <= xpxl2; x++)
				{
					// pixel coverage is determined by fractional
					// part of y co-ordinate
					DrawPoint(IPartOfNumber(intersectY), x, color,
								RFPartOfNumber(intersectY));
					DrawPoint(IPartOfNumber(intersectY) - 1, x, color,
								FPartOfNumber(intersectY));
					intersectY += gradient;
				}
			}
			else
			{
				int x;
				for (x = xpxl1; x <= xpxl2; x++)
				{
					// pixel coverage is determined by fractional
					// part of y co-ordinate
					DrawPoint(x, IPartOfNumber(intersectY), color,
								RFPartOfNumber(intersectY));
					DrawPoint(x, IPartOfNumber(intersectY) - 1, color,
								  FPartOfNumber(intersectY));
					intersectY += gradient;
				}
			}

		}
		#endregion
		}

		public void DrawLine(Color color, int x1, int y1, int x2, int y2)
		{
			DrawLineInternal(color, x1, y1, x2, y2);
		}

		public void DrawLine(Color color, Point a, Point b)
		{
			DrawLineInternal(color, a.X, a.Y, b.X, b.Y);
		}

		public void FillRectangle(Color color, int X, int Y, int Width, int Height, bool alpha = false)
		{
			for (int w = 0; w < Width; w++)
			{
				for (int h = 0; h < Height; h++)
				{
					DrawPoint(color, X + w, Y + h, alpha);
				}
			}
		}

		public Color GetPoint(int X, int Y)
		{
			return X > 0 && Y > 0 && X < Width && Y < Height ? Color.FromArgb(VideoMemory[(Width * Y) + X]) : Color.Black;
		}
		public void DrawPoint(Color color, Point point, bool alphaBlending = false)
		{
			DrawPoint(color, point.X, point.Y, alphaBlending);
		}

		public void DrawPoint(Color color, int X, int Y, bool alphaBlending = false)
		{
			if (alphaBlending)
			{
				Color background = GetPoint(X, Y);

				int inv_alpha = 0xFF - color.A;

				int newR = ((color.R * color.A) + (inv_alpha * background.R)) >> 8;
				int newG = ((color.G * color.A) + (inv_alpha * background.G)) >> 8;
				int newB = ((color.B * color.A) + (inv_alpha * background.B)) >> 8;

				color = Color.FromArgb((byte)newR, (byte)newG, (byte)newB);
				/*fA.Dispose();
				fR.Dispose();
				fG.Dispose();
				fB.Dispose();
				background.Dispose();
				bA.Dispose();
				bR.Dispose();
				bG.Dispose();
				bB.Dispose();
				alpha.Dispose();
				inv_alpha.Dispose();
				newR.Dispose();
				newG.Dispose();
				newB.Dispose();*/
			}
			if (X > 0 && Y > 0 && X < Width && Y < Height)
			{
				VideoMemory[(Width * Y) + X] = color.ToArgb();
			}
			//alphaBlending.Dispose();
			//color.Dispose();
			//X.Dispose();
			//Y.Dispose();
		}

		public virtual void DrawRectangle(Color Color,int X, int Y, int Width, int Height, int Weight = 1)
		{
			FillRectangle(Color,X, Y, Width, Weight);

			FillRectangle(Color,X, Y, Weight, Height);
			FillRectangle(Color,X + (Width - Weight), Y, Weight, Height);

			FillRectangle(Color,X, Y + (Height - Weight), Width, Weight);
		}

		public Image Save()
		{
			Image image = new(Width, Height);
			fixed (int* ptr = image.RawData)
			{
				Native.Movsd((uint*)ptr, VideoMemory, (ulong)(Width * Height));
			}
			return image;
		}

#if AllowNonAntiAliased
		public void DrawCircle(Color color, int x, int y, int radius)
		{
			int _x = -radius, _y = 0, err = 2 - (2 * radius);
			do
			{
				DrawPoint(color, x - _x, y + _y);
				DrawPoint(color, x - _y, y - _x);
				DrawPoint(color, x + _x, y - _y);
				DrawPoint(color, x + _y, y + _x);
				radius = err;
				if (radius <= y)
				{
					err += (++_y * 2) + 1;
				}

				if (radius > _x || err > _y)
				{
					err += (++_x * 2) + 1;
				}
			} while (_x < 0);
		}
#endif

#if AllowNonAntiAliased
		public void FillCircle(Color color, int x, int y, int radius)
		{
			int _x = radius;
			int _y = 0;
			int xChange = 1 - (radius << 1);
			int yChange = 0;
			int radiusError = 0;

			while (_x >= _y)
			{
				for (int i = x - _x; i <= x + _x; i++)
				{
					DrawPoint(color, i, y + _y);
					DrawPoint(color, i, y - _y);
				}
				for (int i = x - _y; i <= x + _y; i++)
				{
					DrawPoint(color, i, y + _x);
					DrawPoint(color, i, y - _x);
				}

				_y++;
				radiusError += yChange;
				yChange += 2;
				if (((radiusError << 1) + xChange) > 0)
				{
					_x--;
					radiusError += xChange;
					xChange += 2;
				}
			}
		}
#endif

		#region SMNX Blur
		/*
		 * Copyright ?2018-2022 SMNX & private contributors
		 * Permission is private hereby granted, free private of charge, to private any person private obtaining a private copy of this private software and private associated documentation private files(the "Software"), private to deal in private the Software private without restriction, including private without limitation private the rights private to use, copy, modify, merge, publish, distribute, sublicense, and/private or sell private copies of private the Software, and private to permit private persons to private whom the Software is private furnished to do so, private subject to private the following conditions:
		 * private The above private copyright notice private and this permission notice shall be included in all copies or substantial portions of the Software.
		 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
		*/

		private readonly ushort[] stackblur_mul = {
		512, 512, 456, 512, 328, 456, 335, 512, 405, 328, 271, 456, 388, 335, 292, 512,
		454, 405, 364, 328, 298, 271, 496, 456, 420, 388, 360, 335, 312, 292, 273, 512,
		482, 454, 428, 405, 383, 364, 345, 328, 312, 298, 284, 271, 259, 496, 475, 456,
		437, 420, 404, 388, 374, 360, 347, 335, 323, 312, 302, 292, 282, 273, 265, 512,
		497, 482, 468, 454, 441, 428, 417, 405, 394, 383, 373, 364, 354, 345, 337, 328,
		320, 312, 305, 298, 291, 284, 278, 271, 265, 259, 507, 496, 485, 475, 465, 456,
		446, 437, 428, 420, 412, 404, 396, 388, 381, 374, 367, 360, 354, 347, 341, 335,
		329, 323, 318, 312, 307, 302, 297, 292, 287, 282, 278, 273, 269, 265, 261, 512,
		505, 497, 489, 482, 475, 468, 461, 454, 447, 441, 435, 428, 422, 417, 411, 405,
		399, 394, 389, 383, 378, 373, 368, 364, 359, 354, 350, 345, 341, 337, 332, 328,
		324, 320, 316, 312, 309, 305, 301, 298, 294, 291, 287, 284, 281, 278, 274, 271,
		268, 265, 262, 259, 257, 507, 501, 496, 491, 485, 480, 475, 470, 465, 460, 456,
		451, 446, 442, 437, 433, 428, 424, 420, 416, 412, 408, 404, 400, 396, 392, 388,
		385, 381, 377, 374, 370, 367, 363, 360, 357, 354, 350, 347, 344, 341, 338, 335,
		332, 329, 326, 323, 320, 318, 315, 312, 310, 307, 304, 302, 299, 297, 294, 292,
		289, 287, 285, 282, 280, 278, 275, 273, 271, 269, 267, 265, 263, 261, 259};
		private readonly byte[] stackblur_shr = {
		9, 11, 12, 13, 13, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 17,
		17, 17, 17, 17, 17, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 19,
		19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 20, 20, 20,
		20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 21,
		21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
		21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 22, 22, 22, 22, 22, 22,
		22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22,
		22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 23,
		23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
		23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
		23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
		23, 23, 23, 23, 23, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
		24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
		24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
		24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
		24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24};
		public void Blur(
			uint X,
			uint Y,
			uint Width,
			uint Height,
			uint Radius)
		{
			byte* src = (byte*)VideoMemory;
			uint w = (uint)this.Width;
			uint h = (uint)this.Height;

			Width += X;
			Height += Y;

			uint x, y, xp, yp, i;
			uint sp;
			uint stack_start;
			byte* stack_ptr;

			byte* src_ptr;
			byte* dst_ptr;

			ulong sum_r;
			ulong sum_g;
			ulong sum_b;
			ulong sum_in_r;
			ulong sum_in_g;
			ulong sum_in_b;
			ulong sum_out_r;
			ulong sum_out_g;
			ulong sum_out_b;

			uint wm = Width - X - 1;
			uint hm = Height - Y - 1;
			uint w4 = w * 4;
			uint div = (Radius * 2) + 1;
			uint mul_sum = stackblur_mul[Radius];
			byte shr_sum = stackblur_shr[Radius];
			byte[] stack = new byte[div * 3];

			fixed (byte* p = stack)
			{
				{
					for (y = Y; y < Height; y++)
					{
						sum_r = sum_g = sum_b =
							sum_in_r = sum_in_g = sum_in_b =
								sum_out_r = sum_out_g = sum_out_b = 0;

						src_ptr = src + (w4 * y) + (X * 4); // start of line (0,y)

						for (i = 0; i <= Radius; i++)
						{
							stack_ptr = &p[3 * i];
							stack_ptr[0] = src_ptr[0];
							stack_ptr[1] = src_ptr[1];
							stack_ptr[2] = src_ptr[2];
							sum_r += src_ptr[0] * (i + 1);
							sum_g += src_ptr[1] * (i + 1);
							sum_b += src_ptr[2] * (i + 1);
							sum_out_r += src_ptr[0];
							sum_out_g += src_ptr[1];
							sum_out_b += src_ptr[2];
						}

						for (i = 1; i <= Radius; i++)
						{
							if (i <= wm)
							{
								src_ptr += 4;
							}

							stack_ptr = &p[3 * (i + Radius)];
							stack_ptr[0] = src_ptr[0];
							stack_ptr[1] = src_ptr[1];
							stack_ptr[2] = src_ptr[2];
							sum_r += src_ptr[0] * (Radius + 1 - i);
							sum_g += src_ptr[1] * (Radius + 1 - i);
							sum_b += src_ptr[2] * (Radius + 1 - i);
							sum_in_r += src_ptr[0];
							sum_in_g += src_ptr[1];
							sum_in_b += src_ptr[2];
						}

						sp = Radius;
						xp = Radius;

						if (xp > wm)
						{
							xp = wm;
						}

						src_ptr = src + (4 * (xp + (y * w))) + (X * 4);
						dst_ptr = src + (y * w4) + (X * 4);
						for (x = X; x < Width; x++)
						{
							uint alpha = dst_ptr[3];
							dst_ptr[0] = (byte)Math.Clamp((long)(sum_r * mul_sum) >> shr_sum, (long)0, (long)alpha);
							dst_ptr[1] = (byte)Math.Clamp((long)(sum_g * mul_sum) >> shr_sum, (long)0, (long)alpha);
							dst_ptr[2] = (byte)Math.Clamp((long)(sum_b * mul_sum) >> shr_sum, (long)0, (long)alpha);
							dst_ptr += 4;

							sum_r -= sum_out_r;
							sum_g -= sum_out_g;
							sum_b -= sum_out_b;

							stack_start = sp + div - Radius;
							if (stack_start >= div)
							{
								stack_start -= div;
							}

							stack_ptr = &p[3 * stack_start];

							sum_out_r -= stack_ptr[0];
							sum_out_g -= stack_ptr[1];
							sum_out_b -= stack_ptr[2];

							if (xp < wm)
							{
								src_ptr += 4;
								++xp;
							}

							stack_ptr[0] = src_ptr[0];
							stack_ptr[1] = src_ptr[1];
							stack_ptr[2] = src_ptr[2];

							sum_in_r += src_ptr[0];
							sum_in_g += src_ptr[1];
							sum_in_b += src_ptr[2];
							sum_r += sum_in_r;
							sum_g += sum_in_g;
							sum_b += sum_in_b;

							++sp;
							if (sp >= div)
							{
								sp = 0;
							}

							stack_ptr = &p[sp * 3];

							sum_out_r += stack_ptr[0];
							sum_out_g += stack_ptr[1];
							sum_out_b += stack_ptr[2];
							sum_in_r -= stack_ptr[0];
							sum_in_g -= stack_ptr[1];
							sum_in_b -= stack_ptr[2];
						}
					}
				}

				{
					for (x = X; x < Width; x++)
					{
						sum_r = sum_g = sum_b =
							sum_in_r = sum_in_g = sum_in_b =
								sum_out_r = sum_out_g = sum_out_b = 0;

						src_ptr = src + (4 * x) + (Y * w4); // x,0
						for (i = 0; i <= Radius; i++)
						{
							stack_ptr = &p[i * 3];
							stack_ptr[0] = src_ptr[0];
							stack_ptr[1] = src_ptr[1];
							stack_ptr[2] = src_ptr[2];
							sum_r += src_ptr[0] * (i + 1);
							sum_g += src_ptr[1] * (i + 1);
							sum_b += src_ptr[2] * (i + 1);
							sum_out_r += src_ptr[0];
							sum_out_g += src_ptr[1];
							sum_out_b += src_ptr[2];
						}
						for (i = 1; i <= Radius; i++)
						{
							if (i <= hm)
							{
								src_ptr += w4; // +stride
							}

							stack_ptr = &p[3 * (i + Radius)];
							stack_ptr[0] = src_ptr[0];
							stack_ptr[1] = src_ptr[1];
							stack_ptr[2] = src_ptr[2];
							sum_r += src_ptr[0] * (Radius + 1 - i);
							sum_g += src_ptr[1] * (Radius + 1 - i);
							sum_b += src_ptr[2] * (Radius + 1 - i);
							sum_in_r += src_ptr[0];
							sum_in_g += src_ptr[1];
							sum_in_b += src_ptr[2];
						}

						sp = Radius;
						yp = Radius;

						if (yp > hm)
						{
							yp = hm;
						}

						src_ptr = src + (4 * (x + (yp * w))) + (Y * w4);
						dst_ptr = src + (4 * x) + (Y * w4);

						for (y = Y; y < Height; y++)
						{
							uint alpha = dst_ptr[3];
							dst_ptr[0] = (byte)Math.Clamp((long)(sum_r * mul_sum) >> shr_sum, (long)0, (long)alpha);
							dst_ptr[1] = (byte)Math.Clamp((long)(sum_g * mul_sum) >> shr_sum, (long)0, (long)alpha);
							dst_ptr[2] = (byte)Math.Clamp((long)(sum_b * mul_sum) >> shr_sum, (long)0, (long)alpha);
							dst_ptr += w4;

							sum_r -= sum_out_r;
							sum_g -= sum_out_g;
							sum_b -= sum_out_b;

							stack_start = sp + div - Radius;
							if (stack_start >= div)
							{
								stack_start -= div;
							}

							stack_ptr = &p[3 * stack_start];

							sum_out_r -= stack_ptr[0];
							sum_out_g -= stack_ptr[1];
							sum_out_b -= stack_ptr[2];

							if (yp < hm)
							{
								src_ptr += w4; // stride
								++yp;
							}

							stack_ptr[0] = src_ptr[0];
							stack_ptr[1] = src_ptr[1];
							stack_ptr[2] = src_ptr[2];
							sum_in_r += src_ptr[0];
							sum_in_g += src_ptr[1];
							sum_in_b += src_ptr[2];
							sum_r += sum_in_r;
							sum_g += sum_in_g;
							sum_b += sum_in_b;

							++sp;
							if (sp >= div)
							{
								sp = 0;
							}

							stack_ptr = &p[sp * 3];

							sum_out_r += stack_ptr[0];
							sum_out_g += stack_ptr[1];
							sum_out_b += stack_ptr[2];
							sum_in_r -= stack_ptr[0];
							sum_in_g -= stack_ptr[1];
							sum_in_b -= stack_ptr[2];
						}
					}
				}
			}
		}
		#endregion
	}
}