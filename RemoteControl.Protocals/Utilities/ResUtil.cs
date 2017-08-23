using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RemoteControl.Protocals.Utilities
{
    /// <summary>
    /// 资源工具类
    /// </summary>
    public class ResUtil
    {
        public static byte[] GetResFileData(string resFileName)
        {
            string[] resNames = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceNames();
            string blackScreenResName = resNames.ToList().Find(m => m.Contains(resFileName));
            Stream stream = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(blackScreenResName);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();

            return data;
        }

        public static string WriteToRandomFile(byte[] data, string fileName)
        {
            var filePath = Environment.GetEnvironmentVariable("temp") + "\\" + fileName;
            System.IO.File.WriteAllBytes(filePath, data);

            return filePath;
        }

        public static string WriteToRandomFile(byte[] data)
        {
            string randomFileName = System.IO.Path.GetRandomFileName();
            return WriteToRandomFile(data, randomFileName);
        }
    }
}
