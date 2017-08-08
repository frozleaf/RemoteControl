namespace Sunisoft.IrisSkin
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    internal class xbd3f2493841f18a1 : NativeWindow, IDisposable
    {
        protected Color backColor;
        protected Control Ctrl;
        protected xb9506a535e31f22a ctrlMouseState = xb9506a535e31f22a.None;
        protected SkinEngine Engine;
        protected Color fontColor;
        public xa1883d0b59b7005b SkinContextMenu;
        public x095234c5c1abb370 SkinContextMenuStrip;
        protected Hashtable skinControlList = new Hashtable();
        private bool x230e512478b11e00 = true;

        public xbd3f2493841f18a1(Control control, SkinEngine engine)
        {
            this.Ctrl = control;
            this.Engine = engine;
            this.DoInit();
            this.x172fd1e80a81620d();
        }

        public void Dispose()
        {
        }

        protected virtual void DoInit()
        {
            if (this.Ctrl.BackColor != this.Engine.Res.Colors.SKIN2_FORMCOLOR)
            {
                this.backColor = this.Ctrl.BackColor;
            }
            this.fontColor = this.Ctrl.ForeColor;
            if (this.CanPaint)
            {
                if (this.ChangeBackColor)
                {
                    this.Ctrl.BackColor = this.Engine.Res.Colors.SKIN2_FORMCOLOR;
                }
                if (this.ChangeFontColor)
                {
                    this.Ctrl.ForeColor = this.Engine.Res.Colors.SKIN2_CONTROLFONTCOLOR;
                }
            }
            this.Engine.CurrentSkinChanged += new SkinChanged(this.x8c06ad1b432b98eb);
            this.Ctrl.Paint += new PaintEventHandler(this.x32475e142481d30b);
            this.Ctrl.BackgroundImageChanged += new EventHandler(this.x1f31d31f0e09c076);
            this.Ctrl.EnabledChanged += new EventHandler(this.x02b398f1260ef2d9);
            this.Ctrl.TextChanged += new EventHandler(this.x238e719e4d456cb9);
            this.Ctrl.MouseEnter += new EventHandler(this.x5aa038edd784e0a2);
            this.Ctrl.MouseLeave += new EventHandler(this.x42271d490e21550a);
            this.Ctrl.MouseUp += new MouseEventHandler(this.xa5ecc87221b6cc18);
            this.Ctrl.MouseDown += new MouseEventHandler(this.xd82fdb580ad5d9c2);
            this.Ctrl.KeyDown += new KeyEventHandler(this.xbca0e60879f588a1);
            this.Ctrl.KeyUp += new KeyEventHandler(this.x3d2022a4b24873fe);
            this.Ctrl.GotFocus += new EventHandler(this.x2bf160cd2bad82a5);
            this.Ctrl.BackColorChanged += new EventHandler(this.xcd7553179bfc3f6c);
            if (this.Ctrl.ContextMenu != null)
            {
                this.SkinContextMenu = new xa1883d0b59b7005b(this.Engine, this.Ctrl.ContextMenu);
            }
            if (this.Ctrl.ContextMenuStrip != null)
            {
                this.SkinContextMenuStrip = new x095234c5c1abb370(this.Ctrl.ContextMenuStrip, this.Engine);
            }
        }

        protected virtual void DoUnInit()
        {
        }

        protected virtual void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            if (this.CanPaint)
            {
                this.xb1ee1b6293236c5f();
                if (this.ChangeBackColor)
                {
                    this.Ctrl.BackColor = this.Engine.Res.Colors.SKIN2_FORMCOLOR;
                }
                if (this.ChangeFontColor)
                {
                    this.Ctrl.ForeColor = this.Engine.Res.Colors.SKIN2_CONTROLFONTCOLOR;
                }
            }
            else if (!this.Ctrl.IsDisposed)
            {
                if (this.ChangeBackColor)
                {
                    this.Ctrl.BackColor = this.backColor;
                }
                if (this.ChangeFontColor)
                {
                    this.Ctrl.ForeColor = this.fontColor;
                }
            }
            this.Ctrl.Refresh();
        }

        protected virtual void PaintControl()
        {
        }

        private void x02b398f1260ef2d9(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void x172fd1e80a81620d()
        {
            this.Ctrl.ControlAdded += new ControlEventHandler(this.xd34eabd7bd15b71a);
            this.Ctrl.ControlRemoved += new ControlEventHandler(this.xf8f5e344dcfab3e8);
            if (this.CanPaint)
            {
                ArrayList list = new ArrayList();
                foreach (Control control in this.Ctrl.Controls)
                {
                    if (!this.skinControlList.ContainsKey(control.Handle))
                    {
                        list.Add(control);
                    }
                }
                foreach (Control control2 in list)
                {
                    if (!this.skinControlList.ContainsKey(control2.Handle))
                    {
                        xbd3f2493841f18a1 xbdffa = xf3f6919ac5d158dc.Create(control2, this.Engine);
                        this.skinControlList.Add(control2.Handle, xbdffa);
                    }
                }
            }
        }

        private void x1f31d31f0e09c076(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void x238e719e4d456cb9(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void x2bf160cd2bad82a5(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void x32475e142481d30b(object xe0292b9ed559da7d, PaintEventArgs xfbf34718e704c6bc)
        {
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void x3d2022a4b24873fe(object xe0292b9ed559da7d, KeyEventArgs xfbf34718e704c6bc)
        {
            if ((xfbf34718e704c6bc.KeyCode == Keys.Space) || (xfbf34718e704c6bc.KeyCode == Keys.Enter))
            {
                this.ctrlMouseState = xb9506a535e31f22a.None;
                if (this.CanPaint)
                {
                    this.PaintControl();
                }
            }
        }

        private void x42271d490e21550a(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.ctrlMouseState = xb9506a535e31f22a.None;
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        internal void x52b190e626f65140()
        {
            foreach (object obj2 in this.skinControlList.Values)
            {
                if (obj2 is xbd3f2493841f18a1)
                {
                    ((xbd3f2493841f18a1) obj2).x52b190e626f65140();
                }
            }
            this.x230e512478b11e00 = false;
            this.OnCurrentSkinChanged(null, new SkinChangedEventArgs(false));
        }

        private void x5aa038edd784e0a2(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.ctrlMouseState = xb9506a535e31f22a.MouseIn;
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void x8c06ad1b432b98eb(object xe0292b9ed559da7d, SkinChangedEventArgs xfbf34718e704c6bc)
        {
            this.OnCurrentSkinChanged(xe0292b9ed559da7d, xfbf34718e704c6bc);
        }

        private void xa5ecc87221b6cc18(object xe0292b9ed559da7d, MouseEventArgs xfbf34718e704c6bc)
        {
            this.ctrlMouseState = xb9506a535e31f22a.MouseIn;
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void xb1ee1b6293236c5f()
        {
            if (this.CanPaint)
            {
                ArrayList list = new ArrayList();
                foreach (Control control in this.Ctrl.Controls)
                {
                    if (!this.skinControlList.ContainsKey(control.Handle))
                    {
                        list.Add(control);
                    }
                }
                foreach (Control control2 in list)
                {
                    if (!this.skinControlList.ContainsKey(control2.Handle))
                    {
                        xbd3f2493841f18a1 xbdffa = xf3f6919ac5d158dc.Create(control2, this.Engine);
                        this.skinControlList.Add(control2.Handle, xbdffa);
                    }
                }
            }
        }

        private void xbca0e60879f588a1(object xe0292b9ed559da7d, KeyEventArgs xfbf34718e704c6bc)
        {
            if ((xfbf34718e704c6bc.KeyCode == Keys.Space) || (xfbf34718e704c6bc.KeyCode == Keys.Enter))
            {
                this.ctrlMouseState = xb9506a535e31f22a.MouseDown;
                if (this.CanPaint)
                {
                    this.PaintControl();
                }
            }
        }

        private void xcd7553179bfc3f6c(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void xd34eabd7bd15b71a(object xe0292b9ed559da7d, ControlEventArgs xfbf34718e704c6bc)
        {
            if (this.CanPaint)
            {
                Control control = xfbf34718e704c6bc.Control;
                if (!this.skinControlList.ContainsKey(control.Handle))
                {
                    xbd3f2493841f18a1 xbdffa = xf3f6919ac5d158dc.Create(control, this.Engine);
                    this.skinControlList.Add(control.Handle, xbdffa);
                }
            }
        }

        private void xd82fdb580ad5d9c2(object xe0292b9ed559da7d, MouseEventArgs xfbf34718e704c6bc)
        {
            this.ctrlMouseState = xb9506a535e31f22a.MouseDown;
            if (this.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void xf8f5e344dcfab3e8(object xe0292b9ed559da7d, ControlEventArgs xfbf34718e704c6bc)
        {
        }

        protected bool CanPaint
        {
            get
            {
                if (!this.x230e512478b11e00)
                {
                    return false;
                }
                if (this.Ctrl.IsDisposed)
                {
                    return false;
                }
                if (!this.Engine.RealActive)
                {
                    return false;
                }
                if (this.Ctrl.Tag is int)
                {
                    if (((int) this.Ctrl.Tag) == this.Engine.DisableTag)
                    {
                        return false;
                    }
                    return true;
                }
                if ((this.Ctrl.Tag is string) && (((string) this.Ctrl.Tag) == this.Engine.DisableTag.ToString()))
                {
                    return false;
                }
                return true;
            }
        }

        protected virtual bool ChangeBackColor
        {
            get
            {
                return true;
            }
        }

        protected virtual bool ChangeFontColor
        {
            get
            {
                return false;
            }
        }
    }
}

