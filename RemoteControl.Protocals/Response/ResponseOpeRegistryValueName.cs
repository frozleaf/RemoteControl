using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    /// <summary>
    /// 操作注册表value名称响应
    /// </summary>
    public class ResponseOpeRegistryValueName : ResponseBase
    {
        public OpeType Operation;
        public eRegistryHive KeyRoot;
        public string KeyPath;
        public string ValueName;
    }
}
