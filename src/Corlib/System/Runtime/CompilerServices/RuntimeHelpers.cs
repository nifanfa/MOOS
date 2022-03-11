// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence

namespace System.Runtime.CompilerServices
{
    public class RuntimeHelpers
    {
        public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);
    }
}
