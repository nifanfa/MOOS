using System.Runtime.InteropServices;

namespace System
{
	public unsafe struct Double
	{

		public const double MinValue = -1.7976931348623157E+308;
		public const double MaxValue = 1.7976931348623157E+308;
		public static unsafe bool IsNaN(double d)
		{
			// A NaN will never equal itself so this is an
			// easy and efficient way to check for NaN.

#pragma warning disable CS1718
			return d != d;
#pragma warning restore CS1718
		}

		[DllImport("*")]
		public static extern void double_tostring(byte* buffer, double value);

		public override string ToString()
		{
			char* p = stackalloc char[22];
			byte* buffer = stackalloc byte[22];
			double_tostring(buffer, this);
			int length = 0;
			while (buffer[length] != 0)
			{
				p[length] = (char)buffer[length];
				length++;
			}

			string s = new(p, 0, length);
			return s;
		}
	}
}