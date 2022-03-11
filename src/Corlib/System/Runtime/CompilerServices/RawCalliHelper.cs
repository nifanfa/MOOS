// Copyright (C) 2021 Contributors of nifanfa/Solution1. Licensed under the MIT licence
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    // These functions will be filled in by IL transforms
    [McgIntrinsics]
    internal unsafe class RawCalliHelper
    {
        //public static void StdCall(IntPtr fn) { }
        public static ulong StdCall<T>(IntPtr fn, T a1) where T : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T>(IntPtr fn, T* a1) where T : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U>(IntPtr fn, T a1, U a2) where T : unmanaged where U : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U>(IntPtr fn, T a1, U* a2) where T : unmanaged where U : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U>(IntPtr fn, T* a1, U a2) where T : unmanaged where U : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U>(IntPtr fn, T* a1, U* a2) where T : unmanaged where U : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W>(IntPtr fn, T a1, U a2, W a3) where T : unmanaged where U : unmanaged where W : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W>(IntPtr fn, T a1, U a2, W* a3) where T : unmanaged where U : unmanaged where W : unmanaged
        {
            return 0;
        }

        //public static ulong StdCall<T, U, W>(IntPtr fn, T a1, U* a2, W a3) where T : unmanaged where U : unmanaged where W : unmanaged => 0;
        public static ulong StdCall<T, U, W>(IntPtr fn, T a1, U* a2, W* a3) where T : unmanaged where U : unmanaged where W : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W>(IntPtr fn, T* a1, U a2, W a3) where T : unmanaged where U : unmanaged where W : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W>(IntPtr fn, T* a1, U a2, W* a3) where T : unmanaged where U : unmanaged where W : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W>(IntPtr fn, T* a1, U* a2, W a3) where T : unmanaged where U : unmanaged where W : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W>(IntPtr fn, T* a1, U* a2, W* a3) where T : unmanaged where U : unmanaged where W : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X>(IntPtr fn, T a1, U a2, W a3, X a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X>(IntPtr fn, T a1, U a2, W a3, X* a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged
        {
            return 0;
        }

        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T a1, U a2, W* a3, X a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T a1, U a2, W* a3, X* a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        public static ulong StdCall<T, U, W, X>(IntPtr fn, T a1, U* a2, W a3, X a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged
        {
            return 0;
        }

        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T a1, U* a2, W a3, X* a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T a1, U* a2, W* a3, X a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T a1, U* a2, W* a3, X* a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T* a1, U a2, W a3, X a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T* a1, U a2, W a3, X* a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T* a1, U a2, W* a3, X a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        public static ulong StdCall<T, U, W, X>(IntPtr fn, T* a1, U a2, W* a3, X* a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged
        {
            return 0;
        }

        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T* a1, U* a2, W a3, X a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T* a1, U* a2, W a3, X* a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        //public static ulong StdCall<T, U, W, X>(IntPtr fn, T* a1, U* a2, W* a3, X a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged => 0;
        public static ulong StdCall<T, U, W, X>(IntPtr fn, T* a1, U* a2, W* a3, X* a4) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X, Y>(IntPtr fn, T* a1, U* a2, W* a3, X a4, Y a5) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged where Y : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X, Y>(IntPtr fn, T* a1, U* a2, W* a3, X* a4, Y* a5) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged where Y : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X, Y>(IntPtr fn, T a1, U* a2, W a3, X* a4, Y* a5) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged where Y : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X, Y>(IntPtr fn, T* a1, U a2, W a3, X* a4, Y* a5) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged where Y : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X, Y>(IntPtr fn, T* a1, U a2, W a3, X a4, Y* a5) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged where Y : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X, Y>(IntPtr fn, T* a1, U a2, W* a3, X* a4, Y* a5) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged where Y : unmanaged
        {
            return 0;
        }

        public static ulong StdCall<T, U, W, X, Y, Z>(IntPtr fn, T a1, U* a2, W* a3, X a4, Y a5, Z a6) where T : unmanaged where U : unmanaged where W : unmanaged where X : unmanaged where Y : unmanaged where Z : unmanaged
        {
            return 0;
        }
    }
}
