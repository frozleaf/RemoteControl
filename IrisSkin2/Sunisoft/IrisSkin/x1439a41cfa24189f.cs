namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;

    internal class x1439a41cfa24189f
    {
        public const uint BLACKNESS = 0x42;
        public const int BS_DIBPATTERN = 5;
        public const int BS_DIBPATTERN8X8 = 8;
        public const int BS_DIBPATTERNPT = 6;
        public const int BS_HATCHED = 2;
        public const int BS_HOLLOW = 1;
        public const int BS_INDEXED = 4;
        public const int BS_MONOPATTERN = 9;
        public const int BS_NULL = 1;
        public const int BS_PATTERN = 3;
        public const int BS_PATTERN8X8 = 7;
        public const int BS_SOLID = 0;
        public const uint CAPTUREBLT = 0x40000000;
        public const uint DSTINVERT = 0x550009;
        public const uint MERGECOPY = 0xc000ca;
        public const uint MERGEPAINT = 0xbb0226;
        public const uint NOMIRRORBITMAP = 0x80000000;
        public const uint NOTSRCCOPY = 0x330008;
        public const uint NOTSRCERASE = 0x1100a6;
        public const uint PATCOPY = 0xf00021;
        public const uint PATINVERT = 0x5a0049;
        public const uint PATPAINT = 0xfb0a09;
        public const int RGN_AND = 1;
        public const int RGN_COPY = 5;
        public const int RGN_DIFF = 4;
        public const int RGN_MAX = 5;
        public const int RGN_MIN = 1;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;
        public const uint SRCAND = 0x8800c6;
        public const uint SRCCOPY = 0xcc0020;
        public const uint SRCERASE = 0x440328;
        public const uint SRCINVERT = 0x660046;
        public const uint SRCPAINT = 0xee0086;
        public const uint WHITENESS = 0xff0062;

        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LOGBRUSH
        {
            public uint lbStyle;
            public uint lbColor;
            public uint lbHatch;
        }
    }
}

