using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    /// <summary>
    /// 操作注册表value名称呢过请求
    /// </summary>
    public class RequestOpeRegistryValueName
    {
        public OpeType Operation;
        public eRegistryHive KeyRoot;
        public string KeyPath;
        public string ValueName;
        public object Value;
        public eRegistryValueKind ValueKind = eRegistryValueKind.REG_SZ;
    }
}
