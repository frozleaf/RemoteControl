using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Server
{
    public class UIUtil
    {
        public static void BindTextBoxCtrlA(TextBox textBox)
        {
            textBox.KeyDown += (o, args) =>
            {
                if (args.Control && args.KeyCode == Keys.A)
                {
                    textBox.SelectAll();
                }
            };
        }
    }
}
