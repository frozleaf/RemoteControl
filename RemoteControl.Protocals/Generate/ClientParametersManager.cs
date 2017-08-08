using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace RemoteControl.Protocals
{
    public class ClientParametersManager
    {
        public static ClientParameters ReadParameters(string filePath)
        {
            ClientParameters p = new ClientParameters();
            
            int paraSize = Marshal.SizeOf(typeof(ClientParameters));
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            byte[] paraData = fileData.ToList().SplitBytes(fileData.Length - paraSize, paraSize);
            for (int i = 0; i < 4; i++)
            {
                if (paraData[i] != 0xff)
                {
                    return p;
                }
            }
            p = ByteArray2Struct<ClientParameters>(paraData);

            return p;
        }

        public static ClientStyle ReadClientStyle(string filePath)
        {
            byte[] data = System.IO.File.ReadAllBytes(filePath);

            return (ClientStyle)data[0xdc];
        }

        public static void WriteClientStyle(byte[] sourceFileData, ClientStyle style)
        {
            sourceFileData[0xdc] = (byte)style;
        }

        public static void WriteClientStyle(string filePath, ClientStyle style)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write);
            fs.Seek(0xdc, SeekOrigin.Begin);
            fs.WriteByte((byte)style);
            fs.Close();
        }

        public static void WriteParameters(byte[] sourceFileData, string destFileName, ClientParameters para)
        {
            int paraSize = Marshal.SizeOf(typeof(ClientParameters));
            byte[] paraData = sourceFileData.ToList().SplitBytes(sourceFileData.Length - paraSize, paraSize);
            bool paraExists = true;
            for (int i = 0; i < 4; i++)
            {
                if (paraData[i] != 0xff)
                {
                    paraExists = false;
                    break;
                }
            }
            para.InitHeader();
            byte[] paraDataNew = Struct2ByteArray(para);
            System.IO.FileStream fs = new System.IO.FileStream(destFileName, FileMode.Create, FileAccess.Write);
            if (paraExists)
            {
                fs.Write(sourceFileData, 0, sourceFileData.Length - paraSize);
            }
            else
            {
                fs.Write(sourceFileData, 0, sourceFileData.Length);
            }
            fs.Write(paraDataNew, 0, paraDataNew.Length);
            fs.Close();
        }

        public static void WriteParameters(string filePath, ClientParameters para)
        {
            byte[] sourceFileData = System.IO.File.ReadAllBytes(filePath);
            WriteParameters(sourceFileData, filePath, para);
        }

        private static byte[] Struct2ByteArray(object obj)
        {
            int size = Marshal.SizeOf(obj.GetType());
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, true);
            byte[] bytes = new byte[size];
            Marshal.Copy(ptr, bytes, 0, bytes.Length);

            return bytes;
        }

        private static T ByteArray2Struct<T>(byte[] data) where T:struct
        {
            T obj = new T();

            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }

        public enum ClientStyle
        {
            /// <summary>
            /// 控制台应用程序（显示）
            /// </summary>
            Normal=0x03,
            /// <summary>
            /// windows应用程序（隐藏）
            /// </summary>
            Hidden=0x02
        }
    }
}
