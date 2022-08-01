namespace System.Runtime.InteropServices
{
	[AttributeUsage(AttributeTargets.All)]
	public sealed class UnmanagedCallersOnlyAttribute : Attribute
	{
		public string EntryPoint;
		public CallingConvention CallingConvention;

		public UnmanagedCallersOnlyAttribute() { }
	}
}