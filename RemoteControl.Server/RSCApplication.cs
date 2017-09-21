using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace RemoteControl.Server
{
    internal class RSCApplication
    {
        public static List<string> lstSkins = new List<string>();
        public static RemoteControlServer oRemoteControlServer = null;

        public static string GetPath(ePathType pathType)
        {
            string sPath = string.Empty;
            switch (pathType)
            {
                case ePathType.APP:
                    sPath = Application.ExecutablePath;
                    break;
                case ePathType.APP_DIR:
                    sPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\";
                    break;
                case ePathType.SKINS_DIR:
                    sPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Skins\\";
                    break;
                case ePathType.AVATAR_DIR:
                    sPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Avatars\\";
                    break;
                case ePathType.TOOL_DIR:
                    sPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Tools\\";
                    break;
            }

            return sPath;
        }

        public static List<string> GetAllSkinFiles()
        {
            List<string> lstSkinFiles = new List<string>();

            string sSkinPath = GetPath(ePathType.SKINS_DIR);
            if (!System.IO.Directory.Exists(sSkinPath))
                return lstSkinFiles;
            string[] arrDirecotry = System.IO.Directory.GetDirectories(sSkinPath);
            for (int i = 0; i < arrDirecotry.Length; i++)
            {
                string sDirectory = arrDirecotry[i];
                // searchPattern,可以使用*或?字符进行模糊搜索，如果不使用则为精确搜索
                string[] arrFile = System.IO.Directory.GetFiles(sDirectory, "*.ssk");
                lstSkinFiles.AddRange(arrFile);
            }

            return lstSkinFiles;
        }

        public static List<string> GetAllAvatarFiles()
        {
            List<string> files = new List<string>();

            string path = GetPath(ePathType.AVATAR_DIR);
            if (!System.IO.Directory.Exists(path))
                return files;

            files.AddRange(System.IO.Directory.GetFiles(path));

            return files;
        }

        public static List<string> GetAllTools()
        {
            List<string> files = new List<string>();

            string path = GetPath(ePathType.TOOL_DIR);
            if (!System.IO.Directory.Exists(path))
                return files;

            files.AddRange(System.IO.Directory.GetFiles(path, "*.exe"));

            return files;
        }

        public static List<string> GetLocalIPV4s()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            return ips.ToList().FindAll(m => m.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(s => s.ToString()).ToList();
        }
    }
}
