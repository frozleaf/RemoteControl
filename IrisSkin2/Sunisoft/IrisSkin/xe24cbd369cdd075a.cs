namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct xe24cbd369cdd075a
    {
        public byte L1;
        public byte L2;
        public byte L3;
        public byte L4;
        public xe24cbd369cdd075a(byte L1, byte L2, byte L3, byte L4)
        {
            this.L1 = L1;
            this.L2 = L2;
            this.L3 = L3;
            this.L4 = L4;
        }

        public static xe24cbd369cdd075a FromInt32(int val)
        {
            return new xe24cbd369cdd075a((byte) (val & 0xff), (byte) (shr(val, 8) & 0xff), (byte) (shr(val, 0x10) & 0xff), (byte) (shr(val, 0x18) & 0xff));
        }

        public static int shr(int i, int j)
        {
            if (i > 0)
            {
                return (i >> j);
            }
            return (i >> j);
        }
    }
}

