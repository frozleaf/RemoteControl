namespace Sunisoft.IrisSkin
{
    using Sunisoft.IrisSkin.InternalControls;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    internal abstract class xd24df615efe9450e : NativeWindow
    {
        protected Bitmap BottomBuffer;
        protected Bitmap[] ButtonBuffer;
        protected Bitmap CaptionBuffer;
        protected Hashtable ChildTable = new Hashtable();
        protected Hashtable ControlTable = new Hashtable();
        protected int cxBorder = 4;
        protected int cyBorder = 4;
        protected SkinEngine Engine;
        protected uint ExtStyle;
        protected bool Initializing = true;
        protected int IsNCMouseDown = -1;
        protected int LastMousePosition = 2;
        protected Rectangle[] NCRects;
        protected Color OriBackColor = Color.FromKnownColor(KnownColor.Control);
        protected uint OriStyle;
        protected int RestoreClientHeight;
        protected int RestoreClientWidth;
        protected int RestoreHeight;
        protected int RestoreWidth;
        protected Form Target;
        protected Bitmap TitleBuffer;
        private bool x230e512478b11e00 = true;
        private int x2b4c907bdfe9f160;
        private int x4cf0eb77a23bef26;
        private bool x507503a87814ae33;
        private int x5a29066520654c5a;
        private int x6b81454f45088f2e;
        private string xa4b5d355cbf01a80;
        private bool xa6ac12f4249c7b54;
        private x902c4aee45bfd906 xcdc1a69e212e5aa9;
        private bool xfd408d62a1acef9b = true;

        public xd24df615efe9450e(IntPtr handle, SkinEngine engine)
        {
            this.Engine = engine;
            this.Target = (Form) Control.FromHandle(handle);
            base.AssignHandle(handle);
            this.GetWindowRestoreSize();
            this.x18f04d42fe6eab01();
            this.SaveWindowStyle();
            if (this.CanPaint)
            {
                this.ResetWindowStyle();
            }
            this.SkinControls();
            this.ApplySkin();
            this.Engine.CurrentSkinChanged += new SkinChanged(this.OnCurrentSkinChanged);
            this.Initializing = false;
        }

        protected virtual unsafe void AfterWndProc(ref Message m)
        {
            uint msg = (uint) m.Msg;
            if (msg <= 0x86)
            {
                int num;
                switch (msg)
                {
                    case 0x21:
                        if (!this.IsDisposed && (this.TitleHeight < SystemInformation.CaptionHeight))
                        {
                            this.RefreshWindow();
                        }
                        return;

                    case 0x22:
                        if (!this.IsDisposed && (this.TitleHeight < SystemInformation.CaptionHeight))
                        {
                            this.RefreshWindow();
                        }
                        return;

                    case 130:
                        if (this.Target != null)
                        {
                            this.Target.Width = this.RestoreWidth;
                            this.Target.Height = this.RestoreHeight;
                        }
                        return;

                    case 0x83:
                        if (m.WParam.ToInt32() != 0)
                        {
                            x40255b11ef821fa3.NCCALCSIZE_PARAMS* lParam = (x40255b11ef821fa3.NCCALCSIZE_PARAMS*) m.LParam;
                            x40255b11ef821fa3.WINDOWPOS* lppos = (x40255b11ef821fa3.WINDOWPOS*) lParam->lppos;
                            lParam->rgrc0.Y = lppos->y + this.TitleHeight;
                            if ((this.WindowState == FormWindowState.Maximized) && this.IsMdiChild)
                            {
                                lParam->rgrc0.Width = lppos->x + lppos->cx;
                                lParam->rgrc0.X = lppos->x;
                                lParam->rgrc0.Height = lppos->y + lppos->cy;
                                lParam->rgrc1 = lParam->rgrc0;
                            }
                            else
                            {
                                lParam->rgrc0.Width = (lppos->x + lppos->cx) - Math.Max(this.Engine.RightBorderWidth, this.cxBorder);
                                lParam->rgrc0.X = lppos->x + Math.Max(this.Engine.LeftBorderWidth, this.cxBorder);
                                lParam->rgrc0.Height = (lppos->y + lppos->cy) - Math.Max(this.Engine.BottomBorderHeight, this.cyBorder);
                                lParam->rgrc1 = lParam->rgrc0;
                            }
                            m.Result = new IntPtr(0x400L);
                            this.x271f237e4704798d();
                            return;
                        }
                        return;

                    case 0x84:
                    case 0x85:
                        return;

                    case 0x86:
                        num = m.WParam.ToInt32();
                        if (!this.IsMdiChild)
                        {
                            if (num != 0)
                            {
                                this.IsActive = true;
                            }
                            else
                            {
                                this.IsActive = false;
                            }
                            this.x3b8ca818f0c2637e();
                            this.RefreshMainMenu();
                            return;
                        }
                        this.x3b8ca818f0c2637e();
                        return;

                    case 5:
                        x61467fe65a98f20c.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 0x37);
                        this.RegionWindow();
                        this.IsNCMouseDown = -1;
                        num = m.WParam.ToInt32();
                        if (this.Target != null)
                        {
                            switch (((uint) num))
                            {
                                case 0:
                                    this.x2b4c907bdfe9f160 = this.Target.Width;
                                    this.x4cf0eb77a23bef26 = this.Target.Height;
                                    this.xa6ac12f4249c7b54 = true;
                                    return;

                                case 1:
                                    if (this.xa6ac12f4249c7b54)
                                    {
                                        this.Target.Width = this.x2b4c907bdfe9f160;
                                        this.Target.Height = this.x4cf0eb77a23bef26;
                                    }
                                    return;

                                case 2:
                                    if (this.xa6ac12f4249c7b54)
                                    {
                                        this.Target.Width = this.x2b4c907bdfe9f160;
                                        this.Target.Height = this.x4cf0eb77a23bef26;
                                    }
                                    return;
                            }
                        }
                        return;

                    case 6:
                        num = m.WParam.ToInt32();
                        if (!this.IsMdiChild)
                        {
                            if (((num & 1L) == 1L) || ((num & 2L) == 2L))
                            {
                                this.IsActive = true;
                            }
                            else
                            {
                                this.IsActive = false;
                            }
                            this.x3b8ca818f0c2637e();
                            this.RefreshMainMenu();
                            if (!this.IsDisposed && (this.TitleHeight < SystemInformation.CaptionHeight))
                            {
                                this.RefreshWindow();
                            }
                            return;
                        }
                        this.x3b8ca818f0c2637e();
                        this.RefreshWindow();
                        return;

                    case 12:
                        this.x271f237e4704798d();
                        this.x3b8ca818f0c2637e();
                        return;
                }
            }
            else
            {
                if (msg <= 310)
                {
                    if (msg == 0xa1)
                    {
                        if (m.WParam.ToInt32() == 3)
                        {
                            this.DoSysMenu();
                        }
                        return;
                    }
                    if (msg != 310)
                    {
                        return;
                    }
                }
                else
                {
                    switch (msg)
                    {
                        case 0x214:
                        {
                            int num2 = m.LParam.ToInt32();
                            int formMinWidth = this.Engine.FormMinWidth;
                            if (!this.HelpButton)
                            {
                                formMinWidth -= 6;
                            }
                            if (!this.MinimizeBox)
                            {
                                formMinWidth -= 6;
                            }
                            if (!this.MaximizeBox)
                            {
                                formMinWidth -= 6;
                            }
                            int num4 = (this.Engine.TitleHeight + Math.Max(this.Engine.BottomBorderHeight, this.cyBorder)) + this.MenuHeight;
                            xae4dd1cafd2eb77c.RECT* rectPtr = (xae4dd1cafd2eb77c.RECT*) num2;
                            if ((rectPtr->right - rectPtr->left) < formMinWidth)
                            {
                                rectPtr->right = rectPtr->left + formMinWidth;
                            }
                            if ((rectPtr->bottom - rectPtr->top) < num4)
                            {
                                rectPtr->bottom = rectPtr->top + num4;
                            }
                            return;
                        }
                        case 0x222:
                            if (!this.IsDisposed && (this.TitleHeight < SystemInformation.CaptionHeight))
                            {
                                this.RefreshWindow();
                            }
                            return;
                    }
                    return;
                }
                m.Result = this.Engine.BackColorBrush;
            }
        }

        protected virtual void ApplySkin()
        {
            if (this.CanPaint)
            {
                x61467fe65a98f20c.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 0x37);
            }
        }

        protected virtual bool BeforeWndProc(ref Message m)
        {
            int num;
            switch (((uint) m.Msg))
            {
                case 0x84:
                    m.Result = new IntPtr((long) this.x13d7e7b52a69e5dc());
                    return false;

                case 0x85:
                    this.x3b8ca818f0c2637e();
                    return false;

                case 0x86:
                    if (m.WParam.ToInt32() != 0)
                    {
                        break;
                    }
                    m.Result = new IntPtr(1);
                    return false;

                case 70:
                    if (this.RightToLeftLayout)
                    {
                        this.ExtStyle = x61467fe65a98f20c.GetWindowLong(base.Handle, -20);
                        if ((this.ExtStyle & 0x400000) == 0x400000)
                        {
                            x61467fe65a98f20c.SetWindowLong(base.Handle, -20, (uint) (this.ExtStyle - 0x400000));
                        }
                    }
                    break;

                case 0x20:
                    this.ResetWindowStyle();
                    break;

                case 0x313:
                {
                    Point lpPoint = new Point(0, 0);
                    x61467fe65a98f20c.GetCursorPos(ref lpPoint);
                    this.xd308f013f560f5be(lpPoint);
                    return false;
                }
                case 0x20d1:
                    m.Result = new IntPtr(0x20d1);
                    return false;

                case 160:
                    num = m.WParam.ToInt32();
                    if (((num != 9) && (num != 8)) && ((num != 20) && (num != 0x15)))
                    {
                        this.IsNCMouseDown = -1;
                    }
                    if (((this.LastMousePosition == 9) || (this.LastMousePosition == 8)) || ((this.LastMousePosition == 20) || (this.LastMousePosition == 0x15)))
                    {
                        switch (num)
                        {
                            case 9:
                            case 8:
                            case 20:
                            case 0x15:
                                if (this.LastMousePosition != num)
                                {
                                    this.x6616b01fd7cbc65d(0);
                                    this.LastMousePosition = 0;
                                    this.x6616b01fd7cbc65d(num);
                                }
                                goto Label_029E;
                        }
                        this.x6616b01fd7cbc65d(num);
                    }
                    else
                    {
                        switch (num)
                        {
                            case 9:
                            case 8:
                            case 20:
                            case 0x15:
                                this.x6616b01fd7cbc65d(num);
                                break;
                        }
                    }
                    goto Label_029E;

                case 0xa1:
                    num = m.WParam.ToInt32();
                    this.IsNCMouseDown = num;
                    if (((this.IsNCMouseDown == 0x15) || (this.IsNCMouseDown == 8)) || ((this.IsNCMouseDown == 9) || (this.IsNCMouseDown == 20)))
                    {
                        this.x6616b01fd7cbc65d(num);
                        this.LastMousePosition = num;
                        return false;
                    }
                    if (((this.Text != "") || this.ControlBox) || (num != 2))
                    {
                        if (num == 0)
                        {
                            return false;
                        }
                        if ((num != 2) || (this.WindowState != FormWindowState.Maximized))
                        {
                            break;
                        }
                        m.WParam = new IntPtr(0);
                        x61467fe65a98f20c.SetWindowPos(base.Handle, new IntPtr(0), 0, 0, 0, 0, 3);
                    }
                    return false;

                case 0xa2:
                {
                    uint num3 = this.x13d7e7b52a69e5dc();
                    if (num3 == this.IsNCMouseDown)
                    {
                        switch (num3)
                        {
                            case 8:
                                this.DoMin();
                                break;

                            case 9:
                                this.DoMax();
                                break;

                            case 20:
                                this.DoClose();
                                break;

                            case 0x15:
                                this.DoHelp();
                                break;
                        }
                    }
                    this.IsNCMouseDown = -1;
                    return false;
                }
                case 0xa3:
                    if ((this.Text != "") || this.ControlBox)
                    {
                        num = m.WParam.ToInt32();
                        if (num == 2)
                        {
                            if (this.MaximizeBox)
                            {
                                this.DoMax();
                            }
                            this.IsNCMouseDown = -1;
                            return false;
                        }
                        if (num == 3)
                        {
                            this.DoClose();
                            this.IsNCMouseDown = -1;
                            this.xd1faceba1dee38cf.Dismiss();
                            return false;
                        }
                        break;
                    }
                    return false;

                case 0xa4:
                    if (this.ControlBox && (m.WParam.ToInt32() == 2))
                    {
                        int num2 = m.LParam.ToInt32();
                        Point p = new Point(num2 & 0xffff, (int) ((num2 & 0xffff0000L) >> 0x10));
                        this.DoSysMenu(p);
                    }
                    return false;

                case 0xae:
                    return false;
            }
            return true;
        Label_029E:
            this.LastMousePosition = num;
            return false;
        }

        protected virtual void DoClose()
        {
            x61467fe65a98f20c.PostMessage(base.Handle, 0x112, (uint) 0xf060, (uint) 0);
        }

        protected virtual void DoHelp()
        {
            x61467fe65a98f20c.PostMessage(base.Handle, 0x112, (uint) 0xf180, (uint) 0);
        }

        protected virtual void DoMax()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        protected virtual void DoMin()
        {
            if (this.IsMdiChild && (this.WindowState == FormWindowState.Minimized))
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        protected virtual void DoSysMenu()
        {
            this.xd1faceba1dee38cf.MenuCommands[0].Enabled = this.MinimizeBox;
            this.xd1faceba1dee38cf.MenuCommands[1].Enabled = this.MaximizeBox;
            if (this.RightToLeftLayout)
            {
                this.xd1faceba1dee38cf.TrackPopup(this.PointToScreen(new Point(this.NCRects[3].X, this.NCRects[3].Bottom - this.TitleHeight)));
            }
            else
            {
                this.xd1faceba1dee38cf.TrackPopup(this.PointToScreen(new Point(0, 0)));
            }
        }

        protected virtual void DoSysMenu(Point p)
        {
            this.xd1faceba1dee38cf.MenuCommands[0].Enabled = this.MinimizeBox;
            this.xd1faceba1dee38cf.MenuCommands[1].Enabled = this.MaximizeBox;
            this.xd1faceba1dee38cf.TrackPopup(p);
        }

        protected virtual void GetWindowRestoreSize()
        {
            x40255b11ef821fa3.WINDOWPLACEMENT lpwndpl = new x40255b11ef821fa3.WINDOWPLACEMENT();
            x61467fe65a98f20c.GetWindowPlacement(base.Handle, ref lpwndpl);
            this.RestoreWidth = lpwndpl.rcNormalPosition.Width - lpwndpl.rcNormalPosition.X;
            this.RestoreHeight = lpwndpl.rcNormalPosition.Height - lpwndpl.rcNormalPosition.Y;
            if (this.Target != null)
            {
                if (((this.Target.WindowState == FormWindowState.Normal) && (this.Target.ClientRectangle.Width != 0)) && (this.Target.ClientRectangle.Height != 0))
                {
                    this.RestoreClientWidth = this.Target.ClientRectangle.Width;
                    this.RestoreClientHeight = this.Target.ClientRectangle.Height;
                }
                else
                {
                    this.RestoreClientHeight = (this.RestoreHeight - SystemInformation.CaptionHeight) - this.cyBorder;
                    if (this.Target.Menu != null)
                    {
                        this.RestoreClientHeight -= SystemInformation.MenuHeight;
                    }
                    this.RestoreClientWidth = this.RestoreWidth - (2 * this.cxBorder);
                }
            }
            else
            {
                Rectangle lpRect = new Rectangle();
                x61467fe65a98f20c.GetClientRect(base.Handle, ref lpRect);
                this.RestoreClientWidth = lpRect.Width;
                this.RestoreClientHeight = lpRect.Height;
            }
        }

        protected virtual void OnCurrentSkinChanged(object sender, SkinChangedEventArgs e)
        {
            this.ResetWindowStyle();
            if (!this.CanPaint)
            {
                this.Region = null;
                if (this.RightToLeftLayout)
                {
                    if ((this.ExtStyle & 0x400000) == 0)
                    {
                        this.ExtStyle += 0x400000;
                    }
                    x61467fe65a98f20c.SetWindowLong(base.Handle, -20, this.ExtStyle);
                }
            }
            else if (this.RightToLeftLayout && ((this.ExtStyle & 0x400000) == 0x400000))
            {
                x61467fe65a98f20c.SetWindowLong(base.Handle, -20, (uint) (this.ExtStyle - 0x400000));
            }
            x61467fe65a98f20c.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 0x37);
        }

        protected virtual void PaintBorder(Graphics g)
        {
            Rectangle rectangle;
            int width = this.Width;
            int height = this.Height;
            int menuHeight = 0;
            if (this.MenuHeight != 0)
            {
                menuHeight = this.MenuHeight;
            }
            Bitmap image = this.Engine.Res.Bitmaps.SKIN2_FORMLEFTBORDER;
            if (image.Width < this.cxBorder)
            {
                rectangle = Rectangle.FromLTRB(0, this.TitleHeight, this.cxBorder, height - this.Engine.BottomBorderHeight);
                g.FillRectangle(this.Engine.Res.Brushes.SKIN2_FORMCOLOR, rectangle);
                if (menuHeight > 0)
                {
                    rectangle = Rectangle.FromLTRB(this.cxBorder - image.Width, this.TitleHeight, this.cxBorder, this.TitleHeight + menuHeight);
                }
            }
            rectangle = Rectangle.FromLTRB(0, this.TitleHeight, this.Engine.LeftBorderWidth, height - this.Engine.BottomBorderHeight);
            Rectangle rectangle2 = new Rectangle(0, 0, image.Width, image.Height);
            g.DrawImage(image, rectangle, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY, null);
            image = this.Engine.Res.Bitmaps.SKIN2_FORMRIGHTBORDER;
            if (image.Width < this.cxBorder)
            {
                rectangle = Rectangle.FromLTRB(width - this.cxBorder, this.TitleHeight, width, height - this.Engine.BottomBorderHeight);
                g.FillRectangle(this.Engine.Res.Brushes.SKIN2_FORMCOLOR, rectangle);
            }
            rectangle = Rectangle.FromLTRB(width - this.Engine.RightBorderWidth, this.TitleHeight, width, height - this.Engine.BottomBorderHeight);
            rectangle2 = new Rectangle(0, 0, image.Width, image.Height);
            g.DrawImage(image, rectangle, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY, null);
        }

        protected abstract Point PointToScreen(Point point);
        protected virtual void RefreshMainMenu()
        {
        }

        protected virtual void RefreshWindow()
        {
        }

        protected virtual unsafe void RegionWindow()
        {
            if (this.IsMdiChild && (this.WindowState != FormWindowState.Normal))
            {
                if ((this.Width > 0) && (this.Height > 0))
                {
                    this.Region = new System.Drawing.Region(new Rectangle(0, 0, this.Width, this.Height));
                }
            }
            else
            {
                if (((this.TitleBuffer == null) || (this.BottomBuffer == null)) || (this.TitleBuffer.Width != this.Width))
                {
                    this.x58714ffe12c688d9();
                    this.x3c424b74aaf6d003();
                }
                if (!this.Engine.Res.Bools.SKIN2_TITLEBARNEEDREGION && !this.Engine.Res.Bools.SKIN2_BOTTOMBORDERNEEDREGION)
                {
                    this.Region = new System.Drawing.Region(new Rectangle(0, 0, this.Width, this.Height));
                }
                else
                {
                    int num;
                    int num2;
                    int num3;
                    int num4;
                    int num8;
                    int num9;
                    byte num10;
                    Bitmap titleBuffer = this.TitleBuffer;
                    int num7 = 4;
                    int height = titleBuffer.Height;
                    int width = titleBuffer.Width;
                    if (width < this.Width)
                    {
                        this.x58714ffe12c688d9();
                        this.x3c424b74aaf6d003();
                        this.x9d937faa8065bfbb();
                        titleBuffer = this.TitleBuffer;
                        height = titleBuffer.Height;
                        width = titleBuffer.Width;
                    }
                    BitmapData bitmapdata = null;
                    bool flag = true;
                    GraphicsPath path = new GraphicsPath();
                    if (this.Engine.Res.Bools.SKIN2_TITLEBARNEEDREGION)
                    {
                        num9 = this.Engine.Res.Integers.SKIN2_TITLEBARREGIONMINY;
                        num8 = this.Engine.Res.Integers.SKIN2_TITLEBARREGIONMAXY;
                        path.AddRectangle(new Rectangle(0, 0, width, num9));
                        path.AddRectangle(Rectangle.FromLTRB(0, num8 + 1, width, height));
                        try
                        {
                            bitmapdata = titleBuffer.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                            byte* numPtr = (byte*) bitmapdata.Scan0;
                            numPtr += 3 + ((num7 * num9) * width);
                            for (num = num9; num <= num8; num++)
                            {
                                num3 = 0;
                                num4 = 0;
                                flag = true;
                                num2 = 0;
                                while (num2 < width)
                                {
                                    num10 = numPtr[0];
                                    if ((num10 == 0) || (num2 == (width - 1)))
                                    {
                                        if ((num2 == (width - 1)) && (num10 != 0))
                                        {
                                            path.AddRectangle(Rectangle.FromLTRB(num3, num, num2 + 1, num + 1));
                                        }
                                        else if (!flag)
                                        {
                                            path.AddRectangle(Rectangle.FromLTRB(num3, num, num4 + 1, num + 1));
                                        }
                                        flag = true;
                                    }
                                    else
                                    {
                                        if (flag)
                                        {
                                            num3 = num2;
                                            num4 = num2;
                                        }
                                        else
                                        {
                                            num4++;
                                        }
                                        flag = false;
                                    }
                                    numPtr += num7;
                                    num2++;
                                }
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            if (bitmapdata != null)
                            {
                                titleBuffer.UnlockBits(bitmapdata);
                            }
                        }
                    }
                    else
                    {
                        path.AddRectangle(new Rectangle(0, 0, width, height));
                    }
                    path.AddRectangle(Rectangle.FromLTRB(0, height, this.Width, this.Height - this.BottomBuffer.Height));
                    bitmapdata = null;
                    titleBuffer = this.BottomBuffer;
                    height = titleBuffer.Height;
                    width = titleBuffer.Width;
                    int y = this.Height - height;
                    if (this.Engine.Res.Bools.SKIN2_BOTTOMBORDERNEEDREGION)
                    {
                        num9 = this.Engine.Res.Integers.SKIN2_BOTTOMREGIONMINY;
                        num8 = this.Engine.Res.Integers.SKIN2_BOTTOMREGIONMAXY;
                        path.AddRectangle(new Rectangle(0, y, width, num9));
                        if (((num8 + y) + 1) < this.Height)
                        {
                            path.AddRectangle(Rectangle.FromLTRB(0, (num8 + y) + 1, width, this.Height));
                        }
                        try
                        {
                            bitmapdata = titleBuffer.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                            byte* numPtr2 = (byte*) bitmapdata.Scan0;
                            numPtr2 += 3 + ((num7 * num9) * width);
                            for (num = num9; num <= num8; num++)
                            {
                                num3 = 0;
                                num4 = 0;
                                flag = true;
                                for (num2 = 0; num2 < width; num2++)
                                {
                                    num10 = numPtr2[0];
                                    if ((num10 == 0) || (num2 == (width - 1)))
                                    {
                                        if ((num2 == (width - 1)) && (num10 != 0))
                                        {
                                            path.AddRectangle(Rectangle.FromLTRB(num3, num + y, num2 + 1, (num + 1) + y));
                                        }
                                        else if (!flag)
                                        {
                                            path.AddRectangle(Rectangle.FromLTRB(num3, num + y, num4 + 1, (num + 1) + y));
                                        }
                                        flag = true;
                                    }
                                    else
                                    {
                                        if (flag)
                                        {
                                            num3 = num2;
                                            num4 = num2;
                                        }
                                        else
                                        {
                                            num4++;
                                        }
                                        flag = false;
                                    }
                                    numPtr2 += num7;
                                }
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            if (bitmapdata != null)
                            {
                                titleBuffer.UnlockBits(bitmapdata);
                            }
                        }
                    }
                    else
                    {
                        path.AddRectangle(new Rectangle(0, y, width, height));
                    }
                    this.Region = new System.Drawing.Region(path);
                }
            }
        }

        protected virtual void ResetWindowStyle()
        {
            uint windowLong = x61467fe65a98f20c.GetWindowLong(base.Handle, -16);
            if (this.CanPaint)
            {
                if ((windowLong & 0x80000) == 0x80000)
                {
                    this.x507503a87814ae33 = true;
                    windowLong -= 0x80000;
                }
            }
            else if (this.x507503a87814ae33)
            {
                windowLong |= 0x80000;
                this.x507503a87814ae33 = false;
            }
            if (!this.IsDisposed && (this.BorderStyle != FormBorderStyle.None))
            {
                x61467fe65a98f20c.SetWindowLong(base.Handle, -16, windowLong);
            }
        }

        protected virtual void SaveWindowStyle()
        {
            this.OriStyle = x61467fe65a98f20c.GetWindowLong(base.Handle, -16);
            this.ExtStyle = x61467fe65a98f20c.GetWindowLong(base.Handle, -20);
            if ((this.RightToLeftLayout && this.CanPaint) && ((this.ExtStyle & 0x400000) == 0x400000))
            {
                x61467fe65a98f20c.SetWindowLong(base.Handle, -20, (uint) (this.ExtStyle - 0x400000));
            }
        }

        protected abstract void SkinControls();
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

        private int x0c7b47d7852a0e99()
        {
            int num = this.Engine.Res.Integers.SKIN2_TITLEBARBUTTONPOSY;
            if ((this.WindowState == FormWindowState.Minimized) && this.IsMdiChild)
            {
                Bitmap bitmap = this.Engine.Res.SplitBitmaps.SKIN2_TITLEBUTTONS[1];
                num = Math.Max((0x1a - bitmap.Height) / 2, 0);
                if ((num + bitmap.Height) < 0x16)
                {
                    num += 2;
                }
            }
            if (num < 4)
            {
                num = 4;
            }
            return num;
        }

        private uint x13d7e7b52a69e5dc()
        {
            Point lpPoint = new Point(0, 0);
            if (x61467fe65a98f20c.GetCursorPos(ref lpPoint))
            {
                if (!x61467fe65a98f20c.ScreenToClient(base.Handle, ref lpPoint))
                {
                    return 0;
                }
                if (this.NCRects == null)
                {
                    return 0;
                }
                lpPoint.X += Math.Max(this.Engine.LeftBorderWidth, 4);
                lpPoint.Y += this.TitleHeight;
                if (this.IsMdiChild && (this.WindowState == FormWindowState.Maximized))
                {
                    return 1;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[3]))
                {
                    return 3;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[20]))
                {
                    return 20;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[9]))
                {
                    return 9;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[8]))
                {
                    return 8;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[8]))
                {
                    return 8;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[0x15]))
                {
                    return 0x15;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[0x10]))
                {
                    return 0x10;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[0x11]))
                {
                    return 0x11;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[13]))
                {
                    return 13;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[14]))
                {
                    return 14;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[10]))
                {
                    return 10;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[11]))
                {
                    return 11;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[12]))
                {
                    return 12;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[15]))
                {
                    return 15;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[2]))
                {
                    return 2;
                }
                if (x448fd9ab43628c71.InRect(lpPoint, this.NCRects[1]))
                {
                    return 1;
                }
            }
            return 0;
        }

        protected void x18f04d42fe6eab01()
        {
            x40255b11ef821fa3.WINDOWINFO pwi = new x40255b11ef821fa3.WINDOWINFO();
            x61467fe65a98f20c.GetWindowInfo(base.Handle, ref pwi);
            this.cxBorder = (int) pwi.cxWindowBorders;
            this.cyBorder = (int) pwi.cyWindowBorders;
        }

        private void x271f237e4704798d()
        {
            int formCaptionPosX;
            this.NCRects = new Rectangle[0x17];
            int width = this.Engine.Res.SplitBitmaps.SKIN2_TITLEBUTTONS[1].Width;
            int height = this.Engine.Res.SplitBitmaps.SKIN2_TITLEBUTTONS[1].Height;
            int y = this.x0c7b47d7852a0e99();
            int num4 = this.Engine.Res.Integers.SKIN2_TITLEBARICONPOSX;
            if (this.ControlBox)
            {
                formCaptionPosX = num4 + 4;
                this.NCRects[3] = new Rectangle(formCaptionPosX, y, 0x10, 0x10);
                num4 = formCaptionPosX + 0x12;
                int rightBorderWidth = this.Engine.Res.Integers.SKIN2_TITLEBARBUTTONPOSX;
                if (rightBorderWidth < this.Engine.RightBorderWidth)
                {
                    rightBorderWidth = this.Engine.RightBorderWidth;
                }
                formCaptionPosX = (this.Width - rightBorderWidth) - width;
                this.NCRects[20] = new Rectangle(formCaptionPosX, y, width, height);
                formCaptionPosX -= width + 2;
                if (this.MaximizeBox)
                {
                    this.NCRects[9] = new Rectangle(formCaptionPosX, y, width, height);
                    formCaptionPosX -= width + 2;
                }
                if (this.MinimizeBox)
                {
                    this.NCRects[8] = new Rectangle(formCaptionPosX, y, width, height);
                    formCaptionPosX -= width + 2;
                }
                if (this.HelpButton)
                {
                    this.NCRects[0x15] = new Rectangle(formCaptionPosX, y, width, height);
                    formCaptionPosX -= width + 2;
                }
            }
            width = this.Width;
            height = this.Height;
            int titleHeight = this.TitleHeight;
            this.NCRects[2] = new Rectangle(0, 0, width, titleHeight);
            this.NCRects[1] = new Rectangle(0, 0, width, height);
            if (this.CanSize)
            {
                this.NCRects[10] = new Rectangle(0, titleHeight, 4, height - titleHeight);
                this.NCRects[11] = new Rectangle(width - 4, titleHeight, 4, height - titleHeight);
                this.NCRects[12] = new Rectangle(15, 0, width - 30, 4);
                this.NCRects[15] = new Rectangle(15, height - 4, width - 30, 4);
                this.NCRects[13] = new Rectangle(0, 0, 15, titleHeight);
                this.NCRects[14] = new Rectangle(width - 15, 0, 15, titleHeight);
                this.NCRects[0x10] = new Rectangle(0, height - 4, 15, 4);
                this.NCRects[0x11] = new Rectangle(width - 15, height - 15, 15, 15);
            }
            if (this.RightToLeftLayout)
            {
                this.NCRects[20].X = (this.Width - this.NCRects[20].X) - 0x10;
                this.NCRects[9].X = (this.Width - this.NCRects[9].X) - 0x10;
                this.NCRects[8].X = (this.Width - this.NCRects[8].X) - 0x10;
                this.NCRects[3].X = (this.Width - this.NCRects[3].X) - 0x10;
                this.NCRects[0x15].X = (this.Width - this.NCRects[0x15].X) - 0x10;
            }
            using (Graphics graphics = Graphics.FromImage(new Bitmap(10, 10)))
            {
                int num2;
                Font titleFont;
                if (this.RightToLeftLayout)
                {
                    if (this.Engine.FormCaptionPosX == -1)
                    {
                        if (this.ControlBox && (this.Icon != null))
                        {
                            num2 = this.NCRects[3].Left - 2;
                        }
                        else
                        {
                            num2 = (width - Math.Max(this.Engine.RightBorderWidth, 4)) - 2;
                        }
                    }
                    else
                    {
                        num2 = width - this.Engine.FormCaptionPosX;
                    }
                    if (this.ControlBox)
                    {
                        formCaptionPosX = this.NCRects[20].Right + 2;
                        if (this.MaximizeBox)
                        {
                            formCaptionPosX = this.NCRects[9].Right + 2;
                        }
                        if (this.MinimizeBox)
                        {
                            formCaptionPosX = this.NCRects[8].Right + 2;
                        }
                        if (this.HelpButton)
                        {
                            formCaptionPosX = this.NCRects[0x15].Right + 2;
                        }
                    }
                    else
                    {
                        formCaptionPosX = Math.Max(this.Engine.LeftBorderWidth, 4) + 0x10;
                    }
                }
                else
                {
                    if (this.Engine.FormCaptionPosX == -1)
                    {
                        if (this.ControlBox && (this.Icon != null))
                        {
                            formCaptionPosX = this.NCRects[3].Right + 2;
                        }
                        else
                        {
                            formCaptionPosX = Math.Max(this.Engine.LeftBorderWidth, 4) + 0x10;
                        }
                    }
                    else
                    {
                        formCaptionPosX = this.Engine.FormCaptionPosX;
                    }
                    if (this.ControlBox)
                    {
                        num2 = this.NCRects[20].Left - 2;
                        if (this.MaximizeBox)
                        {
                            num2 = this.NCRects[9].Left - 2;
                        }
                        if (this.MinimizeBox)
                        {
                            num2 = this.NCRects[8].Left - 2;
                        }
                        if (this.HelpButton)
                        {
                            num2 = this.NCRects[0x15].Left - 2;
                        }
                    }
                    else
                    {
                        num2 = (width - this.Engine.RightBorderWidth) - 2;
                    }
                }
                if (this.Engine.Res.Bools.SKIN2_TITLEFIVESECT)
                {
                    num2 -= this.Engine.Res.Bitmaps.SKIN2_TITLEBAR3.Width;
                }
                if (this.Engine.TitleFont == null)
                {
                    titleFont = x448fd9ab43628c71.TitleFont;
                }
                else
                {
                    titleFont = this.Engine.TitleFont;
                }
                this.xa4b5d355cbf01a80 = x448fd9ab43628c71.FormatStringWithWidth(graphics, this.Text, titleFont, num2 - formCaptionPosX);
                if ((this.RightToLeft & System.Windows.Forms.RightToLeft.Yes) == System.Windows.Forms.RightToLeft.Yes)
                {
                    this.x5a29066520654c5a = num2;
                }
                else
                {
                    this.x5a29066520654c5a = formCaptionPosX;
                }
                this.x6b81454f45088f2e = graphics.MeasureString(this.xa4b5d355cbf01a80, titleFont).ToSize().Width + 10;
            }
        }

        protected void x3b8ca818f0c2637e()
        {
            if ((this.NCRects != null) && this.CanPaint)
            {
                try
                {
                    IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
                    if (windowDC != IntPtr.Zero)
                    {
                        using (Graphics graphics = Graphics.FromHdc(windowDC))
                        {
                            this.x58714ffe12c688d9();
                            this.x3c424b74aaf6d003();
                            this.x9d937faa8065bfbb();
                            if (this.CaptionBuffer != null)
                            {
                                graphics.DrawImageUnscaled(this.CaptionBuffer, 0, 0);
                            }
                            if (this.IsMdiChild && (this.WindowState == FormWindowState.Maximized))
                            {
                                Brush brush = this.Engine.Res.Brushes.SKIN2_FORMCOLOR;
                                graphics.FillRectangle(brush, 0, 0, this.Width, this.TitleHeight);
                            }
                            else
                            {
                                this.xfecf8a78f99f2530(graphics);
                                if (!this.IsMdiChild || (this.WindowState != FormWindowState.Minimized))
                                {
                                    this.PaintBorder(graphics);
                                    if (this.BottomBuffer != null)
                                    {
                                        graphics.DrawImageUnscaled(this.BottomBuffer, 0, this.Height - Math.Max(this.Engine.BottomBorderHeight, this.cyBorder));
                                    }
                                }
                            }
                        }
                        x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
                    }
                }
                catch
                {
                }
            }
        }

        private void x3c424b74aaf6d003()
        {
            if (this.BottomBuffer != null)
            {
                this.BottomBuffer.Dispose();
            }
            int width = this.Width;
            int bottomBorderHeight = this.Engine.BottomBorderHeight;
            int bottom = 0;
            int cyBorder = this.cyBorder;
            this.BottomBuffer = new Bitmap(width, Math.Max(bottomBorderHeight, cyBorder));
            using (Graphics graphics = Graphics.FromImage(this.BottomBuffer))
            {
                Bitmap bitmap;
                Rectangle destRect = new Rectangle(0, 0, this.BottomBuffer.Width, this.BottomBuffer.Height);
                if (bottomBorderHeight < cyBorder)
                {
                    bottom = cyBorder - bottomBorderHeight;
                    graphics.FillRectangle(this.Engine.Res.Brushes.SKIN2_FORMCOLOR, Rectangle.FromLTRB(this.Engine.LeftBorderWidth, 0, width - this.Engine.RightBorderWidth, bottom));
                }
                if (this.Engine.Res.Bools.SKIN2_BOTTOMBORDERTHREESECT)
                {
                    bitmap = this.Engine.Res.Bitmaps.SKIN2_FORMBOTTOMBORDER1;
                    destRect = new Rectangle(0, bottom, bitmap.Width, bottomBorderHeight);
                    Rectangle srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    graphics.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);
                    int right = destRect.Right;
                    bitmap = this.Engine.Res.Bitmaps.SKIN2_FORMBOTTOMBORDER3;
                    destRect = new Rectangle(width - bitmap.Width, bottom, bitmap.Width, bottomBorderHeight);
                    srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    graphics.DrawImage(bitmap, destRect, srcRect, GraphicsUnit.Pixel);
                    int left = destRect.Left;
                    bitmap = this.Engine.Res.Bitmaps.SKIN2_FORMBOTTOMBORDER2;
                    destRect = Rectangle.FromLTRB(right, bottom, left, bottomBorderHeight);
                    srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    graphics.DrawImage(bitmap, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                }
                else
                {
                    bitmap = this.Engine.Res.Bitmaps.SKIN2_FORMBOTTOMBORDER1;
                    using (Bitmap bitmap2 = new Bitmap(width, bitmap.Height))
                    {
                        using (Graphics graphics2 = Graphics.FromImage(bitmap2))
                        {
                            x448fd9ab43628c71.SpitDrawHorizontal(bitmap, graphics2, new Rectangle(0, 0, width, bottomBorderHeight), false);
                        }
                        graphics.DrawImageUnscaled(bitmap2, 0, bottom);
                    }
                }
            }
        }

        internal void x52b190e626f65140(bool xde860fba55c41d76)
        {
            if (xde860fba55c41d76)
            {
                foreach (object obj2 in this.ControlTable.Values)
                {
                    if (obj2 is xbd3f2493841f18a1)
                    {
                        ((xbd3f2493841f18a1) obj2).x52b190e626f65140();
                    }
                }
            }
            this.x230e512478b11e00 = false;
            this.OnCurrentSkinChanged(null, new SkinChangedEventArgs(false));
        }

        private void x58714ffe12c688d9()
        {
            Bitmap bitmap;
            Rectangle rectangle;
            if (this.TitleBuffer != null)
            {
                this.TitleBuffer.Dispose();
            }
            bool flag = this.Engine.Res.Bools.SKIN2_TITLEFIVESECT;
            int width = this.Width;
            int titleHeight = this.TitleHeight;
            bool flag2 = (this.WindowState == FormWindowState.Minimized) && this.IsMdiChild;
            this.TitleBuffer = new Bitmap(width, titleHeight);
            using (Graphics graphics = Graphics.FromImage(this.TitleBuffer))
            {
                Rectangle rectangle2;
                if (flag2)
                {
                    bitmap = this.Engine.Res.Bitmaps.SKIN2_MINIMIZEDTITLE;
                    rectangle = new Rectangle(0, 0, width, titleHeight);
                    rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    graphics.DrawImage(bitmap, rectangle, rectangle2, GraphicsUnit.Pixel);
                }
                else
                {
                    int num4;
                    if (flag)
                    {
                        bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR5;
                    }
                    else
                    {
                        bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR3;
                    }
                    int x = width - bitmap.Width;
                    rectangle = new Rectangle(x, 0, bitmap.Width, titleHeight);
                    rectangle2 = new Rectangle(0, 0, bitmap.Width, titleHeight);
                    graphics.DrawImage(bitmap, rectangle, rectangle2, GraphicsUnit.Pixel);
                    if (flag)
                    {
                        if (x > (this.Engine.Res.Bitmaps.SKIN2_TITLEBAR1.Width + this.Engine.Res.Bitmaps.SKIN2_TITLEBAR3.Width))
                        {
                            bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR1;
                            rectangle = new Rectangle(0, 0, bitmap.Width, titleHeight);
                            rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            graphics.DrawImage(bitmap, rectangle, rectangle2, GraphicsUnit.Pixel);
                            num4 = bitmap.Width;
                            bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR2;
                            rectangle = new Rectangle(num4, 0, this.x6b81454f45088f2e, titleHeight);
                            rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            graphics.DrawImage(bitmap, rectangle, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                            bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR3;
                            rectangle = new Rectangle(rectangle.Right, 0, bitmap.Width, titleHeight);
                            rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            graphics.DrawImage(bitmap, rectangle, rectangle2, GraphicsUnit.Pixel);
                            bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR4;
                            rectangle = Rectangle.FromLTRB(rectangle.Right, 0, x, titleHeight);
                            rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            if (rectangle.Width > 0)
                            {
                                graphics.DrawImage(bitmap, rectangle, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                            }
                            goto Label_0638;
                        }
                        if (x <= 0)
                        {
                            goto Label_0638;
                        }
                        using (Bitmap bitmap2 = new Bitmap(this.Engine.Res.Bitmaps.SKIN2_TITLEBAR1.Width + this.Engine.Res.Bitmaps.SKIN2_TITLEBAR3.Width, titleHeight))
                        {
                            using (Graphics graphics2 = Graphics.FromImage(bitmap2))
                            {
                                bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR1;
                                rectangle = new Rectangle(0, 0, bitmap.Width, titleHeight);
                                rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                                graphics2.DrawImage(bitmap, rectangle, rectangle2, GraphicsUnit.Pixel);
                                num4 = bitmap.Width;
                                bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR3;
                                rectangle = new Rectangle(rectangle.Right, 0, bitmap.Width, titleHeight);
                                rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                                graphics2.DrawImage(bitmap, rectangle, rectangle2, GraphicsUnit.Pixel);
                            }
                            rectangle2 = new Rectangle(0, 0, bitmap2.Width, titleHeight);
                            rectangle = new Rectangle(0, 0, x, titleHeight);
                            graphics.DrawImage(bitmap2, rectangle, rectangle2, GraphicsUnit.Pixel);
                            goto Label_0638;
                        }
                    }
                    if (x > this.Engine.Res.Bitmaps.SKIN2_TITLEBAR1.Width)
                    {
                        bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR1;
                        rectangle = new Rectangle(0, 0, bitmap.Width, titleHeight);
                        rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        graphics.DrawImage(bitmap, rectangle, rectangle2, GraphicsUnit.Pixel);
                        num4 = bitmap.Width;
                        bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR2;
                        if (x > num4)
                        {
                            rectangle = Rectangle.FromLTRB(num4, 0, x, titleHeight);
                            rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            graphics.DrawImage(bitmap, rectangle, rectangle2.X, rectangle2.Y, rectangle2.Width, rectangle2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY, null);
                        }
                        if (bitmap.Height < titleHeight)
                        {
                            Brush brush = this.Engine.Res.Brushes.SKIN2_FORMCOLOR;
                            graphics.FillRectangle(brush, 0, bitmap.Height, width, titleHeight - bitmap.Height);
                        }
                    }
                    else if (x > 0)
                    {
                        using (Bitmap bitmap3 = new Bitmap(this.Engine.Res.Bitmaps.SKIN2_TITLEBAR1.Width, titleHeight))
                        {
                            using (Graphics graphics3 = Graphics.FromImage(bitmap3))
                            {
                                bitmap = this.Engine.Res.Bitmaps.SKIN2_TITLEBAR1;
                                rectangle = new Rectangle(0, 0, bitmap.Width, titleHeight);
                                rectangle2 = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                                graphics3.DrawImage(bitmap, rectangle, rectangle2, GraphicsUnit.Pixel);
                            }
                            rectangle2 = new Rectangle(0, 0, bitmap3.Width, titleHeight);
                            rectangle = new Rectangle(0, 0, x, titleHeight);
                            graphics.DrawImage(bitmap3, rectangle, rectangle2, GraphicsUnit.Pixel);
                        }
                    }
                }
            }
        Label_0638:
            if (this.RightToLeftLayout)
            {
                this.TitleBuffer.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            using (Graphics graphics4 = Graphics.FromImage(this.TitleBuffer))
            {
                if ((this.ControlBox && (this.Icon != null)) && this.Engine.DrawFormIcon)
                {
                    if (this.NCRects == null)
                    {
                        this.x271f237e4704798d();
                    }
                    rectangle = this.NCRects[3];
                    using (bitmap = this.Icon.ToBitmap())
                    {
                        if ((bitmap.Width != 0x10) || (bitmap.Height != 0x10))
                        {
                            using (Image image = bitmap.GetThumbnailImage(0x10, 0x10, null, IntPtr.Zero))
                            {
                                graphics4.DrawImage(image, rectangle, new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                                return;
                            }
                        }
                        graphics4.DrawImageUnscaled(bitmap, rectangle);
                    }
                }
            }
        }

        private void x5a3bdd19fa669e36(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.DoClose();
        }

        private void x6616b01fd7cbc65d(int x30cc7819189f11b6)
        {
            int num;
            if (this.NCRects == null)
            {
                return;
            }
            if (!this.CanPaint)
            {
                return;
            }
            int index = -1;
            switch (this.LastMousePosition)
            {
                case 8:
                    if ((this.WindowState != FormWindowState.Minimized) || !this.IsMdiChild)
                    {
                        if (x30cc7819189f11b6 == 8)
                        {
                            if (this.IsNCMouseDown == 8)
                            {
                                num = 15;
                            }
                            else
                            {
                                num = 14;
                            }
                        }
                        else
                        {
                            num = 13;
                        }
                    }
                    else if (x30cc7819189f11b6 != 8)
                    {
                        num = 10;
                    }
                    else if (this.IsNCMouseDown != 8)
                    {
                        num = 11;
                    }
                    else
                    {
                        num = 12;
                    }
                    index = 8;
                    goto Label_01EE;

                case 9:
                    if (x30cc7819189f11b6 != 9)
                    {
                        if (this.WindowState == FormWindowState.Maximized)
                        {
                            num = 10;
                        }
                        else
                        {
                            num = 7;
                        }
                    }
                    else if (this.WindowState != FormWindowState.Maximized)
                    {
                        if (this.IsNCMouseDown == 9)
                        {
                            num = 9;
                        }
                        else
                        {
                            num = 8;
                        }
                    }
                    else if (this.IsNCMouseDown != 9)
                    {
                        num = 11;
                    }
                    else
                    {
                        num = 12;
                    }
                    index = 9;
                    goto Label_01EE;

                case 20:
                    if (x30cc7819189f11b6 != 20)
                    {
                        num = 0x10;
                        break;
                    }
                    if (this.IsNCMouseDown != 20)
                    {
                        num = 0x11;
                        break;
                    }
                    num = 0x12;
                    break;

                case 0x15:
                    if (x30cc7819189f11b6 != 0x15)
                    {
                        num = 4;
                    }
                    else if (this.IsNCMouseDown != 0x15)
                    {
                        num = 5;
                    }
                    else
                    {
                        num = 6;
                    }
                    index = 0x15;
                    goto Label_01EE;

                default:
                    switch (x30cc7819189f11b6)
                    {
                        case 8:
                            if ((this.WindowState != FormWindowState.Minimized) || !this.IsMdiChild)
                            {
                                if (this.IsNCMouseDown == 8)
                                {
                                    num = 15;
                                }
                                else
                                {
                                    num = 14;
                                }
                            }
                            else if (this.IsNCMouseDown != 8)
                            {
                                num = 11;
                            }
                            else
                            {
                                num = 12;
                            }
                            index = 8;
                            goto Label_01EE;

                        case 9:
                            if (this.WindowState != FormWindowState.Maximized)
                            {
                                if (this.IsNCMouseDown == 9)
                                {
                                    num = 9;
                                }
                                else
                                {
                                    num = 8;
                                }
                            }
                            else if (this.IsNCMouseDown != 9)
                            {
                                num = 11;
                            }
                            else
                            {
                                num = 12;
                            }
                            index = 9;
                            goto Label_01EE;

                        case 20:
                            if (this.IsNCMouseDown != 20)
                            {
                                num = 0x11;
                            }
                            else
                            {
                                num = 0x12;
                            }
                            index = 20;
                            goto Label_01EE;

                        case 0x15:
                            if (this.IsNCMouseDown != 0x15)
                            {
                                num = 5;
                            }
                            else
                            {
                                num = 6;
                            }
                            index = 0x15;
                            goto Label_01EE;
                    }
                    num = 1;
                    index = -1;
                    goto Label_01EE;
            }
            index = 20;
        Label_01EE:
            if (index == -1)
            {
                return;
            }
            Bitmap image = this.Engine.Res.SplitBitmaps.SKIN2_TITLEBUTTONS[num];
            Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
            IntPtr windowDC = x61467fe65a98f20c.GetWindowDC(base.Handle);
            if (windowDC == IntPtr.Zero)
            {
                return;
            }
            using (Graphics graphics = Graphics.FromHdc(windowDC))
            {
                Rectangle rectangle;
                switch (index)
                {
                    case 8:
                        rectangle = this.NCRects[index];
                        graphics.DrawImage(this.ButtonBuffer[1], rectangle, srcRect, GraphicsUnit.Pixel);
                        graphics.DrawImage(image, rectangle, srcRect, GraphicsUnit.Pixel);
                        goto Label_034A;

                    case 9:
                        rectangle = this.NCRects[index];
                        graphics.DrawImage(this.ButtonBuffer[2], rectangle, srcRect, GraphicsUnit.Pixel);
                        graphics.DrawImage(image, rectangle, srcRect, GraphicsUnit.Pixel);
                        goto Label_034A;

                    case 20:
                        rectangle = this.NCRects[index];
                        graphics.DrawImage(this.ButtonBuffer[3], rectangle, srcRect, GraphicsUnit.Pixel);
                        graphics.DrawImage(image, rectangle, srcRect, GraphicsUnit.Pixel);
                        goto Label_034A;

                    case 0x15:
                        rectangle = this.NCRects[index];
                        graphics.DrawImage(this.ButtonBuffer[0], rectangle, srcRect, GraphicsUnit.Pixel);
                        graphics.DrawImage(image, rectangle, srcRect, GraphicsUnit.Pixel);
                        goto Label_034A;
                }
            }
        Label_034A:
            x61467fe65a98f20c.ReleaseDC(base.Handle, windowDC);
        }

        private void x814eab16f4e2a468(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.DoMin();
        }

        private void x841c19853f4810d4(object xe0292b9ed559da7d, EventArgs xfbf34718e704c6bc)
        {
            this.DoMax();
        }

        private void x9d937faa8065bfbb()
        {
            if (this.CaptionBuffer != null)
            {
                this.CaptionBuffer.Dispose();
                this.CaptionBuffer = null;
            }
            if ((this.TitleBuffer != null) && (this.NCRects != null))
            {
                int formCaptionPosY;
                int width = this.TitleBuffer.Width;
                int height = this.TitleBuffer.Height;
                this.CaptionBuffer = new Bitmap(width, height);
                if (this.Engine.FormCaptionPosY == -1)
                {
                    formCaptionPosY = this.x0c7b47d7852a0e99();
                }
                else
                {
                    formCaptionPosY = this.Engine.FormCaptionPosY;
                }
                using (Graphics graphics = Graphics.FromImage(this.CaptionBuffer))
                {
                    Font titleFont;
                    Color color;
                    graphics.DrawImageUnscaled(this.TitleBuffer, 0, 0);
                    if (this.Engine.TitleFont == null)
                    {
                        titleFont = x448fd9ab43628c71.TitleFont;
                    }
                    else
                    {
                        titleFont = this.Engine.TitleFont;
                    }
                    StringFormat format = new StringFormat {
                        LineAlignment = StringAlignment.Near
                    };
                    if ((this.RightToLeft & System.Windows.Forms.RightToLeft.Yes) == System.Windows.Forms.RightToLeft.Yes)
                    {
                        format.Alignment = StringAlignment.Far;
                    }
                    else
                    {
                        format.Alignment = StringAlignment.Near;
                    }
                    if (this.IsActive)
                    {
                        color = this.Engine.Res.Colors.SKIN2_TITLEFONTCOLOR;
                    }
                    else
                    {
                        color = Color.FromKnownColor(KnownColor.InactiveCaptionText);
                    }
                    if (this.Engine.DrawFormCaption)
                    {
                        using (Brush brush = new SolidBrush(color))
                        {
                            if ((this.RightToLeft & System.Windows.Forms.RightToLeft.Yes) == System.Windows.Forms.RightToLeft.Yes)
                            {
                                graphics.DrawString(this.xa4b5d355cbf01a80, titleFont, brush, Rectangle.FromLTRB((this.x5a29066520654c5a - this.x6b81454f45088f2e) + 4, formCaptionPosY, this.x5a29066520654c5a, this.TitleHeight), format);
                            }
                            else
                            {
                                graphics.DrawString(this.xa4b5d355cbf01a80, titleFont, brush, Rectangle.FromLTRB(this.x5a29066520654c5a, formCaptionPosY, this.x5a29066520654c5a + this.x6b81454f45088f2e, this.TitleHeight), format);
                            }
                        }
                    }
                }
                if (this.ControlBox)
                {
                    Rectangle rectangle;
                    if (this.ButtonBuffer != null)
                    {
                        foreach (Bitmap bitmap in this.ButtonBuffer)
                        {
                            if (bitmap != null)
                            {
                                bitmap.Dispose();
                            }
                        }
                    }
                    this.ButtonBuffer = new Bitmap[4];
                    if (this.HelpButton)
                    {
                        this.ButtonBuffer[0] = new Bitmap(this.NCRects[0x15].Width, this.NCRects[0x15].Height);
                        using (Graphics graphics2 = Graphics.FromImage(this.ButtonBuffer[0]))
                        {
                            rectangle = new Rectangle(0, 0, this.ButtonBuffer[0].Width, this.ButtonBuffer[0].Height);
                            graphics2.DrawImage(this.TitleBuffer, rectangle, this.NCRects[0x15], GraphicsUnit.Pixel);
                        }
                    }
                    if (this.MinimizeBox)
                    {
                        this.ButtonBuffer[1] = new Bitmap(this.NCRects[8].Width, this.NCRects[8].Height);
                        using (Graphics graphics3 = Graphics.FromImage(this.ButtonBuffer[1]))
                        {
                            rectangle = new Rectangle(0, 0, this.ButtonBuffer[1].Width, this.ButtonBuffer[1].Height);
                            graphics3.DrawImage(this.TitleBuffer, rectangle, this.NCRects[8], GraphicsUnit.Pixel);
                        }
                    }
                    if (this.MaximizeBox)
                    {
                        this.ButtonBuffer[2] = new Bitmap(this.NCRects[9].Width, this.NCRects[9].Height);
                        using (Graphics graphics4 = Graphics.FromImage(this.ButtonBuffer[2]))
                        {
                            rectangle = new Rectangle(0, 0, this.ButtonBuffer[2].Width, this.ButtonBuffer[2].Height);
                            graphics4.DrawImage(this.TitleBuffer, rectangle, this.NCRects[9], GraphicsUnit.Pixel);
                        }
                    }
                    this.ButtonBuffer[3] = new Bitmap(this.NCRects[20].Width, this.NCRects[20].Height);
                    using (Graphics graphics5 = Graphics.FromImage(this.ButtonBuffer[3]))
                    {
                        rectangle = new Rectangle(0, 0, this.ButtonBuffer[3].Width, this.ButtonBuffer[3].Height);
                        graphics5.DrawImage(this.TitleBuffer, rectangle, this.NCRects[20], GraphicsUnit.Pixel);
                    }
                }
            }
        }

        protected void xd308f013f560f5be(Point x9c79b5ad7b769b12)
        {
            this.xd1faceba1dee38cf.MenuCommands[0].Enabled = this.MinimizeBox;
            this.xd1faceba1dee38cf.MenuCommands[1].Enabled = this.MaximizeBox;
            this.xd1faceba1dee38cf.TrackPopupTaskBar(x9c79b5ad7b769b12);
        }

        private void xfecf8a78f99f2530(Graphics x4b101060f4767186)
        {
            if (this.ControlBox)
            {
                int index = 0x10;
                Bitmap image = this.Engine.Res.SplitBitmaps.SKIN2_TITLEBUTTONS[index];
                x4b101060f4767186.DrawImage(image, this.NCRects[20], 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                if (this.MaximizeBox)
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        index = 10;
                    }
                    else
                    {
                        index = 7;
                    }
                    image = this.Engine.Res.SplitBitmaps.SKIN2_TITLEBUTTONS[index];
                    x4b101060f4767186.DrawImage(image, this.NCRects[9], 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }
                if (this.MinimizeBox)
                {
                    if ((this.WindowState == FormWindowState.Minimized) && this.IsMdiChild)
                    {
                        index = 10;
                    }
                    else
                    {
                        index = 13;
                    }
                    image = this.Engine.Res.SplitBitmaps.SKIN2_TITLEBUTTONS[index];
                    x4b101060f4767186.DrawImage(image, this.NCRects[8], 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }
                if (this.HelpButton)
                {
                    index = 4;
                    image = this.Engine.Res.SplitBitmaps.SKIN2_TITLEBUTTONS[index];
                    x4b101060f4767186.DrawImage(image, this.NCRects[0x15], 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }
            }
        }

        protected abstract FormBorderStyle BorderStyle { get; }

        protected virtual bool CanPaint
        {
            get
            {
                if ((!this.Engine.RealActive || !this.x230e512478b11e00) || (this.IsDisposed || (this.BorderStyle == FormBorderStyle.None)))
                {
                    return false;
                }
                if (this.Tag is int)
                {
                    if (((int) this.Tag) == this.Engine.DisableTag)
                    {
                        return false;
                    }
                }
                else if ((this.Tag is string) && (((string) this.Tag) == this.Engine.DisableTag.ToString()))
                {
                    return false;
                }
                return true;
            }
        }

        protected virtual bool CanPaintChild
        {
            get
            {
                if (!this.Engine.RealActive || this.IsDisposed)
                {
                    return false;
                }
                if (this.Tag is int)
                {
                    if (((int) this.Tag) == this.Engine.DisableTag)
                    {
                        return false;
                    }
                }
                else if ((this.Tag is string) && (((string) this.Tag) == this.Engine.DisableTag.ToString()))
                {
                    return false;
                }
                return this.Engine.RealActive;
            }
        }

        protected virtual bool CanSize
        {
            get
            {
                switch (this.BorderStyle)
                {
                    case FormBorderStyle.None:
                    case FormBorderStyle.FixedSingle:
                    case FormBorderStyle.Fixed3D:
                    case FormBorderStyle.FixedDialog:
                    case FormBorderStyle.FixedToolWindow:
                        return false;
                }
                switch (this.WindowState)
                {
                    case FormWindowState.Minimized:
                    case FormWindowState.Maximized:
                        return false;
                }
                return true;
            }
        }

        protected abstract Rectangle ClientRectangle { get; }

        protected abstract bool ControlBox { get; }

        protected abstract int Height { get; }

        protected abstract bool HelpButton { get; }

        protected abstract System.Drawing.Icon Icon { get; }

        protected virtual bool IsActive
        {
            get
            {
                return this.xfd408d62a1acef9b;
            }
            set
            {
                this.xfd408d62a1acef9b = value;
            }
        }

        protected abstract bool IsDisposed { get; }

        protected abstract bool IsMdiChild { get; }

        protected abstract bool IsMdiContainer { get; }

        protected abstract bool MaximizeBox { get; }

        protected virtual int MenuHeight
        {
            get
            {
                return 0;
            }
        }

        protected abstract bool MinimizeBox { get; }

        protected abstract System.Drawing.Region Region { get; set; }

        protected abstract System.Windows.Forms.RightToLeft RightToLeft { get; }

        protected abstract bool RightToLeftLayout { get; }

        protected abstract object Tag { get; }

        protected abstract string Text { get; }

        protected virtual int TitleHeight
        {
            get
            {
                return this.Engine.TitleHeight;
            }
        }

        protected abstract int Width { get; }

        protected virtual FormWindowState WindowState
        {
            get
            {
                if (x61467fe65a98f20c.IsIconic(base.Handle))
                {
                    return FormWindowState.Minimized;
                }
                if (x61467fe65a98f20c.IsZoomed(base.Handle))
                {
                    return FormWindowState.Maximized;
                }
                return FormWindowState.Normal;
            }
            set
            {
                switch (value)
                {
                    case FormWindowState.Normal:
                        x61467fe65a98f20c.PostMessage(base.Handle, 0x112, (uint) 0xf120, (uint) 0);
                        return;

                    case FormWindowState.Minimized:
                        x61467fe65a98f20c.PostMessage(base.Handle, 0x112, (uint) 0xf020, (uint) 0);
                        return;

                    case FormWindowState.Maximized:
                        x61467fe65a98f20c.PostMessage(base.Handle, 0x112, (uint) 0xf030, (uint) 0);
                        return;
                }
            }
        }

        internal x902c4aee45bfd906 xd1faceba1dee38cf
        {
            get
            {
                if (this.xcdc1a69e212e5aa9 == null)
                {
                    this.xcdc1a69e212e5aa9 = new x902c4aee45bfd906(this.Engine);
                    this.xcdc1a69e212e5aa9.MenuCommands.Add(new x5f4b657f68f87baa(this.Engine.ResSysMenuMin, new EventHandler(this.x814eab16f4e2a468)));
                    this.xcdc1a69e212e5aa9.MenuCommands.Add(new x5f4b657f68f87baa(this.Engine.ResSysMenuMax, new EventHandler(this.x841c19853f4810d4)));
                    this.xcdc1a69e212e5aa9.MenuCommands.Add(new x5f4b657f68f87baa(this.Engine.ResSysMenuClose, new EventHandler(this.x5a3bdd19fa669e36)));
                }
                this.xcdc1a69e212e5aa9.MenuCommands[0].Text = this.Engine.ResSysMenuMin;
                this.xcdc1a69e212e5aa9.MenuCommands[1].Text = this.Engine.ResSysMenuMax;
                this.xcdc1a69e212e5aa9.MenuCommands[2].Text = this.Engine.ResSysMenuClose;
                this.xcdc1a69e212e5aa9.x94975a4c4f1d71c4 = this.RightToLeft;
                return this.xcdc1a69e212e5aa9;
            }
        }
    }
}

