// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
namespace System
{
    public struct RuntimeTypeHandle
    {
        public IntPtr Value;

        public RuntimeTypeHandle(EETypePtr ptr)
        {
            Value = ptr.RawValue;
        }
    }
}
