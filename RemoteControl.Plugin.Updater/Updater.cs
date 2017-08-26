using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals.Plugin;
using System.IO;
using RemoteControl.Protocals.Utilities;
using System.Threading;
using System.Reflection;

namespace RemoteControl.Plugin.Updater
{
    /// <summary>
    /// 更新器
    /// </summary>
    public class Updater:AbstractPlugin
    {
        public override void Exec(byte[] assemblyBytes)
        {
            Console.WriteLine("获取客户端数据...");
            byte[] clientData = GetClientData(assemblyBytes);
            Console.WriteLine("长度：" + clientData.Length);
            Console.WriteLine("生成客户端文件...");
            string filePath = ResUtil.WriteToRandomFile(clientData);
            Console.WriteLine(filePath);
            Console.WriteLine("启动客户端程序...");
            Thread t = ProcessUtil.Run(filePath, "", false);
            t.Join();
            Console.WriteLine("退出当前程序...");
            FireQuit();
        }

        private byte[] GetClientData(byte[] assemblyBytes)
        {
            int length = assemblyBytes.Length;
            int clientStartPos = BitConverter.ToInt32(assemblyBytes, length - 4);
            int clientLength = (int)(length - 4 - clientStartPos);
            byte[] result = new byte[clientLength];
            Array.Copy(assemblyBytes, clientStartPos, result, 0, clientLength);

            return result;
        }
    }
}
