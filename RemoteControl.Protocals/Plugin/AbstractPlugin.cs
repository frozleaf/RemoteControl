using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals.Plugin
{
    /// <summary>
    /// 插件抽象类
    /// </summary>
    public class AbstractPlugin : IPlugin
    {
        /// <summary>
        /// 退出程序事件
        /// </summary>
        public event EventHandler FireQuitEvent;

        /// <summary>
        /// 执行代码
        /// </summary>
        public virtual void Exec()
        {
        }

        /// <summary>
        /// 执行代码
        /// </summary>
        /// <param name="assemblyBytes"></param>
        public virtual void Exec(byte[] assemblyBytes)
        {
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public void FireQuit()
        {
            if (FireQuitEvent != null)
            {
                FireQuitEvent(this, EventArgs.Empty);
            }
        }
    }
}
