namespace Sunisoft.IrisSkin
{
    using System;
    using System.Windows.Forms;

    internal class x3a157e8c7a942ff8 : x2edc3f693fe78d2e
    {
        private BorderStyle xacfbd7a08ba56c78;

        public x3a157e8c7a942ff8(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void DoInit()
        {
            base.DoInit();
            Panel ctrl = (Panel) base.Ctrl;
            this.xacfbd7a08ba56c78 = ctrl.BorderStyle;
            if ((this.xacfbd7a08ba56c78 != BorderStyle.None) && base.CanPaint)
            {
                ctrl.BorderStyle = BorderStyle.FixedSingle;
            }
            base.Engine.CurrentSkinChanged += new SkinChanged(this.x495cb160f8448ee7);
        }

        protected override void PaintControl()
        {
            base.PaintControl();
        }

        private void x495cb160f8448ee7(object xe0292b9ed559da7d, SkinChangedEventArgs xfbf34718e704c6bc)
        {
            if (!base.Ctrl.IsDisposed)
            {
                Panel ctrl = (Panel) base.Ctrl;
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
                if (base.CanPaint)
                {
                    base.Ctrl.BackColor = base.Engine.Res.Colors.SKIN2_FORMCOLOR;
                }
                else
                {
                    base.Ctrl.BackColor = base.backColor;
                }
                base.Ctrl.Refresh();
            }
        }

        protected override int BorderWidth
        {
            get
            {
                Panel ctrl = (Panel) base.Ctrl;
                if (ctrl.BorderStyle == BorderStyle.FixedSingle)
                {
                    return 0;
                }
                return 2;
            }
        }

        protected override bool CanScroll
        {
            get
            {
                return ((Panel) base.Ctrl).AutoScroll;
            }
        }

        protected override bool PaintBorder
        {
            get
            {
                Panel ctrl = (Panel) base.Ctrl;
                if (ctrl.BorderStyle == BorderStyle.None)
                {
                    return false;
                }
                return true;
            }
        }
    }
}

