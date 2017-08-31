using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    /// <summary>
    /// 查看注册表key响应
    /// </summary>
    public class ResponseViewRegistryKey : ResponseBase
    {
        public eRegistryHive KeyRoot;
        public string KeyPath;
        public string[] KeyNames;
        public string[] ValueNames;
        public eRegistryValueKind[] ValueKinds;
        public object[] Values;
    }
}
