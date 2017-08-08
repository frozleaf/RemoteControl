namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct x57f24fc7026c57d0
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=12)]
        public byte[] FileHeader;
        public uint HeaderSize;
        public int FormatVer;
        public uint FirstNode;
    }
}

