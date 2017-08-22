using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Server
{
    /// <summary>
    /// 粘贴信息
    /// </summary>
    class PasteInfo
    {
        /// <summary>
        /// 是否删除源文件（复制模式不会删除，剪切模式会删除）
        /// </summary>
        public bool IsDeleteSourceFile;
        /// <summary>
        /// 源文件路径
        /// </summary>
        public string SourceFilePath;
    }
}
