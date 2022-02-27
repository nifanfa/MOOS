// Copywrite (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence
namespace System
{
    public struct Int32
    {
        public override string ToString()
        {
            return ((long)this).ToString();
        }
    }
}
