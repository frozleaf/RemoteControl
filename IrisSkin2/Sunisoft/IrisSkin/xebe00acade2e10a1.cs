namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    internal class xebe00acade2e10a1 : ToolStripRenderer
    {
        private ToolStripProfessionalRenderer x0f4884030a491f49 = new ToolStripProfessionalRenderer();
        private ImageAttributes x233f092c536593eb = new ImageAttributes();
        private ToolStripSystemRenderer x4b7855aec919b975 = new ToolStripSystemRenderer();
        private SkinEngine xdc87e2b99332cd4a;

        public xebe00acade2e10a1(SkinEngine engine)
        {
            this.xdc87e2b99332cd4a = engine;
            this.x233f092c536593eb.SetWrapMode(WrapMode.Tile);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            this.x0f4884030a491f49.DrawArrow(e);
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            this.x11d38b1ae9f5b99b(e);
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            this.x11d38b1ae9f5b99b(e);
            if (e.Item is ToolStripDropDownButton)
            {
                ToolStripDropDownButton item = (ToolStripDropDownButton) e.Item;
                if (item.ShowDropDownArrow)
                {
                    Brush brush;
                    Rectangle rect = new Rectangle(e.Item.ContentRectangle.Right, e.Item.ContentRectangle.Top, e.Item.Width - e.Item.ContentRectangle.Width, e.Item.Height);
                    rect.X++;
                    rect.Width -= 4;
                    rect.Y -= 2;
                    rect.Height--;
                    if (e.Item.Selected)
                    {
                        brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARONCOLOR;
                        e.Graphics.FillRectangle(brush, rect);
                        using (Pen pen = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARBORDERCOLOR))
                        {
                            e.Graphics.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                            e.Graphics.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
                            e.Graphics.DrawLine(pen, rect.Right, rect.Top, rect.Right, rect.Bottom);
                        }
                    }
                    if (e.Item.Pressed)
                    {
                        brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARONCOLOR;
                        e.Graphics.FillRectangle(brush, rect);
                        using (Pen pen2 = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARBORDERCOLOR))
                        {
                            e.Graphics.DrawLine(pen2, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                            e.Graphics.DrawLine(pen2, rect.Left, rect.Top, rect.Right, rect.Top);
                            e.Graphics.DrawLine(pen2, rect.Right, rect.Top, rect.Right, rect.Bottom);
                        }
                    }
                    if (!e.Item.Selected && !e.Item.Pressed)
                    {
                        this.xf2f6451a5d77af97(item, e);
                    }
                }
            }
        }

        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            this.x0f4884030a491f49.DrawGrip(e);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            this.x0f4884030a491f49.DrawItemCheck(e);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item is ToolStripMenuItem)
            {
                ToolStripMenuItem item = (ToolStripMenuItem) e.Item;
                if (item.Owner is MenuStrip)
                {
                    if (item.Pressed || item.Selected)
                    {
                        e.TextColor = this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOPSELECTEDMENUFONTCOLOR;
                    }
                    else
                    {
                        e.TextColor = this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOPMENUFONTCOLOR;
                    }
                }
                else if (item.Pressed || item.Selected)
                {
                    e.TextColor = this.xdc87e2b99332cd4a.Res.Colors.SKIN2_SELECTEDMENUFONTCOLOR;
                }
                else
                {
                    e.TextColor = this.xdc87e2b99332cd4a.Res.Colors.SKIN2_MENUITEMFONTCOLOR;
                }
                this.x4b7855aec919b975.DrawItemText(e);
            }
            else
            {
                this.x0f4884030a491f49.DrawItemText(e);
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Brush brush;
            if ((((e.Item.OwnerItem == null) && (e.ToolStrip != null)) && ((e.ToolStrip.BackgroundImage != null) && !e.Item.Pressed)) && !e.Item.Selected)
            {
                base.OnRenderMenuItemBackground(e);
                if (e.Item.BackgroundImage == null)
                {
                    return;
                }
            }
            if (!(e.Item is ToolStripMenuItem))
            {
                this.x0f4884030a491f49.DrawMenuItemBackground(e);
                return;
            }
            ToolStripMenuItem item = (ToolStripMenuItem) e.Item;
            Rectangle rect = new Rectangle(0, 0, item.Width, item.Height);
            if (item.Owner is MenuStrip)
            {
                rect.X += 2;
                rect.Width -= 3;
                if (!item.Pressed && !item.Selected)
                {
                    goto Label_032B;
                }
                brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOPSELECTEDMENUCOLOR;
                e.Graphics.FillRectangle(brush, rect);
                using (Pen pen = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOPSELECTEDMENUBORDERCOLOR))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(2, 0, rect.Width - 1, rect.Height - 1));
                    goto Label_032B;
                }
            }
            rect.X += 2;
            rect.Width -= 3;
            if (item.Pressed || item.Selected)
            {
                brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_SELECTEDMENUCOLOR;
            }
            else
            {
                brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_MENUITEMCOLOR;
            }
            e.Graphics.FillRectangle(brush, rect);
            if (item.Selected && item.Enabled)
            {
                brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_SELECTEDMENUCOLOR;
                e.Graphics.FillRectangle(brush, rect.X, rect.Y, 0x1a, rect.Height);
                using (Pen pen2 = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_SELECTEDMENUBORDERCOLOR))
                {
                    e.Graphics.DrawRectangle(pen2, new Rectangle(2, 0, rect.Width - 1, rect.Height - 1));
                    goto Label_032B;
                }
            }
            if (this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARSTARTCOLOR != this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARENDCOLOR)
            {
                using (LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(rect.X, rect.Y, 0x1a, rect.Height), this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARSTARTCOLOR, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARENDCOLOR, LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush2, rect.X, rect.Y, 0x1a, rect.Height);
                    goto Label_032B;
                }
            }
            e.Graphics.FillRectangle(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR, rect.X, rect.Y, 0x1a, rect.Height);
        Label_032B:
            if (!item.Selected && !item.Pressed)
            {
                this.xf2f6451a5d77af97(item, e);
            }
        }

        protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item is ToolStripOverflowButton)
            {
                Rectangle rectangle;
                Brush brush;
                ToolStripOverflowButton item = (ToolStripOverflowButton) e.Item;
                if (e.ToolStrip.Orientation == Orientation.Horizontal)
                {
                    rectangle = Rectangle.FromLTRB(item.Size.Width - 10, 0, item.Size.Width, item.Size.Height);
                }
                else
                {
                    rectangle = Rectangle.FromLTRB(0, item.Size.Height - 10, item.Size.Width, item.Size.Height);
                }
                using (brush = new SolidBrush(this.xdc87e2b99332cd4a.Res.Colors.SKIN2_MENUITEMCOLOR))
                {
                    e.Graphics.FillRectangle(brush, rectangle);
                }
                if (item.Selected)
                {
                    brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARONCOLOR;
                    e.Graphics.FillRectangle(brush, rectangle);
                }
                if (item.Pressed)
                {
                    brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR;
                    e.Graphics.FillRectangle(brush, rectangle);
                }
                if (!e.Item.Selected && !e.Item.Pressed)
                {
                    this.xf2f6451a5d77af97(e.Item, e);
                }
                rectangle = Rectangle.FromLTRB(item.Size.Width - 10, item.Size.Height - 10, item.Size.Width, item.Size.Height);
                if (e.ToolStrip.Orientation == Orientation.Horizontal)
                {
                    e.Graphics.DrawLine(Pens.Black, rectangle.Left + 3, rectangle.Top, rectangle.Right - 3, rectangle.Top);
                    ToolStripArrowRenderEventArgs args = new ToolStripArrowRenderEventArgs(e.Graphics, item, rectangle, Color.Black, ArrowDirection.Down);
                    this.x0f4884030a491f49.DrawArrow(args);
                }
                else
                {
                    e.Graphics.DrawLine(Pens.Black, rectangle.Left, rectangle.Top + 3, rectangle.Left, rectangle.Bottom - 3);
                    ToolStripArrowRenderEventArgs args2 = new ToolStripArrowRenderEventArgs(e.Graphics, item, rectangle, Color.Black, ArrowDirection.Right);
                    this.x0f4884030a491f49.DrawArrow(args2);
                }
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if (!(e.Item is ToolStripSeparator))
            {
                this.x0f4884030a491f49.DrawSeparator(e);
                return;
            }
            ToolStripSeparator item = (ToolStripSeparator) e.Item;
            if (!(item.Owner is ToolStripDropDownMenu))
            {
                goto Label_01AB;
            }
            Rectangle rect = new Rectangle(0, 0, item.Width, item.Height);
            Brush brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_MENUITEMCOLOR;
            e.Graphics.FillRectangle(brush, rect);
            if (this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARSTARTCOLOR != this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARENDCOLOR)
            {
                using (LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(rect.X, rect.Y, 0x1a, rect.Height), this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARSTARTCOLOR, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARENDCOLOR, LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush2, rect.X, rect.Y, 0x1a, rect.Height);
                    goto Label_014B;
                }
            }
            e.Graphics.FillRectangle(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR, rect.X, rect.Y, 0x1a, rect.Height);
        Label_014B:
            using (Pen pen = new Pen(Color.FromArgb(0x84, 130, 0x84), 1f))
            {
                e.Graphics.DrawLine(pen, (rect.Left + 0x1a) + 4, rect.Top + 2, rect.Right, rect.Top + 2);
                return;
            }
        Label_01AB:
            this.x0f4884030a491f49.DrawSeparator(e);
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            this.x11d38b1ae9f5b99b(e);
            Rectangle arrowRectangle = Rectangle.FromLTRB(e.Item.ContentRectangle.Right - 10, e.Item.ContentRectangle.Top, e.Item.ContentRectangle.Right, e.Item.ContentRectangle.Bottom);
            if (e.Item.Selected && !e.Item.Pressed)
            {
                using (Pen pen = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARBORDERCOLOR))
                {
                    e.Graphics.DrawLine(pen, arrowRectangle.Left, arrowRectangle.Top - 1, arrowRectangle.Left, arrowRectangle.Bottom);
                }
            }
            if (!e.Item.Selected && !e.Item.Pressed)
            {
                this.xf2f6451a5d77af97(e.Item, e);
            }
            ToolStripArrowRenderEventArgs args = new ToolStripArrowRenderEventArgs(e.Graphics, e.Item, arrowRectangle, Color.Black, ArrowDirection.Down);
            this.x0f4884030a491f49.DrawArrow(args);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.BackgroundImage != null)
            {
                this.xa8c2e639f6764635(e.ToolStrip.BackgroundImage, e);
            }
            else
            {
                Brush brush;
                if (e.ToolStrip is MenuStrip)
                {
                    Bitmap image = this.xdc87e2b99332cd4a.Res.Bitmaps.SKIN2_MENUBAR;
                    if (image != null)
                    {
                        e.Graphics.DrawImage(image, e.AffectedBounds, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                    }
                    else
                    {
                        if (this.xdc87e2b99332cd4a.Res.Colors.SKIN2_MENUBARSTARTCOLOR != this.xdc87e2b99332cd4a.Res.Colors.SKIN2_MENUBARENDCOLOR)
                        {
                            using (brush = new LinearGradientBrush(e.AffectedBounds, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_MENUBARSTARTCOLOR, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_MENUBARENDCOLOR, LinearGradientMode.Vertical))
                            {
                                e.Graphics.FillRectangle(brush, e.AffectedBounds);
                                return;
                            }
                        }
                        e.Graphics.FillRectangle(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_MENUBARSTARTCOLOR, e.AffectedBounds);
                    }
                }
                else
                {
                    if (e.ToolStrip is StatusStrip)
                    {
                        using (Pen pen = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_CONTROLBORDERCOLOR, 1f))
                        {
                            e.Graphics.DrawLine(pen, e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Right, e.AffectedBounds.Y);
                            return;
                        }
                    }
                    if (e.ToolStrip is ToolStripDropDownMenu)
                    {
                        Rectangle affectedBounds = e.AffectedBounds;
                        affectedBounds.X += 2;
                        affectedBounds.Width -= 4;
                        affectedBounds.Y += 2;
                        affectedBounds.Height -= 4;
                        brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_MENUITEMCOLOR;
                        e.Graphics.FillRectangle(brush, affectedBounds);
                        if (this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARSTARTCOLOR != this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARENDCOLOR)
                        {
                            using (LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(affectedBounds.X, affectedBounds.Y, 0x1a, affectedBounds.Height), this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARSTARTCOLOR, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_LEFTBARENDCOLOR, LinearGradientMode.Horizontal))
                            {
                                e.Graphics.FillRectangle(brush2, affectedBounds.X, affectedBounds.Y, 0x1a, affectedBounds.Height);
                                return;
                            }
                        }
                        e.Graphics.FillRectangle(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR, affectedBounds.X, affectedBounds.Y, 0x1a, affectedBounds.Height);
                    }
                    else
                    {
                        Bitmap bitmap2;
                        if (e.ToolStrip.LayoutStyle == ToolStripLayoutStyle.HorizontalStackWithOverflow)
                        {
                            bitmap2 = this.xdc87e2b99332cd4a.Res.Bitmaps.SKIN2_TOOLBAR;
                            if (bitmap2 != null)
                            {
                                using (Bitmap bitmap3 = (Bitmap) bitmap2.Clone())
                                {
                                    bitmap3.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                    e.Graphics.DrawImage(bitmap3, e.AffectedBounds, 0, 0, bitmap2.Width, bitmap2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                                    return;
                                }
                            }
                            if (this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOOLBARSTARTCOLOR != this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOOLBARENDCOLOR)
                            {
                                using (brush = new LinearGradientBrush(e.AffectedBounds, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOOLBARSTARTCOLOR, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOOLBARENDCOLOR, LinearGradientMode.Vertical))
                                {
                                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
                                    return;
                                }
                            }
                            e.Graphics.FillRectangle(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARSTARTCOLOR, e.AffectedBounds);
                        }
                        else
                        {
                            bitmap2 = this.xdc87e2b99332cd4a.Res.Bitmaps.SKIN2_TOOLBAR;
                            if (bitmap2 != null)
                            {
                                e.Graphics.DrawImage(bitmap2, e.AffectedBounds, 0, 0, bitmap2.Width, bitmap2.Height, GraphicsUnit.Pixel, x448fd9ab43628c71.DrawImageAttrTileY);
                            }
                            else
                            {
                                if (this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOOLBARSTARTCOLOR != this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOOLBARENDCOLOR)
                                {
                                    using (brush = new LinearGradientBrush(e.AffectedBounds, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOOLBARSTARTCOLOR, this.xdc87e2b99332cd4a.Res.Colors.SKIN2_TOOLBARENDCOLOR, LinearGradientMode.Horizontal))
                                    {
                                        e.Graphics.FillRectangle(brush, e.AffectedBounds);
                                        return;
                                    }
                                }
                                e.Graphics.FillRectangle(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARSTARTCOLOR, e.AffectedBounds);
                            }
                        }
                    }
                }
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (!(e.ToolStrip is MenuStrip) && !(e.ToolStrip is StatusStrip))
            {
                if (e.ToolStrip is ToolStripDropDownMenu)
                {
                    using (Pen pen = new Pen(this.xdc87e2b99332cd4a.Res.Colors.SKIN2_SELECTEDMENUBORDERCOLOR))
                    {
                        e.Graphics.DrawRectangle(pen, e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1);
                        return;
                    }
                }
                Rectangle affectedBounds = e.AffectedBounds;
                using (Pen pen2 = new Pen(this.xdc87e2b99332cd4a.Res.Colors.SKIN2_CONTROLBORDERCOLOR))
                {
                    e.Graphics.DrawLine(pen2, affectedBounds.Left, affectedBounds.Bottom - 1, affectedBounds.Right - 4, affectedBounds.Bottom - 1);
                    e.Graphics.DrawLine(pen2, affectedBounds.Right - 1, affectedBounds.Top, affectedBounds.Right - 1, affectedBounds.Bottom - 4);
                    e.Graphics.DrawBezier(pen2, new Point(affectedBounds.Right - 4, affectedBounds.Bottom - 1), new Point(affectedBounds.Right - 3, affectedBounds.Bottom - 2), new Point(affectedBounds.Right - 2, affectedBounds.Bottom - 3), new Point(affectedBounds.Right - 1, affectedBounds.Bottom - 4));
                }
            }
        }

        protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
        {
            this.x0f4884030a491f49.DrawToolStripStatusLabelBackground(e);
        }

        private void x11d38b1ae9f5b99b(ToolStripItemRenderEventArgs xfbf34718e704c6bc)
        {
            Brush brush;
            Rectangle rect = new Rectangle(xfbf34718e704c6bc.Item.ContentRectangle.X - 2, xfbf34718e704c6bc.Item.ContentRectangle.Y - 2, xfbf34718e704c6bc.Item.ContentRectangle.Width + 3, xfbf34718e704c6bc.Item.ContentRectangle.Height + 3);
            this.xf2f6451a5d77af97(xfbf34718e704c6bc.Item, xfbf34718e704c6bc);
            if (xfbf34718e704c6bc.Item.Selected)
            {
                brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARONCOLOR;
                xfbf34718e704c6bc.Graphics.FillRectangle(brush, rect);
                using (Pen pen = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARBORDERCOLOR))
                {
                    xfbf34718e704c6bc.Graphics.DrawRectangle(pen, rect);
                }
            }
            if (xfbf34718e704c6bc.Item is ToolStripButton)
            {
                ToolStripButton item = (ToolStripButton) xfbf34718e704c6bc.Item;
                if (item.Checked)
                {
                    brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARONCOLOR;
                    xfbf34718e704c6bc.Graphics.FillRectangle(brush, rect);
                    using (Pen pen2 = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARBORDERCOLOR))
                    {
                        xfbf34718e704c6bc.Graphics.DrawRectangle(pen2, rect);
                    }
                }
            }
            if (xfbf34718e704c6bc.Item.Pressed)
            {
                if ((xfbf34718e704c6bc.Item is ToolStripDropDownButton) || (xfbf34718e704c6bc.Item is ToolStripSplitButton))
                {
                    brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARONCOLOR;
                }
                else
                {
                    brush = this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARDOWNCOLOR;
                }
                xfbf34718e704c6bc.Graphics.FillRectangle(brush, rect);
                using (Pen pen3 = new Pen(this.xdc87e2b99332cd4a.Res.Brushes.SKIN2_TOOLBARBORDERCOLOR))
                {
                    xfbf34718e704c6bc.Graphics.DrawRectangle(pen3, rect);
                }
            }
        }

        private void xa8c2e639f6764635(Image xe058541ca798c059, ToolStripRenderEventArgs xfbf34718e704c6bc)
        {
            int width = xfbf34718e704c6bc.ToolStrip.Width;
            int height = xfbf34718e704c6bc.ToolStrip.Height;
            if (xe058541ca798c059 != null)
            {
                Rectangle rectangle2;
                Rectangle srcRect = new Rectangle(0, 0, xe058541ca798c059.Width, xe058541ca798c059.Height);
                switch (xfbf34718e704c6bc.ToolStrip.BackgroundImageLayout)
                {
                    case ImageLayout.None:
                        rectangle2 = srcRect;
                        xfbf34718e704c6bc.Graphics.DrawImage(xe058541ca798c059, rectangle2, srcRect, GraphicsUnit.Pixel);
                        return;

                    case ImageLayout.Tile:
                    {
                        rectangle2 = new Rectangle(0, 0, width, height);
                        Rectangle dstRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
                        using (TextureBrush brush = new TextureBrush(xe058541ca798c059, dstRect, this.x233f092c536593eb))
                        {
                            xfbf34718e704c6bc.Graphics.FillRectangle(brush, rectangle2);
                            return;
                        }
                    }
                    case ImageLayout.Center:
                        if (width <= xe058541ca798c059.Width)
                        {
                            rectangle2 = new Rectangle(0, 0, xe058541ca798c059.Width, xe058541ca798c059.Height);
                            break;
                        }
                        rectangle2 = new Rectangle((width - xe058541ca798c059.Width) / 2, 0, xe058541ca798c059.Width, xe058541ca798c059.Height);
                        break;

                    case ImageLayout.Stretch:
                        rectangle2 = new Rectangle(0, 0, width, height);
                        xfbf34718e704c6bc.Graphics.DrawImage(xe058541ca798c059, rectangle2, srcRect, GraphicsUnit.Pixel);
                        return;

                    case ImageLayout.Zoom:
                        if (xe058541ca798c059.Height > 0)
                        {
                            float num3 = ((float) height) / ((float) xe058541ca798c059.Height);
                            int num4 = (int) (xe058541ca798c059.Width * num3);
                            if (width <= num4)
                            {
                                rectangle2 = new Rectangle(0, 0, xe058541ca798c059.Width, height);
                            }
                            else
                            {
                                rectangle2 = new Rectangle((width - num4) / 2, 0, num4, height);
                            }
                            xfbf34718e704c6bc.Graphics.DrawImage(xe058541ca798c059, rectangle2, srcRect, GraphicsUnit.Pixel);
                        }
                        return;

                    default:
                        return;
                }
                xfbf34718e704c6bc.Graphics.DrawImage(xe058541ca798c059, rectangle2, srcRect, GraphicsUnit.Pixel);
            }
        }

        private void xf2f6451a5d77af97(ToolStripItem xccb63ca5f63dc470, ToolStripItemRenderEventArgs xfbf34718e704c6bc)
        {
            if (xccb63ca5f63dc470.BackgroundImage != null)
            {
                Rectangle rectangle2;
                Image backgroundImage = xccb63ca5f63dc470.BackgroundImage;
                Rectangle srcRect = new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height);
                switch (xccb63ca5f63dc470.BackgroundImageLayout)
                {
                    case ImageLayout.None:
                        rectangle2 = srcRect;
                        xfbf34718e704c6bc.Graphics.DrawImage(backgroundImage, rectangle2, srcRect, GraphicsUnit.Pixel);
                        return;

                    case ImageLayout.Tile:
                    {
                        rectangle2 = new Rectangle(0, 0, xccb63ca5f63dc470.Width, xccb63ca5f63dc470.Height);
                        Rectangle dstRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
                        using (TextureBrush brush = new TextureBrush(backgroundImage, dstRect, this.x233f092c536593eb))
                        {
                            xfbf34718e704c6bc.Graphics.FillRectangle(brush, rectangle2);
                            return;
                        }
                    }
                    case ImageLayout.Center:
                        if (xccb63ca5f63dc470.Width <= backgroundImage.Width)
                        {
                            rectangle2 = new Rectangle(0, 0, backgroundImage.Width, backgroundImage.Height);
                            break;
                        }
                        rectangle2 = new Rectangle((xccb63ca5f63dc470.Width - backgroundImage.Width) / 2, 0, backgroundImage.Width, backgroundImage.Height);
                        break;

                    case ImageLayout.Stretch:
                        rectangle2 = new Rectangle(0, 0, xccb63ca5f63dc470.Width, xccb63ca5f63dc470.Height);
                        xfbf34718e704c6bc.Graphics.DrawImage(backgroundImage, rectangle2, srcRect, GraphicsUnit.Pixel);
                        return;

                    case ImageLayout.Zoom:
                        if (backgroundImage.Height > 0)
                        {
                            float num = ((float) xccb63ca5f63dc470.Height) / ((float) backgroundImage.Height);
                            int width = (int) (backgroundImage.Width * num);
                            if (xccb63ca5f63dc470.Width <= width)
                            {
                                rectangle2 = new Rectangle(0, 0, backgroundImage.Width, xccb63ca5f63dc470.Height);
                            }
                            else
                            {
                                rectangle2 = new Rectangle((xccb63ca5f63dc470.Width - width) / 2, 0, width, xccb63ca5f63dc470.Height);
                            }
                            xfbf34718e704c6bc.Graphics.DrawImage(backgroundImage, rectangle2, srcRect, GraphicsUnit.Pixel);
                        }
                        return;

                    default:
                        return;
                }
                xfbf34718e704c6bc.Graphics.DrawImage(backgroundImage, rectangle2, srcRect, GraphicsUnit.Pixel);
            }
        }
    }
}

