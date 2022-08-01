using System.Collections.Generic;

namespace Cosmos.HAL
{
    public enum ClassID
    {
        PCIDevice_2_0 = 0x00,
        MassStorageController = 0x01,
        NetworkController = 0x02,
        DisplayController = 0x03,
        MultimediaDevice = 0x04,
        MemoryController = 0x05,
        BridgeDevice = 0x06,
        SimpleCommController = 0x07,
        BaseSystemPreiph = 0x08,
        InputDevice = 0x09,
        DockingStations = 0x0A,
        Proccesors = 0x0B,
        SerialBusController = 0x0C,
        WirelessController = 0x0D,
        InteligentController = 0x0E,
        SateliteCommController = 0x0F,
        EncryptionController = 0x10,
        SignalProcessingController = 0x11,
        ProcessingAccelerators = 0x12,
        NonEssentialInstsrumentation = 0x13,
        Coprocessor = 0x40,
        Unclassified = 0xFF
    }

    public enum SubclassID
    {
        SCSIStorageController = 0x00,
        IDEInterface = 0x01,
        FloppyDiskController = 0x02,
        IPIBusController = 0x03,
        RAIDController = 0x04,
        ATAController = 0x05,
        SATAController = 0x06,
        SASController = 0x07,
        NVMController = 0x08,
        UnknownMassStorage = 0x09,
    }

    public enum ProgramIF
    {
        SATA_VendorSpecific = 0x00,
        SATA_AHCI = 0x01,
        SATA_SerialStorageBus = 0x02,
        SAS_SerialStorageBus = 0x01,
        NVM_NVMHCI = 0x01,
        NVM_NVMExpress = 0x02
    }

    public enum VendorID
    {
        Intel = 0x8086,
        AMD = 0x1022,
        VMWare = 0x15AD,
        Bochs = 0x1234,
        VirtualBox = 0x80EE
    }

    public enum DeviceID
    {
        SVGAIIAdapter = 0x0405,
        PCNETII = 0x2000,
        BGA = 0x1111,
        VBVGA = 0xBEEF,
        VBoxGuest = 0xCAFE
    }

    internal class PCI
    {
        internal static PCIDevice GetDevice(VendorID vendor, DeviceID device)
        {
            var moos_dev = MOOS.PCI.GetDevice((ushort)vendor, (ushort)device);
            if (moos_dev == null) return null;

            PCIDevice dev = new PCIDevice();
            dev.Bus = moos_dev.Bus;
            dev.Slot = moos_dev.Slot;
            dev.Func = moos_dev.Function;
            dev.BaseAddressBar = new List<PCIBaseAddressBar>();
            dev.BaseAddressBar.Add(new(moos_dev.Bar0));
            dev.BaseAddressBar.Add(new(moos_dev.Bar1));
            dev.BaseAddressBar.Add(new(moos_dev.Bar2));
            dev.BaseAddressBar.Add(new(moos_dev.Bar3));
            dev.BaseAddressBar.Add(new(moos_dev.Bar4));
            dev.BaseAddressBar.Add(new(moos_dev.Bar5));
            return dev;
        }
    }
}
