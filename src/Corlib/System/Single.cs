// Copywrite (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace System
{
    public struct Single
    {
        public override unsafe string ToString()
        {
            return ((double)this).ToString();
        }
    }
}
