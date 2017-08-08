namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class xead0c12c344036e3 : xbd3f2493841f18a1
    {
        public xead0c12c344036e3(Control control, SkinEngine engine) : base(control, engine)
        {
        }

        protected override void DoInit()
        {
            if (base.Ctrl.CompanyName != "WeifenLuo.WinFormsUI")
            {
                base.DoInit();
                base.Ctrl.GotFocus += new EventHandler(this.x903997f2d792df23);
                base.Ctrl.LostFocus += new EventHandler(this.xe44877363ebb618d);
                base.Ctrl.Paint += new PaintEventHandler(this.xbd9410cb429a8e67);
            }
        }

        protected override void PaintControl()
        {
            if (((base.Ctrl.ClientRectangle.Width > 0) && (base.Ctrl.ClientRectangle.Height > 0)) && base.CanPaint)
            {
                Graphics graphics = Graphics.FromHwnd(base.Ctrl.Handle);
                this.x8bc95f030953f87b(graphics);
                graphics.Dispose();
            }
        }

        private void x6b515e4773b2f185(Graphics x4b101060f4767186, Bitmap x5ce6bc2fb6fe458d)
        {
            x4b101060f4767186.DrawImage(x5ce6bc2fb6fe458d, base.Ctrl.ClientRectangle, base.Ctrl.ClientRectangle, GraphicsUnit.Pixel);
        }

        private void x6b515e4773b2f185(Graphics x4b101060f4767186, Bitmap x5ce6bc2fb6fe458d, string x1c2743f354837549, Font x26094932cf7a9139, Rectangle x59dcf7372713ec60)
        {
            Brush gray = null;
            x4b101060f4767186.DrawImage(x5ce6bc2fb6fe458d, base.Ctrl.ClientRectangle, base.Ctrl.ClientRectangle, GraphicsUnit.Pixel);
            if ((x1c2743f354837549 != null) && (x1c2743f354837549 != ""))
            {
                if (!base.Ctrl.Enabled)
                {
                    gray = Brushes.Gray;
                }
                else
                {
                    gray = new SolidBrush(base.Ctrl.ForeColor);
                }
                if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
                {
                    x448fd9ab43628c71.OffsetRect(ref x59dcf7372713ec60, 1, 1);
                }
                x4b101060f4767186.DrawString(x1c2743f354837549, x26094932cf7a9139, gray, x59dcf7372713ec60);
            }
            if ((gray != null) && base.Ctrl.Enabled)
            {
                gray.Dispose();
            }
        }

        private void x6f37a3496a324cf4(Graphics x4b101060f4767186)
        {
            Rectangle clientRectangle;
            Rectangle rectangle2;
            StringFormat format;
            Brush gray;
            Button ctrl = (Button) base.Ctrl;
            string text = ctrl.Text;
            Image image = ctrl.Image;
            if (image == null)
            {
                clientRectangle = ctrl.ClientRectangle;
            }
            else
            {
                switch (ctrl.TextImageRelation)
                {
                    case TextImageRelation.ImageBeforeText:
                        clientRectangle = Rectangle.FromLTRB(image.Width + 8, 0, ctrl.ClientRectangle.Right, ctrl.ClientRectangle.Height);
                        goto Label_012E;

                    case TextImageRelation.TextBeforeImage:
                    {
                        float width = ctrl.ClientRectangle.Width;
                        float num2 = (int) x4b101060f4767186.MeasureString(ctrl.Text, ctrl.Font).Width;
                        float num3 = image.Width;
                        if ((num2 + num3) >= width)
                        {
                            clientRectangle = Rectangle.FromLTRB(0, 0, (ctrl.ClientRectangle.Width - 8) - image.Width, ctrl.ClientRectangle.Height);
                        }
                        else
                        {
                            clientRectangle = Rectangle.FromLTRB(0, 0, (int) ((width * num2) / (num2 + num3)), ctrl.ClientRectangle.Height);
                        }
                        goto Label_012E;
                    }
                }
                clientRectangle = ctrl.ClientRectangle;
            }
        Label_012E:
            x448fd9ab43628c71.CalcLayoutRect(ctrl.TextAlign, clientRectangle, ctrl.RightToLeft, out rectangle2, out format);
            if (rectangle2.Width > 8)
            {
                rectangle2.X += 2;
                rectangle2.Width -= 4;
            }
            if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
            {
                x448fd9ab43628c71.OffsetRect(ref rectangle2, 1, 1);
            }
            if (base.Ctrl.Enabled)
            {
                gray = base.Engine.Res.Brushes.SKIN2_BUTTONFONTCOLOR;
            }
            else
            {
                gray = Brushes.Gray;
            }
            x4b101060f4767186.DrawString(ctrl.Text, ctrl.Font, gray, rectangle2, format);
        }

        private void x8bc95f030953f87b(Graphics x4b101060f4767186)
        {
            if ((base.ctrlMouseState == xb9506a535e31f22a.MouseIn) && !x448fd9ab43628c71.InRect(base.Ctrl.PointToClient(Control.MousePosition), base.Ctrl.ClientRectangle))
            {
                base.ctrlMouseState = xb9506a535e31f22a.None;
            }
            int ctrlMouseState = (int) base.ctrlMouseState;
            Bitmap src = base.Engine.Res.SplitBitmaps.SKIN2_BUTTON[ctrlMouseState];
            using (Bitmap bitmap2 = new Bitmap(base.Ctrl.Width, base.Ctrl.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap2))
                {
                    Brush brush = base.Engine.Res.Brushes.SKIN2_FORMCOLOR;
                    graphics.FillRectangle(brush, base.Ctrl.ClientRectangle);
                    x448fd9ab43628c71.SplitDraw(src, graphics, base.Ctrl.ClientRectangle);
                    if (((Button) base.Ctrl).Image != null)
                    {
                        this.xcea9a0eaf0e83f21(graphics);
                    }
                    if ((base.Ctrl.Text != null) && (base.Ctrl.Text != ""))
                    {
                        this.x6f37a3496a324cf4(graphics);
                    }
                    if (base.Ctrl.Focused)
                    {
                        Rectangle rect = Rectangle.FromLTRB(base.Ctrl.ClientRectangle.Left + 4, base.Ctrl.ClientRectangle.Top + 4, base.Ctrl.ClientRectangle.Right - 5, base.Ctrl.ClientRectangle.Bottom - 5);
                        if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
                        {
                            x448fd9ab43628c71.OffsetRect(ref rect, 1, 1);
                        }
                        graphics.DrawRectangle(x448fd9ab43628c71.FocusRectanglePen, rect);
                    }
                    this.x6b515e4773b2f185(x4b101060f4767186, bitmap2);
                }
            }
        }

        private void x903997f2d792df23(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (base.CanPaint)
            {
                this.PaintControl();
            }
        }

        private void xbd9410cb429a8e67(object xe0292b9ed559da7d, PaintEventArgs xfbf34718e704c6bc)
        {
            if (base.CanPaint)
            {
                this.x8bc95f030953f87b(xfbf34718e704c6bc.Graphics);
            }
        }

        private void xcea9a0eaf0e83f21(Graphics x4b101060f4767186)
        {
            Button ctrl = (Button) base.Ctrl;
            Image image = ctrl.Image;
            if ((image.Height > 0) && (image.Width > 0))
            {
                Rectangle clientRectangle;
                switch (ctrl.TextImageRelation)
                {
                    case TextImageRelation.ImageBeforeText:
                        clientRectangle = new Rectangle(0, 0, image.Width + 8, ctrl.ClientRectangle.Height);
                        break;

                    case TextImageRelation.TextBeforeImage:
                    {
                        float width = ctrl.ClientRectangle.Width;
                        float num2 = (int) x4b101060f4767186.MeasureString(ctrl.Text, ctrl.Font).Width;
                        float num3 = image.Width;
                        if ((num2 + num3) >= width)
                        {
                            clientRectangle = Rectangle.FromLTRB((ctrl.ClientRectangle.Width - 8) - image.Width, 0, ctrl.ClientRectangle.Width, ctrl.ClientRectangle.Height);
                        }
                        else
                        {
                            clientRectangle = Rectangle.FromLTRB((int) ((width * num2) / (num2 + num3)), 0, ctrl.ClientRectangle.Width, ctrl.ClientRectangle.Height);
                        }
                        break;
                    }
                    default:
                        clientRectangle = ctrl.ClientRectangle;
                        break;
                }
                Rectangle rect = x448fd9ab43628c71.CalcLayoutRect(ctrl.ImageAlign, ctrl.RightToLeft, clientRectangle, image.Width, image.Height, 6);
                if (base.ctrlMouseState == xb9506a535e31f22a.MouseDown)
                {
                    x448fd9ab43628c71.OffsetRect(ref rect, 1, 1);
                }
                x4b101060f4767186.DrawImage(image, rect.X, rect.Y, image.Width, image.Height);
            }
        }

        private void xe44877363ebb618d(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (base.CanPaint)
            {
                this.PaintControl();
            }
        }
    }
}

