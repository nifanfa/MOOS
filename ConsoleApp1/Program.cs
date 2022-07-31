using System.Runtime;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
	internal static unsafe class Program
	{

		/// <see cref="MOOS.Api"/>
		/// <see cref="Internal.Runtime.CompilerHelpers"/>

		[DllImport("Allocate")]
		public static extern nint Allocate(ulong size);

		[DllImport("Write")]
		public static extern void Write(string c);

		[DllImport("SingleBuffered")]
		public static extern void SingleBuffered();

		[DllImport("DoubleBuffered")]
		public static extern void DoubleBuffered();

		[DllImport("Sleep")]
		public static extern void Sleep(ulong ms);

		[RuntimeExport("malloc")]
		public static nint malloc(ulong size)
		{
			return Allocate(size);
		}

		[RuntimeExport("Main")]
		public static void Main()
		{
			SingleBuffered();
			Write("Hello, World From ConsoleApp1.exe!");
			Sleep(2000);
			DoubleBuffered();
		}
	}
}