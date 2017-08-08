namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=2)]
    internal struct xf85cd48011a0ce1f
    {
        public int Signature;
        public short DiskNumber;
        public short StartDiskNumber;
        public short EntriesOnDisk;
        public short TotalEntries;
        public int DirectorySize;
        public int DirectoryOffset;
        public short ZipfileCommentLength;
    }
}

