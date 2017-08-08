namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=2)]
    internal struct x895eb13ee3f5b8cc
    {
        public int Signature;
        public short VersionNeededToExtract;
        public short GeneralPurposeBitFlag;
        public short CompressionMethod;
        public short LastModFileTime;
        public short LastModFileDate;
        public int CRC32;
        public int CompressedSize;
        public int UncompressedSize;
        public short FileNameLength;
        public short ExtraFieldLength;
    }
}

