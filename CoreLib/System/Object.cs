using System.Runtime.InteropServices;
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System
{
	public unsafe class Object
	{
		// The layout of object is a contract with the compiler.
		internal unsafe EEType* m_pEEType;

		[StructLayout(LayoutKind.Sequential)]
		private class RawData
		{
			public byte Data;
		}

		internal ref byte GetRawData()
		{
			return ref Unsafe.As<RawData>(this).Data;
		}

		internal uint GetRawDataSize()
		{
			return m_pEEType->BaseSize - (uint)sizeof(ObjHeader) - (uint)sizeof(EEType*);
		}

		public Object() { }
		~Object() { }


		public virtual bool Equals(object o)
		{
			return false;
		}

		public virtual int GetHashCode()
		{
			return 0;
		}

		public virtual string ToString()
		{
			return "System.Object";
		}

		public virtual void Dispose()
		{
			object obj = this;
			free(Unsafe.As<object, IntPtr>(ref obj));
		}

		[DllImport("*")]
		private static extern ulong free(nint ptr);

		public static implicit operator bool(object obj)
		{
			return obj != null;
		}
	}
}