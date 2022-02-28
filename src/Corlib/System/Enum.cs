// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System.Runtime.CompilerServices;
namespace System
{
    public abstract class Enum : ValueType
    {
        [Intrinsic]
        public bool HasFlag(Enum flag)
        {
            return false;
        }
    }
}
