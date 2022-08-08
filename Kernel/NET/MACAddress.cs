using System.Runtime.InteropServices;

namespace MOOS.NET
{
    [StructLayout(LayoutKind.Sequential,Pack = 1)]
    public struct MACAddress
    {
        private byte P1;
        private byte P2;
        private byte P3;
        private byte P4;
        private byte P5;
        private byte P6;

        public MACAddress(byte p1,byte p2,byte p3,byte p4,byte p5,byte p6)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
            this.P4 = p4;
            this.P5 = p5;
            this.P6 = p6;
        }

        public bool Equals(MACAddress b)
        {
            return
                this.P1 == b.P1 &&
                this.P2 == b.P2 &&
                this.P3 == b.P3 &&
                this.P4 == b.P4 &&
                this.P5 == b.P5 &&
                this.P6 == b.P6;
        }

        public static bool operator ==(MACAddress a, MACAddress b)
        {
            return
                a.P1 == b.P1 &&
                a.P2 == b.P2 &&
                a.P3 == b.P3 &&
                a.P4 == b.P4 &&
                a.P5 == b.P5 &&
                a.P6 == b.P6;
        }

        public static bool operator !=(MACAddress a, MACAddress b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"{P1.ToString("x2")}:{P2.ToString("x2")}:{P3.ToString("x2")}:{P4.ToString("x2")}:{P5.ToString("x2")}:{P6.ToString("x2")}";
        }
    }
}
