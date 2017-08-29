using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    /// <summary>
    /// 打开文件请求
    /// <para>以默认方式打开文件</para>
    /// </summary>
    public class RequestOpenFile
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath;
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHide;
    }
}
