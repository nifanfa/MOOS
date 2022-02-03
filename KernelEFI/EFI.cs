using System;

namespace KernelEFI
{
    internal unsafe class Efi
    {
        private static IntPtr pST;

        public static EfiSystemTable* ST 
        {
            get 
            {
                return (EfiSystemTable*)pST;
            }

            set 
            {
                pST = (IntPtr)value;
            }
        }
    }
}
