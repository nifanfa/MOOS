
namespace System.Runtime.InteropServices {
	public sealed class StructLayoutAttribute : Attribute {
		public StructLayoutAttribute(LayoutKind layoutKind) {
			Value = layoutKind;
		}

		public LayoutKind Value { get; }

		public int Pack;
		public int Size;
		public CharSet CharSet;
	}
}