using System;
using System.Runtime.InteropServices;

namespace Internal.Runtime.CompilerHelpers
{
	internal static class InteropHelpers
	{
		[StructLayout(LayoutKind.Sequential)]
		public unsafe struct MethodFixupCell
		{
			public IntPtr Target;
			public IntPtr MethodName;
			public ModuleFixupCell* Module;
			public CharSet CharSetMangling;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ModuleFixupCell
		{
			public IntPtr Handle;
			public IntPtr ModuleName;
			public EETypePtr CallingAssemblyType;
			public uint DllImportSearchPathAndCookie;
		}

		public static unsafe IntPtr ResolvePInvoke(MethodFixupCell* pCell)
		{

			uint int0x80 = 0xC380CD;
			uint* ptr = &int0x80;
			return ((delegate*<MethodFixupCell*, IntPtr>)ptr)(pCell);
		}

		public static string StringToAnsiString(string str, bool bestFit, bool throwOnUnmappableChar)
		{
			//No Ansi support, Return unicode
			return str;
		}

		public static char WideCharToAnsiChar(char managedValue, bool bestFit, bool throwOnUnmappableChar)
		{
			//No Ansi support, Return unicode
			return managedValue;
		}

		public static unsafe void CoTaskMemFree(void* p)
		{
			//TO-DO
		}
	}
}