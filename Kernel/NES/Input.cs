namespace NES
{
    public class Input
    {
        public byte joypadOne = 0x00;
        byte j, joypad;

        public byte ReadJoypad()
        {
            byte tempByte;

            if (j >= 8)
            {
                // There is a reference to this in the Original NES Reference Document
                tempByte = 0x40;
            }
            else
            {
                tempByte = (byte)(((joypad >> j++) & 0x01) + 0x40);
            }
            return tempByte;
        }

        public void WriteJoypad(byte byteOne)
        {
            if ((byteOne & 0x01) == 0x00)
            {
                j = 0;
            }
            else if ((byteOne & 0x01) == 0x01)
            {
                j = 0;
                joypad = joypadOne;
            }
        }
    }
}