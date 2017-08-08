using Sunisoft.IrisSkin.InternalControls;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sunisoft.IrisSkin
{
    [ToolboxItem(false)]
    internal class xa1883d0b59b7005b
    {
        private Menu xcbf78b15dd820156;

        private SkinEngine xcab6a0e662ada486;

        private MeasureItemEventHandler x60e3b5f01780bd33;

        private DrawItemEventHandler x2a5f0003704e39da;

        protected static ImageList menuImages;

        private static Pen x8eaf2b1854fb0bf6;

        private static Brush xd4d28a8f36023ad0;

        private bool xf2140268ef7ddbf7
        {
            get
            {
                return this.xcab6a0e662ada486.RealActive;
            }
        }

        public Menu Menu
        {
            get
            {
                return this.xcbf78b15dd820156;
            }
        }

        public xa1883d0b59b7005b(SkinEngine engine, Menu menu)
        {
            this.xcab6a0e662ada486 = engine;
            this.xcbf78b15dd820156 = menu;
            if (menu == null)
            {
                return;
            }
            this.x60e3b5f01780bd33 = new MeasureItemEventHandler(this.x763e46f2422fcb86);
            this.x2a5f0003704e39da = new DrawItemEventHandler(this.xd590f29a688f99d2);
            this.x5d04f127bbb82004();
            engine.CurrentSkinChanged += new SkinChanged(this.x9a0949aaa2f0a885);
        }

        static xa1883d0b59b7005b()
        {
            xa1883d0b59b7005b.x8eaf2b1854fb0bf6 = new Pen(Color.FromArgb(132, 130, 132), 1f);
            xa1883d0b59b7005b.xd4d28a8f36023ad0 = new SolidBrush(Color.Gray);
            xa1883d0b59b7005b.menuImages = x58dd58a96343fde0.LoadBitmapStrip(Type.GetType("Sunisoft.IrisSkin.SkinEngine"), "Sunisoft.IrisSkin.ImagesPopupMenu.bmp", new Size(16, 16), new Point(0, 0));
        }

        private void x76f955f9fcce00c1(ArrayList xf8b54ce7724a27f2)
        {
            foreach (object current in xf8b54ce7724a27f2)
            {
                MenuItem menuItem = (MenuItem)current;
                menuItem.OwnerDraw = true;
                menuItem.MeasureItem += this.x60e3b5f01780bd33;
                menuItem.DrawItem += this.x2a5f0003704e39da;
            }
        }

        private void xd590f29a688f99d2(object xe0292b9ed559da7d, DrawItemEventArgs xfbf34718e704c6bc)
        {
            if (!this.xf2140268ef7ddbf7)
            {
                return;
            }
            MenuItem menuItem = xe0292b9ed559da7d as MenuItem;
            if (menuItem.Parent is MainMenu)
            {
                return;
            }
            if (menuItem == null)
            {
                return;
            }
            Rectangle bounds = xfbf34718e704c6bc.Bounds;
            x448fd9ab43628c71.GetMessageFont();
            Rectangle rectangle;
            if (menuItem.Text == "-")
            {
                rectangle = Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Left + 26, bounds.Bottom);
                if (this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARSTARTCOLOR != this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARENDCOLOR)
                {
                    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARSTARTCOLOR, this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARENDCOLOR, LinearGradientMode.Horizontal))
                    {
                        xfbf34718e704c6bc.Graphics.FillRectangle(linearGradientBrush, rectangle);
                        goto IL_10A;
                    }
                }
                xa1883d0b59b7005b.DrawMenuBackGround(xfbf34718e704c6bc.Graphics, rectangle, this.xcab6a0e662ada486.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR);
            IL_10A:
                rectangle = Rectangle.FromLTRB(bounds.Left + 26, bounds.Top, bounds.Right, bounds.Bottom);
                xa1883d0b59b7005b.DrawMenuBackGround(xfbf34718e704c6bc.Graphics, rectangle, this.xcab6a0e662ada486.Res.Brushes.SKIN2_MENUITEMCOLOR);
                xa1883d0b59b7005b.DrawMenuLineItem(xfbf34718e704c6bc.Graphics, bounds, xa1883d0b59b7005b.x8eaf2b1854fb0bf6, 26);
                return;
            }
            if ((xfbf34718e704c6bc.State & DrawItemState.Selected) == DrawItemState.Selected && (xfbf34718e704c6bc.State & DrawItemState.Disabled) != DrawItemState.Disabled)
            {
                xa1883d0b59b7005b.DrawMenuBackGround(xfbf34718e704c6bc.Graphics, bounds, this.xcab6a0e662ada486.Res.Brushes.SKIN2_SELECTEDMENUCOLOR);
                xa1883d0b59b7005b.DrawMenuBorder(xfbf34718e704c6bc.Graphics, bounds, this.xcab6a0e662ada486.Res.Brushes.SKIN2_SELECTEDMENUBORDERCOLOR);
            }
            else
            {
                rectangle = Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Left + 26, bounds.Bottom);
                if (this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARSTARTCOLOR != this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARENDCOLOR)
                {
                    using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rectangle, this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARSTARTCOLOR, this.xcab6a0e662ada486.Res.Colors.SKIN2_LEFTBARENDCOLOR, LinearGradientMode.Horizontal))
                    {
                        xfbf34718e704c6bc.Graphics.FillRectangle(linearGradientBrush2, rectangle);
                        goto IL_287;
                    }
                }
                xa1883d0b59b7005b.DrawMenuBackGround(xfbf34718e704c6bc.Graphics, rectangle, this.xcab6a0e662ada486.Res.Brushes.SKIN2_LEFTBARSTARTCOLOR);
            IL_287:
                rectangle = Rectangle.FromLTRB(bounds.Left + 26, bounds.Top, bounds.Right, bounds.Bottom);
                xa1883d0b59b7005b.DrawMenuBackGround(xfbf34718e704c6bc.Graphics, rectangle, this.xcab6a0e662ada486.Res.Brushes.SKIN2_MENUITEMCOLOR);
            }
            Brush brush;
            if ((xfbf34718e704c6bc.State & DrawItemState.Disabled) != DrawItemState.Disabled)
            {
                if ((xfbf34718e704c6bc.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    brush = this.xcab6a0e662ada486.Res.Brushes.SKIN2_SELECTEDMENUFONTCOLOR;
                }
                else
                {
                    brush = this.xcab6a0e662ada486.Res.Brushes.SKIN2_MENUITEMFONTCOLOR;
                }
            }
            else
            {
                brush = xa1883d0b59b7005b.xd4d28a8f36023ad0;
            }
            DrawItemState arg_325_0 = xfbf34718e704c6bc.State & DrawItemState.Default;
            if (menuItem.Checked)
            {
                Image image;
                if (menuItem.RadioCheck)
                {
                    image = xa1883d0b59b7005b.menuImages.Images[1];
                }
                else
                {
                    image = this.xcab6a0e662ada486.Res.Bitmaps.SKIN2_CHECKEDMENUICON;
                }
                xfbf34718e704c6bc.Graphics.DrawImageUnscaled(image, bounds.X, bounds.Y);
            }
            rectangle = Rectangle.FromLTRB(bounds.Left + 30, bounds.Top, bounds.Right, bounds.Bottom);
            ContextMenu contextMenu = menuItem.GetContextMenu();
            if (contextMenu != null && (contextMenu.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
            {
                xfbf34718e704c6bc.Graphics.DrawString(menuItem.Text, xfbf34718e704c6bc.Font, brush, rectangle, x448fd9ab43628c71.MenuItemStringFormatR);
            }
            else
            {
                xfbf34718e704c6bc.Graphics.DrawString(menuItem.Text, xfbf34718e704c6bc.Font, brush, rectangle, x448fd9ab43628c71.MenuItemStringFormat);
            }
            if (menuItem.Shortcut != Shortcut.None)
            {
                rectangle = Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Right - 10, bounds.Bottom);
                xfbf34718e704c6bc.Graphics.DrawString(x448fd9ab43628c71.GetShortCutString(menuItem.Shortcut), xfbf34718e704c6bc.Font, brush, rectangle, x448fd9ab43628c71.MenuItemShortcutFormat);
            }
        }

        private void x763e46f2422fcb86(object xe0292b9ed559da7d, MeasureItemEventArgs xfbf34718e704c6bc)
        {
            MenuItem menuItem = xe0292b9ed559da7d as MenuItem;
            if (menuItem == null)
            {
                return;
            }
            if (menuItem.Parent is MainMenu || menuItem.Parent is ContextMenu)
            {
                xfbf34718e704c6bc.ItemHeight = 0;
                xfbf34718e704c6bc.ItemWidth = 0;
            }
            Font messageFont = x448fd9ab43628c71.GetMessageFont();
            SizeF sizeF = xfbf34718e704c6bc.Graphics.MeasureString(menuItem.Text, messageFont, 0, x448fd9ab43628c71.MenuItemStringFormat);
            if (menuItem.Text == "-")
            {
                xfbf34718e704c6bc.ItemHeight = 5;
            }
            else
            {
                xfbf34718e704c6bc.ItemHeight = (int)sizeF.Height + 5;
            }
            xfbf34718e704c6bc.ItemWidth = (int)sizeF.Width + 33;
            bool flag = false;
            int num = 0;
            if (menuItem.Parent != null)
            {
                foreach (MenuItem menuItem2 in menuItem.Parent.MenuItems)
                {
                    if (menuItem2.Shortcut != Shortcut.None)
                    {
                        flag = true;
                        int num2 = (int)xfbf34718e704c6bc.Graphics.MeasureString(x448fd9ab43628c71.GetShortCutString(menuItem2.Shortcut), messageFont, 0, x448fd9ab43628c71.MenuItemShortcutFormat).Width;
                        if (num < num2)
                        {
                            num = num2;
                        }
                    }
                }
            }
            if (flag)
            {
                xfbf34718e704c6bc.ItemWidth += num;
                xfbf34718e704c6bc.ItemWidth += 10;
            }
        }

        private void x9a0949aaa2f0a885(object xe0292b9ed559da7d, SkinChangedEventArgs xfbf34718e704c6bc)
        {
            this.x5d04f127bbb82004();
        }

        private void x5d04f127bbb82004()
        {
            bool xf2140268ef7ddbf = this.xf2140268ef7ddbf7;
            ArrayList arrayList = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            IEnumerator enumerator = this.Menu.MenuItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                MenuItem menuItem = (MenuItem)enumerator.Current;
                menuItem.OwnerDraw = xf2140268ef7ddbf;
                arrayList2.Add(menuItem);
                if (menuItem.MenuItems.Count > 0)
                {
                    arrayList.Add(menuItem);
                }
            }
            goto IL_FB;
        IL_81:
            MenuItem menuItem2 = (MenuItem)arrayList[0];
            arrayList.RemoveAt(0);
            foreach (MenuItem menuItem3 in menuItem2.MenuItems)
            {
                menuItem3.OwnerDraw = xf2140268ef7ddbf;
                arrayList2.Add(menuItem3);
                if (menuItem3.MenuItems.Count > 0)
                {
                    arrayList.Add(menuItem3);
                }
            }
        IL_FB:
            if (arrayList.Count <= 0)
            {
                if (xf2140268ef7ddbf)
                {
                    this.x76f955f9fcce00c1(arrayList2);
                }
                return;
            }
            goto IL_81;
        }

        public static void DrawMenuBackGround(Graphics g, Rectangle rect, Brush brush)
        {
            g.FillRectangle(brush, rect);
        }

        public static void DrawMenuLineItem(Graphics g, Rectangle rect, Pen p, int barWidth)
        {
            g.DrawLine(p, rect.Left + barWidth + 4, rect.Top + 2, rect.Right - 2, rect.Top + 2);
        }

        public static void DrawMenuBorder(Graphics g, Rectangle rect, Brush brush)
        {
            Pen pen = new Pen(brush, 1f);
            g.DrawRectangle(pen, Rectangle.FromLTRB(rect.Right, rect.Left, rect.Top, rect.Bottom - 1));
            pen.Dispose();
        }
    }
}
