namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class x095234c5c1abb370 : xbd3f2493841f18a1
    {
        private ToolStripRenderer x38870620fd380a6b;
        private xebe00acade2e10a1 xc6e9edf302baab93;
        private static Brush xd4d28a8f36023ad0 = new SolidBrush(Color.Gray);

        public x095234c5c1abb370(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void DoInit()
        {
            ToolStrip ctrl = (ToolStrip) base.Ctrl;
            if (ctrl.Renderer is xebe00acade2e10a1)
            {
                this.x38870620fd380a6b = new ToolStripProfessionalRenderer();
            }
            else
            {
                this.x38870620fd380a6b = ctrl.Renderer;
            }
            this.xc6e9edf302baab93 = new xebe00acade2e10a1(base.Engine);
            this.x2580793586131414();
            base.DoInit();
        }

        protected override void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            this.x2580793586131414();
        }

        private void x2580793586131414()
        {
            ToolStrip ctrl = (ToolStrip) base.Ctrl;
            if (base.CanPaint)
            {
                ctrl.Renderer = this.xc6e9edf302baab93;
            }
            else
            {
                ctrl.Renderer = this.x38870620fd380a6b;
            }
        }
    }
}

