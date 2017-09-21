using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace RemoteControl.Server.Utils
{
    public static class IconChangerExtensions
    {
        /// <summary>
        /// 从文件流中读取结构体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static T Read<T>(this FileStream fs) where T:struct
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            fs.Read(buffer, 0, buffer.Length);

            return buffer.ToStruct<T>();
        }

        public static T ToStruct<T>(this byte[] data)
        {
            // 字节数组转IntPtr
            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);
            // IntPtr转struct
            T result = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.Release(ptr);

            return result;
        }

        public static IntPtr ToPtr(this byte[] data)
        {
            IntPtr p = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, p, data.Length);

            return p;
        }

        public static byte[] ToByteArray<T>(this T obj) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, true);
            byte[] buffer = new byte[size];
            Marshal.Copy(ptr, buffer, 0, buffer.Length);
            Marshal.FreeHGlobal(ptr);

            return buffer;
        }

        public static IntPtr ToPtr<T>(this T obj) where T:struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr p = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, p, true);

            return p;
        }

        public static int GetStructSize<T>(this T obj) where T:struct
        {
            return Marshal.SizeOf(typeof(T));
        }
    }
}
