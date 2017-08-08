namespace Sunisoft.IrisSkin
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    public class Skin3rdControl : NativeWindow, IDisposable
    {
        protected Control Ctrl;
        protected SkinEngine Engine;
        protected Hashtable skinControlList = new Hashtable();
        private bool x230e512478b11e00 = true;

        public Skin3rdControl(Control control, SkinEngine engine)
        {
            this.Ctrl = control;
            this.Engine = engine;
            this.DoInit();
            this.x172fd1e80a81620d();
        }

        protected virtual void AfterWndProc(ref Message m)
        {
        }

        protected virtual bool BeforeWndProc(ref Message m)
        {
            return true;
        }

        public void Dispose()
        {
        }

        protected virtual void DoInit()
        {
            this.Engine.CurrentSkinChanged += new SkinChanged(this.x8c06ad1b432b98eb);
            base.AssignHandle(this.Ctrl.Handle);
        }

        protected virtual void DoUnInit()
        {
        }

        protected virtual void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            if (this.CanPaint)
            {
                this.xb1ee1b6293236c5f();
            }
            this.Ctrl.Refresh();
        }

        protected virtual void PaintControl()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (this.CanPaint)
            {
                if (this.BeforeWndProc(ref m))
                {
                    base.WndProc(ref m);
                }
                this.AfterWndProc(ref m);
            }
            else
            {
                base.WndProc(ref m);
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

        private void x8c06ad1b432b98eb(object xe0292b9ed559da7d, SkinChangedEventArgs xfbf34718e704c6bc)
        {
            this.OnCurrentSkinChanged(xe0292b9ed559da7d, xfbf34718e704c6bc);
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
    }
}

