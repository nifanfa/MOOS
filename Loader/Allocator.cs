using System;

namespace Kernel 
{
	internal static unsafe class Allocator
	{
		public static IntPtr Allocate(ulong size)
		{
			IntPtr ptr = default;
			EFI.EFI.ST->BootServices->AllocatePool(EFI.MemoryType.LoaderData, size, &ptr);

			return ptr;
		}

		public static void Free(IntPtr buf)
		{
			EFI.EFI.ST->BootServices->FreePool(buf);
		}

		public unsafe static void Print(string msg)
		{
			fixed (char* c = msg)
				EFI.EFI.ST->ConOut.Ref.OutputString(c);
		}

		public unsafe static void Print(char c)
		{
			EFI.EFI.ST->ConOut.Ref.OutputString(&c);
		}

		public unsafe static void Print(ushort c)
		{
			string s = ((ulong)c).ToString();
			fixed (char* p = s)
				EFI.EFI.ST->ConOut.Ref.OutputString(p);
			s.Dispose();
		}

		public unsafe static void Print(char* msg, int len)
		{
			var s = new string(msg, 0, len);
			Print(s);
			s.Dispose();
		}

		public unsafe static void PrintLine(string msg)
		{
			Print(msg);

			char* x = stackalloc char[3];
			x[0] = '\r';
			x[1] = '\n';
			x[2] = '\0';
			EFI.EFI.ST->ConOut.Ref.OutputString(x);
		}

		public unsafe static void PrintLine()
		{
			char* x = stackalloc char[3];
			x[0] = '\r';
			x[1] = '\n';
			x[2] = '\0';
			EFI.EFI.ST->ConOut.Ref.OutputString(x);
		}

		//public unsafe static void PrintLine(char* msg, int len) {
		//	Print(msg, len);

		//	char* x = stackalloc char[3];
		//	x[0] = '\r';
		//	x[1] = '\n';
		//	x[2] = '\0';
		//	Print(x, 2);
		//}

		//public unsafe static char ReadKey() {
		//	char* x = stackalloc char[2];
		//	x[1] = '\0';
		//	int read = 0;

		//	Win32.ReadConsoleW(StdIn, (IntPtr)x, 1, (IntPtr)(&read), IntPtr.Zero);

		//	return x[0];
		//}

		public static void ClearConsole()
		{
			EFI.EFI.ST->ConOut.Ref.ClearScreen();
		}

		public static unsafe void ZeroFill(IntPtr ptr, ulong len)
		{
			EFI.EFI.ST->BootServices->SetMem(ptr, len, 0);
		}

		public static unsafe void MemoryCopy(IntPtr dst, IntPtr src, ulong len)
		{
			EFI.EFI.ST->BootServices->CopyMem(dst, src, len);
		}
	}
}