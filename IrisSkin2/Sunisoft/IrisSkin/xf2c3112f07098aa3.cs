namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class xf2c3112f07098aa3 : xbd3f2493841f18a1
    {
        private Color x303d02b7f8775afc;
        private Color x9629e77ac7f4d39c;
        private Color xc664c9651c8df79a;
        private Color xe145387a71f8d313;
        private Color xef45ba12d6a1ece8;

        public xf2c3112f07098aa3(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void DoInit()
        {
            this.x2a81cba7d4877ed7();
            this.x645eb62d485b533f();
            base.DoInit();
        }

        protected override void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            base.OnCurrentSkinChanged(sender, e);
            if (base.CanPaint)
            {
                this.x645eb62d485b533f();
            }
            else
            {
                this.x3d7a7fd39121f268();
            }
        }

        private void x2a81cba7d4877ed7()
        {
            ToolStripContainer ctrl = (ToolStripContainer) base.Ctrl;
            this.xc664c9651c8df79a = ctrl.TopToolStripPanel.BackColor;
            this.x9629e77ac7f4d39c = ctrl.ContentPanel.BackColor;
            this.xef45ba12d6a1ece8 = ctrl.BottomToolStripPanel.BackColor;
            this.xe145387a71f8d313 = ctrl.LeftToolStripPanel.BackColor;
            this.x303d02b7f8775afc = ctrl.RightToolStripPanel.BackColor;
        }

        private void x3d7a7fd39121f268()
        {
            ToolStripContainer ctrl = (ToolStripContainer) base.Ctrl;
            ctrl.TopToolStripPanel.BackColor = this.xc664c9651c8df79a;
            ctrl.ContentPanel.BackColor = this.x9629e77ac7f4d39c;
            ctrl.BottomToolStripPanel.BackColor = this.xef45ba12d6a1ece8;
            ctrl.LeftToolStripPanel.BackColor = this.xe145387a71f8d313;
            ctrl.RightToolStripPanel.BackColor = this.x303d02b7f8775afc;
        }

        private void x645eb62d485b533f()
        {
            ToolStripContainer ctrl = (ToolStripContainer) base.Ctrl;
            Color color = base.Engine.Res.Colors.SKIN2_FORMCOLOR;
            ctrl.TopToolStripPanel.BackColor = color;
            ctrl.RightToolStripPanel.BackColor = color;
            ctrl.LeftToolStripPanel.BackColor = color;
            ctrl.BottomToolStripPanel.BackColor = color;
        }
    }
}

