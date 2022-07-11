namespace MOOS.Driver
{
    public static class PS2Controller
    {
        public static void Initialize()
        {
            PS2Keyboard.Initialize();
            PS2Mouse.Initialise();
        }
    }
}
