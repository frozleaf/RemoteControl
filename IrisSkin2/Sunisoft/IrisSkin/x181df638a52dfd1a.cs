namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=2)]
    internal struct x181df638a52dfd1a
    {
        public int Signature;
        public short VersionMadeBy;
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
        public short FileCommentLength;
        public short DiskNumberStart;
        public short InternalFileAttributes;
        public int ExternalFileAttributes;
        public int RelativeOffset;
    }
}

