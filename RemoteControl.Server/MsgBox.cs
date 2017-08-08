using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Server
{
    public class MsgBox
    {
        /// <summary>
        /// 显示信息对话框
        /// </summary>
        /// <param name="content">信息内容</param>
        /// <returns></returns>
        public static DialogResult ShowInfo(string content)
        {
            return MessageBox.Show(content, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ShowOkCancel(string content)
        {
            return MessageBox.Show(content, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        public static DialogResult ShowYesNo(string content)
        {
            return MessageBox.Show(content, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
