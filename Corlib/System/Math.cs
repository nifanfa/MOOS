namespace System
{
	public static class Math
	{
		public const double PI = 3.14159265358979323846;
		public const double E = 2.7182818284590452354;
		public const double Tau = 6.2831853071795862;
		public static byte Abs(byte value)
		{
			return (byte)(value < 0 ? -value : value);
		}
		public static double Abs(double value)
		{
			return value < 0 ? -value : value;
		}
		public static short Abs(short value)
		{
			return (short)(value < 0 ? -value : value);
		}
		public static int Abs(int value)
		{
			return value < 0 ? -value : value;
		}
		public static long Abs(long value)
		{
			return value < 0 ? -value : value;
		}
		public static nint Abs(nint value)
		{
			return value < 0 ? -value : value;
		}
		public static sbyte Abs(sbyte value)
		{
			return (sbyte)(value < 0 ? -value : value);
		}
		public static float Abs(float value)
		{
			return value < 0 ? -value : value;
		}
		public static byte Max(byte val1, byte val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static double Max(double val1, double val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static short Max(short val1, short val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static int Max(int val1, int val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static long Max(long val1, long val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static nint Max(nint val1, nint val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static sbyte Max(sbyte val1, sbyte val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static float Max(float val1, float val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static ushort Max(ushort val1, ushort val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static uint Max(uint val1, uint val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static ulong Max(ulong val1, ulong val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static nuint Max(nuint val1, nuint val2)
		{
			return val1 < val2 ? val2 : val1;
		}
		public static byte Min(byte val1, byte val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static double Min(double val1, double val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static short Min(short val1, short val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static int Min(int val1, int val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static long Min(long val1, long val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static nint Min(nint val1, nint val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static sbyte Min(sbyte val1, sbyte val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static float Min(float val1, float val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static ushort Min(ushort val1, ushort val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static uint Min(uint val1, uint val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static ulong Min(ulong val1, ulong val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static nuint Min(nuint val1, nuint val2)
		{
			return val1 > val2 ? val2 : val1;
		}
		public static byte Clamp(byte value, byte min, byte max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static double Clamp(double value, double min, double max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static short Clamp(short value, short min, short max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static int Clamp(int value, int min, int max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static long Clamp(long value, long min, long max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static nint Clamp(nint value, nint min, nint max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static sbyte Clamp(sbyte value, sbyte min, sbyte max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static float Clamp(float value, float min, float max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static ushort Clamp(ushort value, ushort min, ushort max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static uint Clamp(uint value, uint min, uint max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static ulong Clamp(ulong value, ulong min, ulong max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static nuint Clamp(nuint value, nuint min, nuint max)
		{
			return value < min ? min : value > max ? max : value;
		}
		public static double MinMagnitude(double x, double y)
		{
			return Abs(x) > Abs(y) ? y : x;
		}
		public static double MaxMagnitude(double x, double y)
		{
			return Abs(x) < Abs(y) ? y : x;
		}
		public static int Sign(nuint value)
		{
			return value < (nuint)0 ? -1 : value == (nuint)0 ? 0 : 1;
		}
		public static int Sign(float value)
		{
			return value < 0 ? -1 : value == 0 ? 0 : 1;
		}
		public static int Sign(sbyte value)
		{
			return value < 0 ? -1 : value == 0 ? 0 : 1;
		}
		public static int Sign(long value)
		{
			return value < 0 ? -1 : value == 0 ? 0 : 1;
		}
		public static int Sign(short value)
		{
			return value < 0 ? -1 : value == 0 ? 0 : 1;
		}
		public static int Sign(double value)
		{
			return value < 0 ? -1 : value == 0 ? 0 : 1;
		}
		public static int Sign(int value)
		{
			return value < 0 ? -1 : value == 0 ? 0 : 1;
		}

		public static double Pow(double a, double b)
		{
			double c = 1;
			for (int i = 0; i < b; i++)
			{
				c *= a;
			}

			return c;
		}

		public static double Fact(double x)
		{
			double ret = 1;
			for (int i = 1; i <= x; i++)
			{
				ret *= i;
			}

			return ret;
		}

		public static double Sin(double x)
		{
			double y = x;
			double s = -1;
			for (int i = 3; i <= 100; i += 2)
			{
				y += s * (Pow(x, i) / Fact(i));
				s *= -1;
			}
			return y;
		}

		public static double Cos(double x)
		{
			double y = 1;
			double s = -1;
			for (int i = 2; i <= 100; i += 2)
			{
				y += s * (Pow(x, i) / Fact(i));
				s *= -1;
			}
			return y;
		}
		public static double Tan(double x)
		{
			return Sin(x) / Cos(x);
		}

		public static unsafe double Sqrt(double x)
		{
			double w = x, h = 1, t;
			if (w < 1)
			{
				h = x;
				w = 1;
			}
			do
			{
				w *= 0.5;
				h += h;
			} while (w > h);
			for (int i = 0; i < 4; i++)
			{
				t = (w + h) * 0.5;
				h = h / t * w;
				w = t;
			}
			return (w + h) * 0.5;
		}

		public static double Round(double number, int decimal_places)
		{
			if (decimal_places <= 0)
			{
				return number;
			}

			double power = Pow(10, decimal_places - 1);
			number *= power;

			return (number >= 0) ? ((int)(number + 0.5)) / power : ((int)(number - 0.5)) / power;
		}

		public static int Ceiling(double val)
		{
			return (int)((val + 10 - 1) / 10);
		}

		public static double Floor(double x)
		{
			if (x >= 0.0)
			{
				return x < (((long.MaxValue / 2) + 1) * 2.0) ? (long)x : x;
			} else if (x < 0.0)
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
	}
}
