// Copywrite (C) 2021 Contributors of nifanfa/Solution1. Licensed under the  MIT licence

namespace System.Runtime.InteropServices
{
    public sealed class UnmanagedFunctionPointerAttribute : Attribute
    {
        public UnmanagedFunctionPointerAttribute()
        {
            CallingConvention = CallingConvention.Winapi;
        }

        public UnmanagedFunctionPointerAttribute(CallingConvention callingConvention)
        {
            CallingConvention = callingConvention;
        }

        public CallingConvention CallingConvention { get; }

        public bool BestFitMapping;
        public bool SetLastError;
        public bool ThrowOnUnmappableChar;
        public CharSet CharSet;
    }
}
