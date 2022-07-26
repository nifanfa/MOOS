using MOOS.Misc;

namespace MOOS
{
    public static unsafe class API
    {
        public static unsafe void* HandleSystemCall(string name)
        {
            switch (name)
            {
                case "WriteLine":
                    return (delegate*<string, void>)&Console.WriteLine;
                default:
                    Panic.Error($"System call \"{name}\" is not found");
                    return null;
            }
        }
    }
}