using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Client.MsgBox
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 4)
            {
                try
                {
                    MessageBox.Show(args[0], args[1],
                        (MessageBoxButtons)Convert.ToInt32(args[2]), 
                        (MessageBoxIcon)Convert.ToInt32(args[3]));

                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
