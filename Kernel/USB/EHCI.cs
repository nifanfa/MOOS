namespace Kernel.USB
{
    public static class EHCI
    {
        public static void Initialize() 
        {
            PCIDevice dev = null;
            for(int i = 0; i < PCI.Devices.Count; i++) 
            {
                if(PCI.Devices[i].ClassID == 0x0C && PCI.Devices[i].SubClassID == 0x03 && PCI.Devices[i].ProgIF == 0x20) 
                {
                    dev = PCI.Devices[i];
                }
            }
            if (dev == null) return;

            Console.WriteLine("EHCI Controller Found");
        }
    }
}
