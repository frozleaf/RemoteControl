namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x41d32e77872302c5 : xbd3f2493841f18a1
    {
        private x60982e1b4eef090a x1f8a3bb85c91d0b5;

        public x41d32e77872302c5(Control control, SkinEngine engine) : base(control, engine)
        {
            ScrollBar host = (ScrollBar) control;
            if (control is HScrollBar)
            {
                this.x1f8a3bb85c91d0b5 = new x60982e1b4eef090a(host, engine, true);
            }
            else
            {
                this.x1f8a3bb85c91d0b5 = new x60982e1b4eef090a(host, engine, false);
            }
            this.xf149df02f1aa3d56();
            host.VisibleChanged += new EventHandler(this.xbad904bdcc347055);
            host.SizeChanged += new EventHandler(this.xe8a173d72f8f3729);
            host.LocationChanged += new EventHandler(this.x9fca9353a2d2087c);
            host.Resize += new EventHandler(this.xb82f209246987e7a);
            host.EnabledChanged += new EventHandler(this.xc68edf742e725d77);
            host.ValueChanged += new EventHandler(this.xdf8f78b46a8ed400);
        }

        protected override void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            base.OnCurrentSkinChanged(sender, e);
            this.xf149df02f1aa3d56();
        }

        private void x9fca9353a2d2087c(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xf149df02f1aa3d56();
        }

        private void xb82f209246987e7a(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xf149df02f1aa3d56();
        }

        private void xbad904bdcc347055(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xf149df02f1aa3d56();
        }

        private void xc68edf742e725d77(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.x1f8a3bb85c91d0b5.Repaint();
        }

        private void xdf8f78b46a8ed400(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.x1f8a3bb85c91d0b5.Repaint();
        }

        private void xe8a173d72f8f3729(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.xf149df02f1aa3d56();
            this.x1f8a3bb85c91d0b5.Repaint();
        }

        private void xf149df02f1aa3d56()
        {
            this.x1f8a3bb85c91d0b5.Visible = (base.CanPaint & base.Ctrl.Visible) & base.Engine.SkinScrollBar;
            if (this.x1f8a3bb85c91d0b5.Visible)
            {
                if (base.Ctrl.Parent is TableLayoutPanel)
                {
                    Rectangle bounds = base.Ctrl.Bounds;
                    bounds.X += base.Ctrl.Parent.Left;
                    bounds.Y += base.Ctrl.Parent.Top;
                    this.x1f8a3bb85c91d0b5.Bounds = bounds;
                }
                else
                {
                    this.x1f8a3bb85c91d0b5.Bounds = base.Ctrl.Bounds;
                }
                this.x1f8a3bb85c91d0b5.BringToFront();
            }
        }
    }
}

