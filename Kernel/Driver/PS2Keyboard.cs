namespace Kernel
{
    public static class PS2Keyboard
    {
        public static char ProcessKey(byte b)
        {
            switch (b)
            {
                case 0x1E: return 'A';
                case 0x30: return 'B';
                case 0x2E: return 'C';
                case 0x20: return 'D';
                case 0x12: return 'E';
                case 0x21: return 'F';
                case 0x22: return 'G';
                case 0x23: return 'H';
                case 0x17: return 'I';
                case 0x24: return 'J';
                case 0x25: return 'K';
                case 0x26: return 'L';
                case 0x32: return 'M';
                case 0x31: return 'N';
                case 0x18: return 'O';
                case 0x19: return 'P';
                case 0x10: return 'Q';
                case 0x13: return 'R';
                case 0x1F: return 'S';
                case 0x14: return 'T';
                case 0x16: return 'U';
                case 0x2F: return 'V';
                case 0x11: return 'W';
                case 0x2D: return 'X';
                case 0x15: return 'Y';
                case 0x2C: return 'Z';
                case 0x02: return '1';
                case 0x03: return '2';
                case 0x04: return '3';
                case 0x05: return '4';
                case 0x06: return '5';
                case 0x07: return '6';
                case 0x08: return '7';
                case 0x09: return '8';
                case 0x0A: return '9';
                case 0x0B: return '0';
                case 0x1C: return '\n';
                default: return '?';
            }
        }
    }
}
