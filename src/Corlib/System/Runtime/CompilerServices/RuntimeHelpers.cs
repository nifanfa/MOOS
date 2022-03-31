/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/


namespace System.Runtime.CompilerServices
{
    public class RuntimeHelpers
    {
        public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);
    }
}
