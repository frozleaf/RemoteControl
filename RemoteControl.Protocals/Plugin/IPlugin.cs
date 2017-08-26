using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals.Plugin
{
    /// <summary>
    /// 插件接口
    /// </summary>
    public interface IPlugin
    {
        void Exec();
        void Exec(byte[] assemblyBytes);
        event EventHandler FireQuitEvent;
    }
}
