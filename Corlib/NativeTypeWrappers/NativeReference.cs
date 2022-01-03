using System;
using System.Runtime.InteropServices;

using Internal.Runtime.CompilerServices;

namespace NativeTypeWrappers {
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct ReadonlyNativeReference<T> where T : unmanaged {
		readonly IntPtr _pointer;

		/// <summary>Gets the object pointed to by the native pointer as a reference.</summary>
		public ref T Ref => ref Unsafe.AsRef<T>(_pointer);
	}
}