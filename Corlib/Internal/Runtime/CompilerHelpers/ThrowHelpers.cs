using Internal.TypeSystem;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Internal.Runtime.CompilerHelpers
{
    public static class ThrowHelpers
    {
        [DllImport("Error")]
        public static extern void Error(string s, bool skippable);

        public static void ThrowInvalidProgramExceptionWithArgument(ExceptionStringID id, string methodName)
        {
            Error($"Invalid Program Exception With Argument: {methodName}", true);
        }

        private static void ThrowOverflowException()
        {
            Error("Overflow Exception", true);
        }

        private static void ThrowIndexOutOfRangeException()
        {
            Error("Index Out Of Range Exception", true);
        }

        private static void ThrowNullReferenceException()
        {
            Error("Null Reference Exception", true);
        }

        private static void ThrowDivideByZeroException()
        {
            Error("Divide By Zero Exception", true);
        }

        private static void ThrowArrayTypeMismatchException()
        {
            Error("Array Type Mismatch Exception", true);
        }

        private static void ThrowPlatformNotSupportedException()
        {
            Error("Platform Not Supported Exception", true);
        }

        private static void ThrowTypeLoadException()
        {
            Error("Type Load Exception", true);
        }

        private static void ThrowArgumentException()
        {
            Error("Argument Exception", true);
        }

        private static void ThrowArgumentOutOfRangeException()
        {
            Error("Argument Out Of Range Exception", true);
        }

        private static void ThrowTypeLoadException(ExceptionStringID id, string typeName, string assemblyName, string messageArg)
        {
            Error($"Type Load Exception: {typeName} {assemblyName} {messageArg}", true);
        }

        private static void ThrowTypeLoadException(ExceptionStringID id, string typeName, string assemblyName)
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