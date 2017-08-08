namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    internal struct x8632e8b92c54e123
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x40)]
        public byte[] NodeKey;
        public byte NodeType;
        public uint NodeStart;
        public uint NodeSize;
        public uint NextNode;
        public xab0f0a5b63c83169 Type
        {
            get
            {
                switch (this.NodeType)
                {
                    case 1:
                        return xab0f0a5b63c83169.Int;

                    case 2:
                        return xab0f0a5b63c83169.Bool;

                    case 3:
                        return xab0f0a5b63c83169.Str;
                }
                return xab0f0a5b63c83169.Stream;
            }
        }
        public string Key
        {
            get
            {
                return Encoding.Default.GetString(this.NodeKey, 0, this.NodeKey.Length).TrimEnd(new char[1]);
            }
        }
    }
}

