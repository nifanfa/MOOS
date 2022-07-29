namespace Cosmos.HAL
{
    public class MemoryBlock
    {
        private uint Base;
        public uint Size;

        public MemoryBlock(uint address, uint size)
        {
            this.Base = address;
            this.Size = size;
        }

        public unsafe uint this[uint aByteOffset]
        {
            get
            {
                if (aByteOffset >= Size)
                {
                    return 0;
                }
                return *(uint*)(Base + aByteOffset);
            }
            set
            {
                if (aByteOffset >= Size)
                {
                    return;
                }
                (*(uint*)(Base + aByteOffset)) = value;
            }
        }

        unsafe public void Copy(int aByteOffset, int[] aData, int aIndex, int aCount)
        {
            uint* dest = (uint*)(Base + aByteOffset);

            fixed (int* aDataPtr = aData)
            {
                Native.Movsb(dest, aDataPtr + aIndex, (ulong)aCount);
            }
        }

        public unsafe void Fill(uint offset, uint size, uint color)
        {
            Native.Stosd((void*)(Base + offset), color, size);
        }

        public unsafe void MoveDown(uint aDest, uint aSrc, uint aCount)
        {
            byte* dest = (byte*)(Base + aDest);
            byte* src = (byte*)(Base + aSrc);
            Native.Movsb(dest, src, aCount);
        }
    }
}
