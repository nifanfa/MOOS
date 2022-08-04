using System.Runtime.InteropServices;

namespace System
{
	public static unsafe class MathF
	{
		public const float E = 2.71828175F;
		public const float PI = 3.14159274F;
		public const float Tau = 6.28318548F;

		[DllImport("*")]
		private static extern void _fabs(float* x);

		[DllImport("*")]
		private static extern void _fyl2x(float* y, float* x);

		[DllImport("*")]
		private static extern void _fsqrt(float* x);

		[DllImport("*")]
		private static extern void _fcos(float* x);

		[DllImport("*")]
		private static extern void _fsin(float* x);

		[DllImport("*")]
		private static extern void _frndint(float* x);

		public static float Max(float x, float y)
		{
			return x < y ? y : x;
		}
		public static float Min(float x, float y)
		{
			return x > y ? y : x;
		}
		public static float Sqrt(float x)
		{
			_fsqrt(&x);
			return x;
		}

		public static float Abs(float x)
		{
			_fabs(&x);
			return x;
		}

		public static float Log2(float n)
		{
			float y = 1F;
			_fyl2x(&y, &n);
			return n;
		}

		public static float Log(float x)
		{
			return Log2(x) / Log2(E);
		}

		public static float Log10(float x)
		{
			return Log2(x) / Log2(10);
		}

		public static float Log(float a, float b)
		{
			return Log2(a) / Log2(b);
		}

		public static int Sign(float x)
		{
			return x < 0 ? -1 : x == 0 ? 0 : 1;
		}

		public static float Acos(float x)
		{
			float negate = Sign(x);
			x = Abs(x);
			float ret = -0.0187293F;
			ret *= x;
			ret += 0.0742610F;
			ret *= x;
			ret -= 0.2121144F;
			ret *= x;
			ret += 1.5707288F;
			ret *= Sqrt(1.0F - x);
			ret -= 2 * negate * ret;
			return (negate * PI) + ret;
		}

		public static float Acosh(float x)
		{
			return Log(x + Sqrt(Pow(x, 2) - 1));
		}

		public static float Pow(float x, float y)
		{
			return Exp(Log(x) * y);
		}

		public static float Exp(float x)
		{
			float sum = 1;

			for (int i = 20 - 1; i > 0; --i)
			{
				sum = 1 + (x * sum / i);
			}

			return sum;
		}

		public static float Asin(float x)
		{
			float negate = Sign(x);
			x = Abs(x);
			float ret = -0.0187293F;
			ret *= x;
			ret += 0.0742610F;
			ret *= x;
			ret -= 0.2121144F;
			ret *= x;
			ret += 1.5707288F;
			ret = (PI * 0.5F) - (Sqrt(1.0F - x) * ret);
			return ret - (2 * negate * ret);
		}
		public static float Asinh(float x)
		{
			return Log(x + Sqrt(Pow(x, 2) + 1));
		}

		public static float Atan2(float y, float x)
		{
			float t0,
			t1,
			t3,
			t4;

			t3 = Abs(x);
			t1 = Abs(y);
			t0 = Max(t3, t1);
			t1 = Min(t3, t1);
			t3 = 1.0F / t0;
			t3 = t1 * t3;

			t4 = t3 * t3;
			t0 = -0.013480470F;
			t0 = (t0 * t4) + 0.057477314F;
			t0 = (t0 * t4) - 0.121239071F;
			t0 = (t0 * t4) + 0.195635925F;
			t0 = (t0 * t4) - 0.332994597F;
			t0 = (t0 * t4) + 0.999995630F;
			t3 = t0 * t3;

			t3 = (Abs(y) > Abs(x)) ? 1.570796327F - t3 : t3;
			t3 = (x < 0) ? PI - t3 : t3;
			t3 = (y < 0) ? -t3 : t3;

			return t3;
		}
		public static float Atan(float x)
		{
			return Atan2(x, 1F);
		}
		public static float Atanh(float x)
		{
			return 0.5F * Log((1 + x) / (1 - x));
		}
		public static float Cbrt(float x)
		{
			return Pow(x, 0.3333333F);
		}
		public static float Ceiling(float x)
		{
			return (float)((x + 10 - 1) / 10);
		}
		public static float Cos(float x)
		{
			_fcos(&x);
			return x;
		}
		public static float Cosh(float x)
		{
			return 0.5F * (Exp(x) + Exp(-x));
		}
		public static float Floor(float x)
		{
			if (x >= 0.0F)
			{
				return x < (((long.MaxValue / 2) + 1) * 2.0F) ? (long)x : x;
			} else if (x < 0.0F)
			{
				if (x >= long.MinValue)
				{
					long ix = (long)x;
					return (ix == x) ? x : ix - 1;
				}
				return x;
			}
			return x;
		}
		/// <summary>
		/// WARNING!! This method is non-native and not optimized, it computes `(x * y) + z` with no extra steps
		/// </summary>
		public static float FusedMultiplyAdd(float x, float y, float z)
		{
			return (x * y) + z;
		}

		public static int Magnitude(float x)
		{
			return x > 0 ? 1 : -1;
		}

		public static float MinMagnitude(float x, float y)
		{
			int xMag = Magnitude(x);
			int yMag = Magnitude(y);
			return Min(xMag, yMag);
		}
		public static double MaxMagnitude(float x, float y)
		{
			int xMag = Magnitude(x);
			int yMag = Magnitude(y);
			return Max(xMag, yMag);
		}

		public static float ReciprocalEstimate(float x)
		{
			return 1.0F / x;
		}

		/// <summary>
		/// This is the "Fast Inverse Square Root" from Quake III
		/// </summary>
		public static float ReciprocalSqrtEstimate(float x)
		{
			long i;
			float x2,
			y;

			x2 = x * 0.5F;
			y = x;
			i = *(long*)&y;
			i = 0x5f3759df - (i >> 1);
			y = *(float*)&i;
			y *= 1.5F - (x2 * y * y);

			return y;
		}

		public static float Round(float x)
		{
			_frndint(&x);
			return x;
		}

		/// <summary>
		/// This goes against the MS Documentation on MathF.ScaleB, it is not calculated efficentially
		/// </summary>
		public static float ScaleB(float x, int n)
		{
			return x * Pow(2, n);
		}
		public static float Sin(float x)
		{
			_fsin(&x);
			return x;
		}
		public static (float Sin, float Cos) SinCos(float x)
		{
			return (Sin(x), Cos(x));
		}
		public static float Sinh(float x)
		{
			return 0.5F * (Exp(x) - Exp(-x));
		}
		public static float Tan(float x)
		{
			return Sin(x) / Cos(x);
		}
		public static float Tanh(float x)
		{
			float exp2x = Exp(2 * x);
			return (exp2x - 1) / (exp2x + 1);
		}
	}
}