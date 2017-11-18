using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Server.Utils
{
    class MsgBox
    {
        public static DialogResult Error(string text)
        {
            return Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Warning(string text)
        {
            return Show(text, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult Question(string text, MessageBoxButtons buttons)
        {
            return Show(text, "询问", buttons, MessageBoxIcon.Question);
        }

        public static DialogResult Info(string text)
        {
            return Show(text, "提示", MessageBoxButtons.OK);
        }

        public static DialogResult Show(string text, string caption)
        {
            return Show(text, caption, MessageBoxButtons.OK);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Show(text, caption, buttons, MessageBoxIcon.Information);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }
    }
}
