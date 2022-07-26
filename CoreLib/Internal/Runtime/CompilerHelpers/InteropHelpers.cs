#if Kernel
using MOOS;
using MOOS.Misc;
#endif
using System;
using System.Runtime.InteropServices;

namespace Internal.Runtime.CompilerHelpers
{
	internal static class InteropHelpers
	{
		[StructLayout(LayoutKind.Sequential)]
		internal unsafe struct MethodFixupCell
		{
			public IntPtr Target;
			public IntPtr MethodName;
			public ModuleFixupCell* Module;
			public CharSet CharSetMangling;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal unsafe struct ModuleFixupCell
		{
			public IntPtr Handle;
			public IntPtr ModuleName;
			public EETypePtr CallingAssemblyType;
			public uint DllImportSearchPathAndCookie;
		}

		internal static unsafe IntPtr ResolvePInvoke(MethodFixupCell* pCell)
		{
#if Kernel
			Console.Write("Method Name: ");
			Console.WriteLine(string.FromASCII(pCell->Module->ModuleName, strings.strlen((byte*)pCell->Module->ModuleName)));
			//Return the pointer of method
			return (IntPtr)(delegate*<void>)&Hello;
#else
	uint int0x80 = 0xC380CD;
	uint* ptr = &int0x80;
	return ((delegate*<MethodFixupCell*, IntPtr>)ptr)(pCell);
#endif
		}

#if Kernel
		internal static void Hello()
		{
			Panic.Error("Not implemented, check out Internal.Runtime.CompilerHelpers.InteropHelpers");
		}
#endif

		internal static unsafe string StringToAnsiString(string str, bool bestFit, bool throwOnUnmappableChar)
		{
			//No Ansi support, Return unicode
			return str;
		}

		public static unsafe char WideCharToAnsiChar(char managedValue, bool bestFit, bool throwOnUnmappableChar)
		{
			//No Ansi support, Return unicode
			return managedValue;
		}

		internal static unsafe void CoTaskMemFree(void* p)
		{
			//TO-DO
		}
	}
}