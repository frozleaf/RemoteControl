namespace Sunisoft.IrisSkin
{
    using System;
    using System.Windows.Forms;

    internal class xc0bf85194d12f6b3 : x2edc3f693fe78d2e
    {
        private BorderStyle xacfbd7a08ba56c78;

        public xc0bf85194d12f6b3(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void DoInit()
        {
            base.DoInit();
            base.Engine.CurrentSkinChanged += new SkinChanged(this.x9d36565e6d24455f);
            TreeView ctrl = (TreeView) base.Ctrl;
            this.xacfbd7a08ba56c78 = ctrl.BorderStyle;
            if ((this.xacfbd7a08ba56c78 != BorderStyle.None) && base.CanPaint)
            {
                ctrl.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        protected override void PaintControl()
        {
            base.PaintControl();
        }

        private void x9d36565e6d24455f(object xe0292b9ed559da7d, SkinChangedEventArgs xfbf34718e704c6bc)
        {
            if (!base.Ctrl.IsDisposed)
            {
                TreeView ctrl = (TreeView) base.Ctrl;
                if (this.xacfbd7a08ba56c78 != BorderStyle.None)
                {
                    if (base.CanPaint)
                    {
                        ctrl.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else
                    {
                        ctrl.BorderStyle = this.xacfbd7a08ba56c78;
                    }
                }
            }
        }

        protected override int BorderWidth
        {
            get
            {
                return 0;
            }
        }

        protected override bool ChangeBackColor
        {
            get
            {
                return false;
            }
        }

        protected override bool PaintBorder
        {
            get
            {
                return base.PaintBorder;
            }
        }

        protected override bool PaintBorderLine
        {
            get
            {
                TreeView ctrl = (TreeView) base.Ctrl;
                if (ctrl.BorderStyle == BorderStyle.None)
                {
                    return false;
                }
                return true;
            }
        }
    }
}

