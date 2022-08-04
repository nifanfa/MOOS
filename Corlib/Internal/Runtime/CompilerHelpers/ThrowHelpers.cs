using System.Runtime.InteropServices;
using Internal.TypeSystem;

namespace Internal.Runtime.CompilerHelpers
{
	public static class ThrowHelpers
	{
		[DllImport("Error")]
		private static extern void Error(string s, bool skippable = false);

		public static void ThrowInvalidProgramExceptionWithArgument(ExceptionStringID id, string methodName)
		{
			Error($"Invalid Program Exception With Argument: {methodName}", true);
		}

		public static void ThrowOverflowException()
		{
			Error("Overflow Exception", true);
		}

		public static void ThrowIndexOutOfRangeException()
		{
			Error("Index Out Of Range Exception", true);
		}

		public static void ThrowNullReferenceException()
		{
			Error("Null Reference Exception", true);
		}

		public static void ThrowDivideByZeroException()
		{
			Error("Divide By Zero Exception", true);
		}

		public static void ThrowArrayTypeMismatchException()
		{
			Error("Array Type Mismatch Exception", true);
		}

		public static void ThrowPlatformNotSupportedException(string message = "")
		{
			Error($"Platform Not Supported Exception{(message != "" ? $", {message}" : "")}", true);
		}

		public static void ThrowTypeLoadException()
		{
			Error("Type Load Exception", true);
		}

		public static void ThrowArgumentNullException(string argumentName = "")
		{
			Error($"Argument Null Exception{(argumentName != "" ? $", Argument name: {argumentName}" : "")}", true);
		}

		public static void ThrowArgumentException(string argumentName = null, string extraInfo = null)
		{
			Error($"Argument Exception{(argumentName != null ? $", Argument name: {argumentName}" : "")}{extraInfo ?? ""}", true);
		}

		public static void ThrowArgumentOutOfRangeException(string argumentName = null, string extraInfo = null)
		{
			Error($"Argument Out Of Range Exception{(argumentName != null ? $", Argument name: {argumentName}" : "")}{extraInfo ?? ""}", true);
		}

		public static void ThrowTypeLoadException(ExceptionStringID id, string typeName, string assemblyName, string messageArg)
		{
			Error($"Type Load Exception: {typeName} {assemblyName} {messageArg}", true);
		}

		public static void ThrowTypeLoadException(ExceptionStringID id, string typeName, string assemblyName)
		{
			Error($"Type Load Exception: {typeName} {assemblyName}", true);
		}

		public static void ThrowMissingMethodException(object owningType, string methodName, object signature)
		{
			Error($"Missing Method Exception: {methodName}", true);
		}

		public static void ThrowMissingFieldException(object owningType, string fieldName)
		{
			Error($"Missing Field Exception: {fieldName}", true);
		}

		public static void ThrowFileNotFoundException(ExceptionStringID id, string fileName)
		{
			Error($"File Not Found Exception: {fileName}", true);
		}

		public static void ThrowInvalidProgramException()
		{
			Error($"Invalid Program Exception", true);
		}

		public static void ThrowInvalidProgramException(ExceptionStringID id, object method)
		{
			Error($"Invalid Program Exception", true);
		}

		public static void ThrowBadImageFormatException()
		{
			Error($"Bad Image Format Exception", true);
		}
	}
}