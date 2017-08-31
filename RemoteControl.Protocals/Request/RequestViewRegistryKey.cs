using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    /// <summary>
    /// 查看注册表key请求
    /// </summary>
    public class RequestViewRegistryKey
    {
        public eRegistryHive KeyRoot;
        public string KeyPath;
    }
}
