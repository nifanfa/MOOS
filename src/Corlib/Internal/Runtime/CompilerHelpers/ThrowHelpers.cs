// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using Internal.TypeSystem;

namespace Internal.Runtime.CompilerHelpers
{
    public static class ThrowHelpers
    {
        public static void ThrowInvalidProgramException(ExceptionStringID id) { }
        public static void ThrowInvalidProgramExceptionWithArgument(ExceptionStringID id, string methodName) { }
        public static void ThrowOverflowException() { }
        public static void ThrowIndexOutOfRangeException() { }
        public static void ThrowTypeLoadException(ExceptionStringID id, string className, string typeName) { }
    }
}
