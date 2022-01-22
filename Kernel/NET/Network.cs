namespace Kernel.NET
{
    public static class Network
    {
        public static byte[] MAC;
        public static byte[] IP;
        public static byte[] Boardcast;
        public static byte[] Gateway;

        public static void Initialise() 
        {
            Boardcast = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        }
    }
}
