using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals.Response
{
    /// <summary>
    /// 获取计算机名称
    /// </summary>
    public class ResponseGetHostName:ResponseBase
    {
        public string HostName;
        public string AppPath;
    }
}
