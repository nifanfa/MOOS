// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
namespace System
{
    public struct UInt16
    {
        public override unsafe string ToString()
        {
            return ((ulong)this).ToString();
        }

        public string ToString(string format)
        {
            return ((ulong)this).ToString(format);
        }
    }
}
