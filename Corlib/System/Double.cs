using System.Runtime.InteropServices;
using System.Text;

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
		public static extern void dtoa_(double value, byte* buffer);

		public override string ToString()
		{
			byte* p = stackalloc byte[22];
			dtoa_(this, p);
			return Encoding.ASCII.GetString(p);
		}
	}
}