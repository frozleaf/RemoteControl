namespace Sunisoft.IrisSkin.Design
{
    using System;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class x1cc8dd3ebd3495cd : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "Sunisoft skin file (ssk file) | *.ssk";
        }
    }
}

