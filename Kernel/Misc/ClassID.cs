namespace MOOS.Misc
{
    public static class ClassID
    {
        public static string GetName(byte id) 
        {
            if (id == 0x00) return "Unclassified device";
            if (id == 0x01) return "Mass storage controller";
            if (id == 0x02) return "Network controller";
            if (id == 0x03) return "Display controller";
            if (id == 0x04) return "Multimedia controller";
            if (id == 0x05) return "Memory controller";
            if (id == 0x06) return "Bridge";
            if (id == 0x07) return "Communication controller";
            if (id == 0x08) return "Generic system peripheral";
            if (id == 0x09) return "Input device controller";
            if (id == 0x0a) return "Docking station";
            if (id == 0x0b) return "Processor";
            if (id == 0x0c) return "Serial bus controller";
            if (id == 0x0d) return "Wireless controller";
            if (id == 0x0e) return "Intelligent controller";
            if (id == 0x0f) return "Satellite communications controller";
            if (id == 0x10) return "Encryption controller";
            if (id == 0x11) return "Signal processing controller";
            if (id == 0x12) return "Processing accelerators";
            if (id == 0x13) return "Non-Essential Instrumentation";
            if (id == 0x40) return "Coprocessor";
            if (id == 0xff) return "Unassigned class";
            return "Unknown class";
        }
    }
}