namespace Sunisoft.IrisSkin
{
    using System;
    using System.Windows.Forms;

    internal class x8917d01b98173f4c : xbd3f2493841f18a1
    {
        public x8917d01b98173f4c(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected virtual void AfterWndProc(ref Message m)
        {
        }

        protected virtual bool BeforeWndProc(ref Message m)
        {
            return true;
        }

        protected override void DoInit()
        {
            base.DoInit();
            base.AssignHandle(base.Ctrl.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            if (base.CanPaint)
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
    }
}

