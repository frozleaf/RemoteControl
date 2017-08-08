using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using RemoteControl.Protocals;

namespace RemoteControl.Server
{
    public partial class FrmSelectIcon : FrmBase
    {
        private string _sExeOrDllFileName = null;

        public FrmSelectIcon()
        {
            InitializeComponent();
        }

        private void FrmSelectIcon_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.openFileDialog1.Title = "请选择exe或dll文件";
            this.openFileDialog1.Filter = "exe文件|*.exe|dll文件|*.dll|所有文件|*.*";
            this.openFileDialog1.FileName = "shell32.dll";
            this.openFileDialog1.FilterIndex = 1;
            this.openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System);
            this.openFileDialog1.Multiselect = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this._sExeOrDllFileName = this.openFileDialog1.FileName;
                this.listView1.Items.Clear();
                this.imageList1.Images.Clear();
                extractIcons(this.openFileDialog1.FileName);
            }
        }

        private void extractIcons(string sFileName)
        {
            int iIconCount = Win32API.ExtractIconEx(sFileName, -1, null, null, 0);
            IntPtr[] pLargeIcons = new IntPtr[iIconCount];
            IntPtr[] pSmallIcons = new IntPtr[iIconCount];
            Win32API.ExtractIconEx(sFileName, 0, pLargeIcons, pSmallIcons, iIconCount);
            for (int i = 0; i < iIconCount; i++)
            {
                this.imageList1.Images.Add(Icon.FromHandle(pLargeIcons[i]));
                this.imageList2.Images.Add(Icon.FromHandle(pSmallIcons[i]));
                this.listView1.Items.Add(new ListViewItem(i + "", i, null));
            }
            
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            if (this.listView1.SelectedItems.Count > 0)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                cms.Items.Add("保存图标", null, (o, args) =>
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string sFolder = dialog.SelectedPath;
                        IntPtr[] pLargeIcons = new IntPtr[1];
                        for (int i = 0; i < this.listView1.SelectedItems.Count; i++)
                        {
                            ListViewItem item = this.listView1.SelectedItems[i];
                            string sText = item.Text;
                            Win32API.ExtractIconEx(this._sExeOrDllFileName, Convert.ToInt32(sText), pLargeIcons, null, 1);
                            Icon icon = Icon.FromHandle(pLargeIcons[0]);
                        }
                    }
                });
                cms.Show(sender as ListView, e.Location);
            }
        }
    }
}
