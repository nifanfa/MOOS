namespace System.Runtime {
	// Custom attribute that the compiler understands that instructs it
	// to export the method under the given symbolic name.
	internal sealed class RuntimeExportAttribute : Attribute {
		public RuntimeExportAttribute(string entry) { }
	}
}