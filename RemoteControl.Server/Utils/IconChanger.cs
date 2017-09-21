using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace RemoteControl.Server.Utils
{
    class IconChanger
    {
        [DllImport("Kernel32.dll")]
        public static extern int GetLastError();

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("Kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hLibModule);

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hReslnfo);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources);

        [DllImport("Kernel32.dll")]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        [DllImport("Kernel32.dll")]
        public static extern int SizeofResource(IntPtr hModule, IntPtr hReslnfo);

        [DllImport("Kernel32.dll")]
        public static extern bool UpdateResource(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, int wLanguage, IntPtr lpData, int cbData);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ICONDIRENTRY
        {
            public byte bWidth; // 宽度
            public byte bHeight; // 高度
            public byte bColorCount; // 位深度
            public byte bReserved;
            public short wPlanes;
            public short wBitCount;
            public int dwBytesInRes; // 资源大小
            public int dwImageOffset; // 资源偏移
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ICONDIR
        {
            public short idReserved;
            public short idType;
            public short idCount;
            //ICONDIRENTRY idEntries;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct GRPICONDIRENTRY
        {
            public byte bWidth; // 宽度
            public byte bHeight; // 高度
            public byte bColorCount; // 位深度
            public byte bReserved;
            public short wPlanes;
            public short wBitCount;
            public int dwBytesInRes; // 资源大小
            public short nID; // ID

            public void CopyFrom(ICONDIRENTRY other)
            {
                this.bWidth = other.bWidth;
                this.bHeight = other.bHeight;
                this.bColorCount = other.bColorCount;
                this.bReserved = other.bReserved;
                this.wPlanes = other.wPlanes;
                this.wBitCount = other.wBitCount;
                this.dwBytesInRes = other.dwBytesInRes;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct GRPICONDIR
        {
            public short idReserved;
            public short idType;
            public short idCount;
            //public GRPICONDIRENTRY idEntries;
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        enum eResourceTypes
        {
            RT_CURSOR = 1,
            RT_BITMAP = 2,
            RT_ICON = 3,
            RT_MENU = 4,
            RT_DIALOG = 5,
            RT_STRING = 6,
            RT_FONTDIR = 7,
            RT_FONT = 8,
            RT_ACCELERATOR = 9,
            RT_RCDATA = 10,
            RT_MESSAGETABLE = 11,
            DIFFERENCE = 11,
            RT_GROUP_CURSOR = DIFFERENCE + RT_CURSOR,
            RT_GROUP_ICON = DIFFERENCE + RT_ICON,
            RT_VERSION = 16,
            RT_DLGINCLUDE = 17,
            RT_PLUGPLAY = 19,
            RT_VXD = 20,
            RT_ANICURSOR = 21,
            RT_ANIICON = 22,
            RT_HTML = 23
        }

        public static void ChangeIcon(string exeFilePath, string icoFilePath)
        {
            int len = Marshal.SizeOf(typeof(GRPICONDIR));
            using (FileStream fs = new FileStream(icoFilePath, FileMode.Open, FileAccess.Read))
            {
                // 读取图标目录
                ICONDIR iconDir = fs.Read<ICONDIR>();
                // 读取图标头列表
                List<ICONDIRENTRY> iconDirEntrys = new List<ICONDIRENTRY>();
                for (int i = 0; i < iconDir.idCount; i++)
                {
                    ICONDIRENTRY iconDirEntry = fs.Read<ICONDIRENTRY>();
                    iconDirEntrys.Add(iconDirEntry);
                }
                // 读取图标数据列表
                List<byte[]> iconDatas = new List<byte[]>();
                for (int i = 0; i < iconDir.idCount; i++)
                {
                    byte[] iconData = new byte[iconDirEntrys[i].dwBytesInRes];
                    fs.Seek(iconDirEntrys[i].dwImageOffset, SeekOrigin.Begin);
                    fs.Read(iconData, 0, iconData.Length);
                    iconDatas.Add(iconData);
                }

                // 生成GRPICONDIR
                GRPICONDIR grpIconDir = new GRPICONDIR();
                grpIconDir.idCount = iconDir.idCount;
                grpIconDir.idReserved = 0;
                grpIconDir.idType = 1; // 1代表图标

                // 生成List<GRPICONDIRENTRY>
                List<GRPICONDIRENTRY> grpIconDirEntrys = new List<GRPICONDIRENTRY>();
                for (int i = 0; i < iconDirEntrys.Count; i++)
                {
                    GRPICONDIRENTRY grpIconDirEntry = new GRPICONDIRENTRY();
                    grpIconDirEntry.CopyFrom(iconDirEntrys[i]);
                    grpIconDirEntry.nID = (short)(i + 1);
                    grpIconDirEntrys.Add(grpIconDirEntry);
                }
                List<byte> grpData = new List<byte>();
                grpData.AddRange(grpIconDir.ToByteArray());
                for (int i = 0; i < grpIconDirEntrys.Count; i++)
                {
                    grpData.AddRange(grpIconDirEntrys[i].ToByteArray());
                }
                IntPtr grpDataPtr = grpData.ToArray().ToPtr();

                bool ret = false;
                IntPtr pUpdateRes = BeginUpdateResource(exeFilePath, false);
                for (int i = 0; i < grpIconDirEntrys.Count; i++)
                {
                    // 更新图标数据
                    int id = grpIconDirEntrys[i].nID;
                    ret = UpdateResource(pUpdateRes, new IntPtr((int)eResourceTypes.RT_ICON), new IntPtr(id), 0, iconDatas[i].ToPtr(), iconDatas[i].Length);
                }
                // 更新图标目录和图标头
                // 32512为当前exe中Icon Group下id号
                ret = UpdateResource(pUpdateRes, new IntPtr((int)eResourceTypes.RT_GROUP_ICON), new IntPtr(32512), 0, grpDataPtr, grpData.Count);
                ret = EndUpdateResource(pUpdateRes, false);
            }
        }

        /// <summary>
        /// 字符串转IntPtr
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private IntPtr MakeStrResource(string str)
        {
            return Marshal.StringToHGlobalAnsi(str);
        }

        private void Test(string filePath, string icoPath)
        {
            IntPtr hModule = LoadLibrary(icoPath);
            IntPtr hRes = FindResource(hModule, MakeStrResource("MAINICON"), new IntPtr((int)eResourceTypes.RT_GROUP_ICON));
            IntPtr hResData = LoadResource(hModule, hRes);
            IntPtr hResLock = LockResource(hResData);
            IntPtr hUpdateRes = BeginUpdateResource(filePath, false);
            bool res = UpdateResource(hUpdateRes,
                new IntPtr((int)eResourceTypes.RT_GROUP_ICON),
                new IntPtr(32512), 0, hRes,
                SizeofResource(hModule, hRes));
            res = EndUpdateResource(hUpdateRes, false);
            res = FreeLibrary(hModule);
        }
    }
}
