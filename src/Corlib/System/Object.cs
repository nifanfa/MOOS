/*
* Copyright (c) 2022 nifanfa, This code is part of the OS-Sharp licensed under the MIT licence.
*/

using System.Runtime.InteropServices;
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System
{
    public unsafe class Object
    {
        // The layout of object is a contract with the compiler.
        internal unsafe EEType* m_pEEType;

        [StructLayout(LayoutKind.Sequential)]
        private class RawData
        {
            public byte Data;
        }

        internal ref byte GetRawData()
        {
            return ref Unsafe.As<RawData>(this).Data;
        }

        internal uint GetRawDataSize()
        {
            return m_pEEType->BaseSize - (uint)sizeof(ObjHeader) - (uint)sizeof(EEType*);
        }

        public uint GetBitValue(int bitIndex)
        {
            return (uint)(GetRawData() & (1 << bitIndex));
        }

        public uint GetByteValue(int start, int end)
        {
            uint v = 0;
            for (int i = start; i < end; i++)
            {
                v += GetBitValue(i);
            }
            return v;
        }

        public Object() { }
        ~Object() { }


        public virtual bool Equals(object o)
        {
            return false;
        }

        public virtual int GetHashCode()
        {
            return 0;
        }

        public virtual string ToString()
        {
            return "System.Object";
        }

        public virtual void Dispose()
        {
            object obj = this;
            Allocator.Free(Unsafe.As<object, IntPtr>(ref obj));
        }

        public static implicit operator bool(object obj)
        {
            return ((ulong)Unsafe.AsPointer(ref obj)) >= (ulong)Allocator._Info.Start;
        }
    }
}
