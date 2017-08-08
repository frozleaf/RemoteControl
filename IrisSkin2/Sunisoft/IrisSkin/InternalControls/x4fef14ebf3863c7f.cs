namespace Sunisoft.IrisSkin.InternalControls
{
    using System;
    using System.Windows.Forms;

    internal class x4fef14ebf3863c7f : Form
    {
        private x3c41176af7e54b01 x11d58b056c032b03;
        private bool xbb04efaa616d8bab = true;

        public x4fef14ebf3863c7f(x3c41176af7e54b01 target, MainMenu ori)
        {
            this.x11d58b056c032b03 = target;
            base.Menu = ori;
            IntPtr handle = base.Handle;
            this.xbb04efaa616d8bab = false;
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == 0x47) && !this.xbb04efaa616d8bab)
            {
                this.x11d58b056c032b03.x39e008704a72ea56();
                this.x11d58b056c032b03.Invalidate();
            }
            base.WndProc(ref m);
        }
    }
}

