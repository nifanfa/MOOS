using System;
using System.Drawing;

namespace MOOS.Graph
{
	public sealed unsafe class Graphics
	{
		public uint* VideoMemory;
		public int Width;
		public int Height;

		public Graphics(int width, int height, uint* vm)
		{
			Width = width;
			Height = height;
			VideoMemory = vm;
		}

		public static Graphics FromImage(Image img)
		{
			fixed (uint* ptr = img.RawData)
			{
				return new Graphics(img.Width, img.Height, ptr);
			}
		}

		public void Clear(Color Color)
		{
			Native.Stosd(VideoMemory, Color.ToArgb(), (ulong)(Width * Height));
		}

		public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
		{
			for (int X = 0; X < blockRegionSize.Width; X++)
			{
				for (int Y = 0; Y < blockRegionSize.Height; Y++)
				{
					DrawPoint(X + destinationX, Y + destinationY, GetPoint(X + sourceX, Y + sourceY));
				}
			}
		}

		public void CopyFromScreen(Point source, Point destination, Size blockRegionSize)
		{
			for (int X = 0; X < blockRegionSize.Width; X++)
			{
				for (int Y = 0; Y < blockRegionSize.Height; Y++)
				{
					DrawPoint(X + destination.X, Y + destination.Y, GetPoint(X + source.X, Y + source.Y));
				}
			}
		}

		// TODO: DrawArc
		// TODO: DrawBeizer
		// TODO: DrawBeizers
		// TODO: DrawClosedCurve
		// TODO: DrawCurve
		// TODO: DrawElipse

