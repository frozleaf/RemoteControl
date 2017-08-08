namespace Sunisoft.IrisSkin
{
    using Sunisoft.IrisSkin.InternalControls;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    internal class xa427f1b2281f554b : xd24df615efe9450e
    {
        private x3c41176af7e54b01 x5486200f56f78413;
        private bool x71d812f69bdec219;
        private Color x7c26c2d810682d2e;
        private MainMenu x946d4396b0c9cde4;
        private xa1883d0b59b7005b xad9f8190f430c699;
        private x095234c5c1abb370 xf6c8424d43a8aa53;

        internal xa427f1b2281f554b(IntPtr handle, SkinEngine engine) : base(handle, engine)
        {
            base.Target = (Form) Control.FromHandle(handle);
            this.x7c26c2d810682d2e = base.Target.BackColor;
            this.x5486200f56f78413 = new x3c41176af7e54b01(base.Engine);
            this.x30c11b5a89921b08();
            if (base.Target.ContextMenu != null)
            {
                this.xad9f8190f430c699 = new xa1883d0b59b7005b(base.Engine, base.Target.ContextMenu);
            }
            this.x33251e4c534e7926();
            base.ChildTable = new Hashtable();
            if (base.Target.IsMdiContainer)
            {
                this.x08d09b54d1a926bd();
            }
            this.x05931f73c2d66689();
            if (base.Target.ContextMenuStrip != null)
            {
                this.xf6c8424d43a8aa53 = new x095234c5c1abb370(base.Target.ContextMenuStrip, base.Engine);
            }
            if (base.Target.MainMenuStrip != null)
            {
                base.Target.MainMenuStrip.LocationChanged += new EventHandler(this.x1d183dacf6b4e43c);
            }
            if ((SkinEngine.Random % 0xc350) == 0)
            {
                try
                {
                    if (x448fd9ab43628c71.x1c12011307e0a753.Contains(base.Engine.SerialNumber.Trim()))
                    {
                        x448fd9ab43628c71.SkinShowMessageBox();
                    }
                    if (!SkinEngine.DSA.VerifyData(SkinEngine.DSAHash, Convert.FromBase64String(base.Engine.SerialNumber)))
                    {
                        x448fd9ab43628c71.SkinShowMessageBox();
                    }
                }
                catch
                {
                    x448fd9ab43628c71.SkinShowMessageBox();
                }
            }
        }

        protected override bool BeforeWndProc(ref Message m)
        {
            if ((m.Msg == 0x20d3) && (m.WParam.ToInt32() == 0x20d3))
            {
                this.xbe3f57786c90d37f();
            }
            return base.BeforeWndProc(ref m);
        }

        protected override void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            if (!this.CanPaint)
            {
                if (this.x5486200f56f78413 != null)
                {
                    this.x5486200f56f78413.MdiContainer = null;
                    if (base.Target.Controls.Contains(this.x5486200f56f78413))
                    {
                        base.Target.Controls.Remove(this.x5486200f56f78413);
                    }
                }
                if ((base.Target.Menu != this.x946d4396b0c9cde4) && (this.x946d4396b0c9cde4 != null))
                {
                    base.Target.Menu = this.x946d4396b0c9cde4;
                }
                this.x33251e4c534e7926();
                if (this.CanPaintChild)
                {
                    this.x7ca6c09f91bb596a();
                }
            }
            else
            {
                this.x30c11b5a89921b08();
                this.x33251e4c534e7926();
                this.x7ca6c09f91bb596a();
                this.RegionWindow();
            }
            this.x05931f73c2d66689();
            base.OnCurrentSkinChanged(sender, e);
        }

        protected override void PaintBorder(Graphics g)
        {
            Rectangle rectangle;
            Rectangle rectangle2;
            Rectangle rectangle3;
            Rectangle rectangle4;
            base.PaintBorder(g);
            if (((base.Engine.LeftBorderWidth >= base.cxBorder) && (base.Engine.RightBorderWidth >= base.cxBorder)) || (this.x946d4396b0c9cde4 == null))
            {
                goto Label_0357;
            }
            int height = this.x5486200f56f78413.Height;
            Bitmap image = base.Engine.Res.Bitmaps.SKIN2_MENUBAR;
            if (base.Engine.LeftBorderWidth < base.cxBorder)
            {
                rectangle = new Rectangle(base.Engine.LeftBorderWidth, base.Engine.TitleHeight + this.x5486200f56f78413.Top, base.cxBorder - base.Engine.LeftBorderWidth, height);
                if (image != null)
                {
                    if (image.Width > (base.cxBorder - base.Engine.LeftBorderWidth))
                    {
                        rectangle2 = new Rectangle(0, 0, base.cxBorder - base.Engine.LeftBorderWidth, image.Height);
                    }
                    else
                    {
                        rectangle2 = new Rectangle(0, 0, image.Width, image.Height);
                    }
                    g.DrawImage(image, rectangle, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                }
                else
                {
                    if (base.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR != base.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR)
                    {
                        using (LinearGradientBrush brush = new LinearGradientBrush(rectangle, base.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR, base.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR, LinearGradientMode.Vertical))
                        {
                            g.FillRectangle(brush, rectangle);
                            goto Label_01CD;
                        }
                    }
                    g.FillRectangle(base.Engine.Res.Brushes.SKIN2_MENUBARSTARTCOLOR, rectangle);
                }
            }
        Label_01CD:
            if (base.Engine.RightBorderWidth < base.cxBorder)
            {
                rectangle = new Rectangle(this.Width - base.cxBorder, this.x5486200f56f78413.Top + base.Engine.TitleHeight, base.cxBorder - base.Engine.RightBorderWidth, height);
                if (image != null)
                {
                    if (image.Width > (base.cxBorder - base.Engine.RightBorderWidth))
                    {
                        rectangle2 = new Rectangle((image.Width - base.cxBorder) + base.Engine.RightBorderWidth, 0, base.cxBorder - base.Engine.RightBorderWidth, image.Height);
                    }
                    else
                    {
                        rectangle2 = new Rectangle(0, 0, image.Width, image.Height);
                    }
                    g.DrawImage(image, rectangle, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                }
                else
                {
                    if (base.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR != base.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR)
                    {
                        using (LinearGradientBrush brush2 = new LinearGradientBrush(rectangle, base.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR, base.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR, LinearGradientMode.Vertical))
                        {
                            g.FillRectangle(brush2, rectangle);
                            goto Label_0357;
                        }
                    }
                    g.FillRectangle(base.Engine.Res.Brushes.SKIN2_MENUBARSTARTCOLOR, rectangle);
                }
            }
        Label_0357:
            if (((base.Engine.LeftBorderWidth >= base.cxBorder) && (base.Engine.RightBorderWidth >= base.cxBorder)) || (base.Target.MainMenuStrip == null))
            {
                return;
            }
            int num2 = base.Target.MainMenuStrip.Height;
            Bitmap bitmap2 = base.Engine.Res.Bitmaps.SKIN2_MENUBAR;
            if (base.Engine.LeftBorderWidth < base.cxBorder)
            {
                rectangle3 = new Rectangle(base.Engine.LeftBorderWidth, base.Engine.TitleHeight + base.Target.MainMenuStrip.Top, base.cxBorder - base.Engine.LeftBorderWidth, num2);
                if (bitmap2 != null)
                {
                    if (bitmap2.Width > (base.cxBorder - base.Engine.LeftBorderWidth))
                    {
                        rectangle4 = new Rectangle(0, 0, base.cxBorder - base.Engine.LeftBorderWidth, bitmap2.Height);
                    }
                    else
                    {
                        rectangle4 = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
                    }
                    g.DrawImage(bitmap2, rectangle3, rectangle4.X, rectangle4.Y, rectangle4.Width, rectangle4.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                }
                else
                {
                    if (base.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR != base.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR)
                    {
                        using (LinearGradientBrush brush3 = new LinearGradientBrush(rectangle3, base.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR, base.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR, LinearGradientMode.Vertical))
                        {
                            g.FillRectangle(brush3, rectangle3);
                            goto Label_0539;
                        }
                    }
                    g.FillRectangle(base.Engine.Res.Brushes.SKIN2_MENUBARSTARTCOLOR, rectangle3);
                }
            }
        Label_0539:
            if (base.Engine.RightBorderWidth < base.cxBorder)
            {
                rectangle3 = new Rectangle(this.Width - base.cxBorder, base.Engine.TitleHeight + base.Target.MainMenuStrip.Top, base.cxBorder - base.Engine.RightBorderWidth, num2);
                if (bitmap2 != null)
                {
                    if (bitmap2.Width > (base.cxBorder - base.Engine.RightBorderWidth))
                    {
                        rectangle4 = new Rectangle((bitmap2.Width - base.cxBorder) + base.Engine.RightBorderWidth, 0, base.cxBorder - base.Engine.RightBorderWidth, bitmap2.Height);
                    }
                    else
                    {
                        rectangle4 = new Rectangle(0, 0, bitmap2.Width, bitmap2.Height);
                    }
                    g.DrawImage(bitmap2, rectangle3, rectangle4.X, rectangle4.Y, rectangle4.Width, rectangle4.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                    return;
                }
                if (base.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR != base.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR)
                {
                    using (LinearGradientBrush brush4 = new LinearGradientBrush(rectangle3, base.Engine.Res.Colors.SKIN2_MENUBARSTARTCOLOR, base.Engine.Res.Colors.SKIN2_MENUBARENDCOLOR, LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(brush4, rectangle3);
                        return;
                    }
                }
                g.FillRectangle(base.Engine.Res.Brushes.SKIN2_MENUBARSTARTCOLOR, rectangle3);
            }
        }

        protected override Point PointToScreen(Point point)
        {
            return base.Target.PointToScreen(point);
        }

        protected override void RefreshMainMenu()
        {
            if (this.x5486200f56f78413 != null)
            {
                this.x5486200f56f78413.Refresh();
            }
        }

        protected override void RefreshWindow()
        {
            base.Target.Refresh();
        }

        protected override void SkinControls()
        {
            if (!base.Target.IsDisposed)
            {
                base.Target.ControlAdded += new ControlEventHandler(this.xe08c104b24ddae95);
                base.Target.ControlRemoved += new ControlEventHandler(this.xbd90dc5580ff6562);
            }
            if (this.CanPaintChild)
            {
                ArrayList list = new ArrayList();
                foreach (Control control in base.Target.Controls)
                {
                    if (!(control is MdiClient) && !base.ControlTable.ContainsKey(control.Handle))
                    {
                        list.Add(control);
                    }
                }
                foreach (Control control2 in list)
                {
                    if (!base.ControlTable.ContainsKey(control2.Handle))
                    {
                        xbd3f2493841f18a1 xbdffa = xf3f6919ac5d158dc.Create(control2, base.Engine);
                        base.ControlTable.Add(control2.Handle, xbdffa);
                    }
                }
            }
        }

        private void x05931f73c2d66689()
        {
            if (this.CanPaint)
            {
                base.Target.BackColor = base.Engine.Res.Colors.SKIN2_FORMCOLOR;
            }
            else if (!this.IsDisposed && (this.BorderStyle != FormBorderStyle.None))
            {
                base.Target.BackColor = this.x7c26c2d810682d2e;
            }
        }

        private void x08d09b54d1a926bd()
        {
            if (base.Engine.SkinAllForm)
            {
                base.Target.MdiChildActivate += new EventHandler(this.x23daf02978257d54);
                this.x23daf02978257d54(null, null);
            }
        }

        private void x1d183dacf6b4e43c(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            base.x3b8ca818f0c2637e();
        }

        private void x23daf02978257d54(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            xa427f1b2281f554b xafbfb;
            foreach (Form form in base.Target.MdiChildren)
            {
                if (((form != null) && !form.IsDisposed) && (form.Visible && !base.ChildTable.ContainsKey(form.Handle)))
                {
                    xafbfb = new xa427f1b2281f554b(form.Handle, base.Engine);
                    base.ChildTable.Add(form.Handle, xafbfb);
                }
            }
            if (base.Target.ActiveMdiChild != null)
            {
                foreach (object obj3 in base.ChildTable.Keys)
                {
                    if (obj3 is IntPtr)
                    {
                        object obj2;
                        if (((IntPtr) obj3) == base.Target.ActiveMdiChild.Handle)
                        {
                            obj2 = base.ChildTable[obj3];
                            if (obj2 is xa427f1b2281f554b)
                            {
                                xafbfb = (xa427f1b2281f554b) obj2;
                                xafbfb.IsActive = true;
                            }
                        }
                        else
                        {
                            obj2 = base.ChildTable[obj3];
                            if (obj2 is xa427f1b2281f554b)
                            {
                                xafbfb = (xa427f1b2281f554b) obj2;
                                xafbfb.IsActive = false;
                                x61467fe65a98f20c.SendMessage((IntPtr) obj3, 0x85, IntPtr.Zero, IntPtr.Zero);
                            }
                        }
                    }
                }
            }
        }

        private void x30c11b5a89921b08()
        {
            if (base.Target.Menu == null)
            {
                if (base.Target.IsMdiContainer && (this.x5486200f56f78413 != null))
                {
                    this.x5486200f56f78413.MdiContainer = base.Target;
                }
            }
            else if ((!base.Target.IsMdiContainer || (base.Target.MainMenuStrip == null)) && this.CanPaint)
            {
                this.x946d4396b0c9cde4 = base.Target.Menu;
                base.Target.Menu = null;
                this.x45cc2ae14a69c414();
                if (base.Target.IsMdiContainer)
                {
                    this.x5486200f56f78413.MdiContainer = base.Target;
                }
                base.Target.Controls.Add(this.x5486200f56f78413);
                new x4fef14ebf3863c7f(this.x5486200f56f78413, this.x946d4396b0c9cde4);
            }
        }

        private void x33251e4c534e7926()
        {
            if (!base.Target.IsDisposed && (base.Target.WindowState == FormWindowState.Normal))
            {
                int num = base.Engine.TitleHeight + Math.Max(base.Engine.BottomBorderHeight, base.cyBorder);
                int num2 = Math.Max(base.Engine.LeftBorderWidth, base.cxBorder) + Math.Max(base.Engine.RightBorderWidth, base.cxBorder);
                if (this.CanPaint)
                {
                    base.Target.Height = (num + base.RestoreClientHeight) + this.MenuHeight;
                    base.Target.Width = num2 + base.RestoreClientWidth;
                }
                else
                {
                    base.Target.Height = base.RestoreHeight;
                    base.Target.Width = base.RestoreWidth;
                }
                if (!this.x71d812f69bdec219)
                {
                    this.x71d812f69bdec219 = true;
                    if (this.MenuHeight > 0)
                    {
                        foreach (Control control in base.Target.Controls)
                        {
                            if (control != this.x5486200f56f78413)
                            {
                                try
                                {
                                    if (control.Dock == DockStyle.None)
                                    {
                                        control.Top += this.MenuHeight;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
        }

        private void x45cc2ae14a69c414()
        {
            if (this.x946d4396b0c9cde4 != null)
            {
                x5f4b657f68f87baa xfbffbaa;
                if (this.x5486200f56f78413 == null)
                {
                    this.x5486200f56f78413 = new x3c41176af7e54b01(base.Engine);
                }
                if ((this.x946d4396b0c9cde4.RightToLeft & System.Windows.Forms.RightToLeft.Inherit) == System.Windows.Forms.RightToLeft.Inherit)
                {
                    this.x5486200f56f78413.RightToLeft = base.Target.RightToLeft;
                }
                else
                {
                    this.x5486200f56f78413.RightToLeft = this.x946d4396b0c9cde4.RightToLeft;
                }
                this.x5486200f56f78413.MenuCommands.Clear();
                ArrayList list = new ArrayList();
                ArrayList list2 = new ArrayList();
                foreach (MenuItem item2 in this.x946d4396b0c9cde4.MenuItems)
                {
                    xfbffbaa = this.xd010ef1851698a47(item2);
                    this.x5486200f56f78413.MenuCommands.Add(xfbffbaa);
                    if (item2.MenuItems.Count > 0)
                    {
                        list.Add(item2);
                        list2.Add(xfbffbaa);
                    }
                }
                while (list.Count > 0)
                {
                    MenuItem item = (MenuItem) list[0];
                    x5f4b657f68f87baa xfbffbaa2 = (x5f4b657f68f87baa) list2[0];
                    list.RemoveAt(0);
                    list2.RemoveAt(0);
                    foreach (MenuItem item3 in item.MenuItems)
                    {
                        xfbffbaa = this.xd010ef1851698a47(item3);
                        xfbffbaa2.MenuCommands.Add(xfbffbaa);
                        if (item3.MenuItems.Count > 0)
                        {
                            list.Add(item3);
                            list2.Add(xfbffbaa);
                        }
                    }
                }
            }
        }

        private void x7ca6c09f91bb596a()
        {
            if (this.CanPaintChild)
            {
                ArrayList list = new ArrayList();
                foreach (Control control in base.Target.Controls)
                {
                    if (!(control is MdiClient) && !base.ControlTable.ContainsKey(control.Handle))
                    {
                        list.Add(control);
                    }
                }
                foreach (Control control2 in list)
                {
                    if (!base.ControlTable.ContainsKey(control2.Handle))
                    {
                        xbd3f2493841f18a1 xbdffa = xf3f6919ac5d158dc.Create(control2, base.Engine);
                        base.ControlTable.Add(control2.Handle, xbdffa);
                    }
                }
            }
        }

        private void xbd90dc5580ff6562(object xe0292b9ed559da7d, ControlEventArgs xfbf34718e704c6bc)
        {
        }

        private void xbe3f57786c90d37f()
        {
            if ((this.CanPaint && (this.x5486200f56f78413 != null)) && (this.x946d4396b0c9cde4 != null))
            {
                this.xe09f26c14e176ef5();
            }
        }

        private x5f4b657f68f87baa xd010ef1851698a47(MenuItem xccb63ca5f63dc470)
        {
            x5f4b657f68f87baa xfbffbaa = new x5f4b657f68f87baa {
                AttachedMenuItem = xccb63ca5f63dc470
            };
            xfbffbaa.Click += new EventHandler(this.xe26186777e7e0508);
            return xfbffbaa;
        }

        private void xe08c104b24ddae95(object xe0292b9ed559da7d, ControlEventArgs xfbf34718e704c6bc)
        {
            if (this.CanPaintChild)
            {
                Control control = xfbf34718e704c6bc.Control;
                if (!(control is MdiClient) && !base.ControlTable.ContainsKey(control.Handle))
                {
                    xbd3f2493841f18a1 xbdffa = xf3f6919ac5d158dc.Create(control, base.Engine);
                    base.ControlTable.Add(control.Handle, xbdffa);
                }
            }
        }

        private void xe09f26c14e176ef5()
        {
            x5f4b657f68f87baa xfbffbaa;
            this.x5486200f56f78413.MenuCommands.Clear();
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            foreach (MenuItem item2 in this.x946d4396b0c9cde4.MenuItems)
            {
                xfbffbaa = this.xd010ef1851698a47(item2);
                this.x5486200f56f78413.MenuCommands.Add(xfbffbaa);
                if (item2.MenuItems.Count > 0)
                {
                    list.Add(item2);
                    list2.Add(xfbffbaa);
                }
            }
            while (list.Count > 0)
            {
                MenuItem item = (MenuItem) list[0];
                x5f4b657f68f87baa xfbffbaa2 = (x5f4b657f68f87baa) list2[0];
                list.RemoveAt(0);
                list2.RemoveAt(0);
                foreach (MenuItem item3 in item.MenuItems)
                {
                    xfbffbaa = this.xd010ef1851698a47(item3);
                    xfbffbaa2.MenuCommands.Add(xfbffbaa);
                    if (item3.MenuItems.Count > 0)
                    {
                        list.Add(item3);
                        list2.Add(xfbffbaa);
                    }
                }
            }
        }

        private void xe26186777e7e0508(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            if (xe0292b9ed559da7d is x5f4b657f68f87baa)
            {
                x5f4b657f68f87baa xfbffbaa = (x5f4b657f68f87baa) xe0292b9ed559da7d;
                if (xfbffbaa.AttachedMenuItem != null)
                {
                    xfbffbaa.AttachedMenuItem.PerformClick();
                }
            }
        }

        protected override FormBorderStyle BorderStyle
        {
            get
            {
                return base.Target.FormBorderStyle;
            }
        }

        protected override Rectangle ClientRectangle
        {
            get
            {
                return base.Target.ClientRectangle;
            }
        }

        protected override bool ControlBox
        {
            get
            {
                return base.Target.ControlBox;
            }
        }

        protected override int Height
        {
            get
            {
                return base.Target.Height;
            }
        }

        protected override bool HelpButton
        {
            get
            {
                return base.Target.HelpButton;
            }
        }

        protected override System.Drawing.Icon Icon
        {
            get
            {
                switch (base.Target.FormBorderStyle)
                {
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return null;
                }
                return base.Target.Icon;
            }
        }

        protected override bool IsActive
        {
            get
            {
                if (base.Target.IsMdiChild)
                {
                    return (base.Target == base.Target.MdiParent.ActiveMdiChild);
                }
                if (base.Target.IsMdiContainer)
                {
                    return (base.Target == Form.ActiveForm);
                }
                return base.IsActive;
            }
            set
            {
                base.IsActive = value;
            }
        }

        protected override bool IsDisposed
        {
            get
            {
                return base.Target.IsDisposed;
            }
        }

        protected override bool IsMdiChild
        {
            get
            {
                return base.Target.IsMdiChild;
            }
        }

        protected override bool IsMdiContainer
        {
            get
            {
                return base.Target.IsMdiContainer;
            }
        }

        protected override bool MaximizeBox
        {
            get
            {
                switch (base.Target.FormBorderStyle)
                {
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return false;
                }
                return base.Target.MaximizeBox;
            }
        }

        protected override int MenuHeight
        {
            get
            {
                if ((this.x946d4396b0c9cde4 != null) && (this.x5486200f56f78413 != null))
                {
                    return this.x5486200f56f78413.Height;
                }
                return 0;
            }
        }

        protected override bool MinimizeBox
        {
            get
            {
                switch (base.Target.FormBorderStyle)
                {
                    case FormBorderStyle.FixedToolWindow:
                    case FormBorderStyle.SizableToolWindow:
                        return false;
                }
                return base.Target.MinimizeBox;
            }
        }

        protected override System.Drawing.Region Region
        {
            get
            {
                return base.Target.Region;
            }
            set
            {
                base.Target.Region = value;
            }
        }

        protected override System.Windows.Forms.RightToLeft RightToLeft
        {
            get
            {
                return base.Target.RightToLeft;
            }
        }

        protected override bool RightToLeftLayout
        {
            get
            {
                return (base.Target.RightToLeftLayout && ((base.Target.RightToLeft & System.Windows.Forms.RightToLeft.Yes) == System.Windows.Forms.RightToLeft.Yes));
            }
        }

        protected override object Tag
        {
            get
            {
                return base.Target.Tag;
            }
        }

        protected override string Text
        {
            get
            {
                return base.Target.Text;
            }
        }

        protected override int TitleHeight
        {
            get
            {
                if ((this.IsMdiChild && (this.WindowState == FormWindowState.Maximized)) && (this.x946d4396b0c9cde4 != null))
                {
                    return (this.x5486200f56f78413.Height - 1);
                }
                return base.Engine.TitleHeight;
            }
        }

        protected override int Width
        {
            get
            {
                return base.Target.Width;
            }
        }
    }
}

