namespace Cosmos.Core
{
    internal class ACPI
    {
        public static void Shutdown() => MOOS.Driver.Power.Shutdown();
    }
}
