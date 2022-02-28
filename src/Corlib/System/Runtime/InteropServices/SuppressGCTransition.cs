// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence

namespace System.Runtime.InteropServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class SuppressGCTransitionAttribute : Attribute
    {
        public SuppressGCTransitionAttribute()
        {
        }
    }
}