		private void DrawImageInternal(Image image, int X, int Y, int cutWidth, int cutHeight, int startX, int startY, bool AlphaBlending)
		{
			if (AlphaBlending)
			{
				for (int h = startY; h < cutHeight; h++)
				{
					for (int w = startX; w < cutWidth; w++)
					{
						uint foreground = image.RawData[(cutWidth * h) + w];
						int fA = (byte)((foreground >> 24) & 0xFF);

						if (fA != 0)
						{
							//if (w > image.Width || h > image.Height || w <= image.Width || h <= image.Height)
							//{
							//								foreground = 0xFFFFFF;
							//}
							DrawPoint(X + w, Y + h, foreground, true);
						}
					}
				}
			} else
			{
				for (int h = startY; h < cutHeight; h++)
				{
					for (int w = startX; w < cutWidth; w++)
					{
						uint color = image.RawData[(cutWidth * h) + w];
						//if (w > image.Width || h > image.Height || w <= image.Width || h <= image.Height)
						//{
						//							color = 0xFFFFFF;
						//}
						DrawPoint(X + w, Y + h, color);
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

		// TODO: Finish GDI+

		public void FillRectangle(int X, int Y, int Width, int Height, uint Color)
		{
			for (int w = 0; w < Width; w++)
			{
				for (int h = 0; h < Height; h++)
				{
					DrawPoint(X + w, Y + h, Color);
				}
			}
		}

		public uint GetPoint(int X, int Y)
		{
			return X > 0 && Y > 0 && X < Width && Y < Height ? VideoMemory[(Width * Y) + X] : 0;
		}

		public void DrawPoint(int X, int Y, uint color, bool alphaBlending = false)
		{
			if (alphaBlending)
			{
				int fA = (byte)((color >> 24) & 0xFF);
				int fR = (byte)((color >> 16) & 0xFF);
				int fG = (byte)((color >> 8) & 0xFF);
				int fB = (byte)(color & 0xFF);

				uint background = GetPoint(X, Y);
				int bA = (byte)((background >> 24) & 0xFF);
				int bR = (byte)((background >> 16) & 0xFF);
				int bG = (byte)((background >> 8) & 0xFF);
				int bB = (byte)(background & 0xFF);

				int alpha = fA;
				int inv_alpha = 0xFF - alpha;

				int newR = ((fR * alpha) + (inv_alpha * bR)) >> 8;
				int newG = ((fG * alpha) + (inv_alpha * bG)) >> 8;
				int newB = ((fB * alpha) + (inv_alpha * bB)) >> 8;

				color = Color.FromArgb((byte)newR, (byte)newG, (byte)newB).ToArgb();
			}
			if (X > 0 && Y > 0 && X < Width && Y < Height)
			{
				VideoMemory[(Width * Y) + X] = color;
			}
		}

		public void DrawRectangle(int X, int Y, int Width, int Height, uint Color)
		{
			DrawLine(X, Y, X + Width, Y, Color);
			DrawLine(X, Y + Height, X + Width, Y + Height, Color);
			DrawLine(X, Y, X, Y + Height, Color);
			DrawLine(X + Width, Y, X + Width, Y + Height, Color);
		}

		public Image Save()
		{
			Image image = new(Width, Height);
			fixed (uint* ptr = image.RawData)
			{
				Native.Movsd(ptr, VideoMemory, (ulong)(Width * Height));
			}
			return image;
		}

		/*public void DrawCircle(int x, int y, int radius, uint color)
		{
			double i, angle, x1, y1;

			for (i = 0; i < 360; i += 0.1)
			{
				angle = i;
				x1 = radius * Math.Cos(angle * Math.PI / 180);
				y1 = radius * Math.Sin(angle * Math.PI / 180);
				DrawPoint((int)(x + x1), (int)(y + y1), color, true);
			}
		}*/
		public void DrawCircle(int x_center, int y_center, int radius, uint color)
		{
			int x = radius;
			int y = 0;
			int e = 0;

			while (x >= y)
			{
				DrawPoint(x_center + x, y_center + y, color);
				DrawPoint(x_center + y, y_center + x, color);
				DrawPoint(x_center - y, y_center + x, color);
				DrawPoint(x_center - x, y_center + y, color);
				DrawPoint(x_center - x, y_center - y, color);
				DrawPoint(x_center - y, y_center - x, color);
				DrawPoint(x_center + y, y_center - x, color);
				DrawPoint(x_center + x, y_center - y, color);

				y++;
				if (e <= 0)
				{
					e += (2 * y) + 1;
				}
				if (e > 0)
				{
					x--;
					e -= (2 * x) + 1;
				}
			}
		}
		public void FillCircle(int x0, int y0, int radius, uint color)
		{
			for (int x = -radius; x < radius; x++)
			{
				int height = (int)Math.Sqrt((radius * radius) - (x * x));

				for (int y = -height; y < height; y++)
				{
					DrawPoint(x + x0, y + y0, color);
				}
			}
		}

		public void DrawLine(int x0, int y0, int x1, int y1, uint color)
		{
			void Swap(int* a, int* b)
			{
				(*b, *a) = (*a, *b);
			}

			float FPartOfNumber(float x)
			{
				return x > 0 ? x - (int)x : x - ((int)x + 1);
			}

			float RFPartOfNumber(float x)
			{
				return 1 - FPartOfNumber(x);
			}

			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

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
			{
				gradient = 1;
			}

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

					Color A = Color.FromArgb(color);

					A.A = (byte)(1f - RFPartOfNumber(intersectY));
					DrawPoint((int)intersectY, x, A.ToArgb());

					A.A = (byte)(1f - FPartOfNumber(intersectY));
					DrawPoint((int)intersectY - 1, x, A.ToArgb());

					A.Dispose();

					intersectY += gradient;
				}
			} else
			{
				int x;
				for (x = xpxl1; x <= xpxl2; x++)
				{
					// pixel coverage is determined by fractional
					// part of y co-ordinate

					Color B = Color.FromArgb(color);

					B.A = (byte)(1f - RFPartOfNumber(intersectY));
					DrawPoint(x, (int)intersectY, B.ToArgb());

					B.A = (byte)(1f - FPartOfNumber(intersectY));
					DrawPoint(x, (int)intersectY - 1, B.ToArgb());

					B.Dispose();
					intersectY += gradient;
				}
			}

		}

		#region SMNX Blur
		/*
		 * Copyright © 2018-2022 SMNX & private contributors
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
							dst_ptr[0] = (byte)Math.Clamp((long)(sum_r * mul_sum) >> shr_sum, 0, alpha);
							dst_ptr[1] = (byte)Math.Clamp((long)(sum_g * mul_sum) >> shr_sum, 0, alpha);
							dst_ptr[2] = (byte)Math.Clamp((long)(sum_b * mul_sum) >> shr_sum, 0, alpha);
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
							dst_ptr[0] = (byte)Math.Clamp((long)(sum_r * mul_sum) >> shr_sum, 0, alpha);
							dst_ptr[1] = (byte)Math.Clamp((long)(sum_g * mul_sum) >> shr_sum, 0, alpha);
							dst_ptr[2] = (byte)Math.Clamp((long)(sum_b * mul_sum) >> shr_sum, 0, alpha);
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