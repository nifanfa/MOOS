using System.Runtime.InteropServices;

public static unsafe class EFIDevice
{
    public const byte EfiDpTypeMask = 0x7F;
    public const byte EfiDpTypeUnpacked = 0x80;
    public const byte EndDevicePathType = 0x7f;
    public const byte EndEntireDevicePathSubtype = 0xff;
    public const byte EndInstanceDevicePathSubtype = 0x01;

    public static bool DP_IS_END_SUBTYPE(EfiDevicePathProtocol* a)
    {
        return a->SubType == EndEntireDevicePathSubtype;
    }

    public static byte DevicePathType(EfiDevicePathProtocol* a)
    {
        return (byte) (a->Type & EfiDpTypeMask);
    }

    public static byte DevicePathSubType(EfiDevicePathProtocol* a)
    {
        return a->SubType;
    }

    public static ulong DevicePathNodeLength(EfiDevicePathProtocol* a)
    {
        return (ulong) (a->Length[0] | (a->Length[1] << 8));
    }

    public static EfiDevicePathProtocol* NextDevicePathNode(EfiDevicePathProtocol* a)
    {
        return (EfiDevicePathProtocol*) ((byte*) a + DevicePathNodeLength(a));
    }

    public static bool IsDevicePathEndType(EfiDevicePathProtocol* a)
    {
        return DevicePathType(a) == EndDevicePathType;
    }

    public static bool IsDevicePathEndSubType(EfiDevicePathProtocol* a)
    {
        return a->SubType == EndEntireDevicePathSubtype;
    }

    public static bool IsDevicePathEnd(EfiDevicePathProtocol* a)
    {
        return IsDevicePathEndType(a) && IsDevicePathEndSubType(a);
    }

    public static bool IsDevicePathUnpacked(EfiDevicePathProtocol* a)
    {
        return (a->Type & EfiDpTypeUnpacked) != 0;
    }
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct EfiDevicePathProtocol
{
    public byte Type;
    public byte SubType;
    public fixed byte Length[2];
}