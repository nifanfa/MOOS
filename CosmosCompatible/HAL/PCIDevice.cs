using System.Collections.Generic;

namespace Cosmos.HAL
{
    internal class PCIDevice
    {
        public ushort Bus { get; internal set; }
        public ushort Slot { get; internal set; }
        public ushort Func { get; internal set; }

        public List<PCIBaseAddressBar> BaseAddressBar;

        internal void EnableMemory(bool enable)
        {
            MOOS.PCI.WriteRegister16(Bus, Slot, Func, 0x04, 0x04 | 0x02 | 0x01);
        }
    }

    public class PCIBaseAddressBar
    {
        private uint baseAddress = 0;
        private ushort prefetchable = 0;
        private ushort type = 0;
        private bool isIO = false;

        public PCIBaseAddressBar(uint raw)
        {
            isIO = (raw & 0x01) == 1;

            if (isIO)
            {
                baseAddress = raw & 0xFFFFFFFC;
            }
            else
            {
                type = (ushort)((raw >> 1) & 0x03);
                prefetchable = (ushort)((raw >> 3) & 0x01);
                switch (type)
                {
                    case 0x00:
                        baseAddress = raw & 0xFFFFFFF0;
                        break;
                    case 0x01:
                        baseAddress = raw & 0xFFFFFFF0;
                        break;
                }
            }
        }

        public uint BaseAddress
        {
            get { return baseAddress; }
        }

        public bool IsIO
        {
            get { return isIO; }
        }
    }
}
