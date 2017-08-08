namespace Sunisoft.IrisSkin.InternalControls
{
    using Sunisoft.IrisSkin;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x849a1ee256bf6d69 : NativeWindow
    {
        protected Control Ctrl;
        protected IntPtr CtrlHandle;
        protected SkinEngine Engine;
        protected int Type;

        public x849a1ee256bf6d69(SkinEngine engine, Control control, int type)
        {
            this.Type = type;
            this.Engine = engine;
            this.Ctrl = control;
            this.CtrlHandle = control.Handle;
            this.x63eb955995a0f242();
        }

        protected override void WndProc(ref Message m)
        {
            IntPtr ctrlHandle = this.CtrlHandle;
            if (m.Msg == 15)
            {
                x61467fe65a98f20c.PostMessage(ctrlHandle, 15, m.LParam, m.WParam);
            }
            base.WndProc(ref m);
        }

        private void x63eb955995a0f242()
        {
            if ((this.Ctrl.Parent != null) && !this.Ctrl.Parent.IsDisposed)
            {
                CreateParams cp = new CreateParams {
                    ClassName = "STATIC",
                    Caption = "",
                    Parent = this.Ctrl.Parent.Handle,
                    X = 0,
                    Y = 0,
                    Width = 0,
                    Height = 0,
                    Style = 0x40000000
                };
                this.CreateHandle(cp);
                x61467fe65a98f20c.ShowWindow(base.Handle, 5);
            }
        }

        public Rectangle Bounds
        {
            get
            {
                xae4dd1cafd2eb77c.RECT lpRect = new xae4dd1cafd2eb77c.RECT();
                x61467fe65a98f20c.GetWindowRect(base.Handle, ref lpRect);
                Point location = this.Ctrl.Parent.PointToClient(new Point(lpRect.left, lpRect.top));
                return new Rectangle(location, new Size(lpRect.right - lpRect.left, lpRect.bottom - lpRect.top));
            }
            set
            {
                x61467fe65a98f20c.SetWindowPos(base.Handle, this.CtrlHandle, value.X, value.Y, value.Width, value.Height, 0x40);
            }
        }

        public Rectangle ClientRectangle
        {
            get
            {
                Rectangle lpRect = new Rectangle(0, 0, 0, 0);
                x61467fe65a98f20c.GetClientRect(base.Handle, ref lpRect);
                return lpRect;
            }
        }

        public bool Visible
        {
            get
            {
                return x61467fe65a98f20c.IsWindowVisible(base.Handle);
            }
            set
            {
                if (value)
                {
                    x61467fe65a98f20c.ShowWindow(base.Handle, 5);
                }
                else
                {
                    x61467fe65a98f20c.ShowWindow(base.Handle, 0);
                }
            }
        }
    }
}

