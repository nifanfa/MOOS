namespace System.Runtime.InteropServices
{
	// Custom attribute that marks a class as having special "Call" intrinsics.
	[AttributeUsage(AttributeTargets.All)]
	internal class McgIntrinsicsAttribute : Attribute { }
}