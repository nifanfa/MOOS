namespace System
{
    public static class Convert
    {
        internal static int ToUInt16(bool boolean)
        {
            return boolean ? 1 : 0;
        }

        internal static int ToInt16(bool boolean)
        {
            return boolean ? 1 : 0;
        }

        internal static bool ToBoolean(int integer)
        {
            return integer != 0;
        }
    }
}
