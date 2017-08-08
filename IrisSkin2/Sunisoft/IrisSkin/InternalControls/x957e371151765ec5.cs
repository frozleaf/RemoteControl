namespace Sunisoft.IrisSkin.InternalControls
{
    using Sunisoft.IrisSkin;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    internal class x957e371151765ec5
    {
        protected static IntPtr halfToneBrush = IntPtr.Zero;

        public static void DrawButtonCommand(Graphics g, xdbfa333b4cd503e0 direction, Rectangle drawRect, x44fcc2f4d57dc4d8 state, Color baseColor, Color trackLight, Color trackBorder)
        {
            Rectangle rect = new Rectangle(drawRect.Left, drawRect.Top, drawRect.Width - 1, drawRect.Height - 1);
            switch (state)
            {
                case x44fcc2f4d57dc4d8.Normal:
                {
                    using (SolidBrush brush = new SolidBrush(baseColor))
                    {
                        g.FillRectangle(brush, rect);
                        break;
                    }
                }
                case x44fcc2f4d57dc4d8.HotTrack:
                    g.FillRectangle(Brushes.White, rect);
                    using (SolidBrush brush2 = new SolidBrush(trackLight))
                    {
                        g.FillRectangle(brush2, rect);
                    }
                    using (Pen pen = new Pen(trackBorder))
                    {
                        g.DrawRectangle(pen, rect);
                    }
                    break;

                case x44fcc2f4d57dc4d8.Pushed:
                    break;

                default:
                    return;
            }
        }

        public static void DrawDragRectangle(Rectangle newRect, int indent)
        {
            Rectangle[] newRects = new Rectangle[] { newRect };
            DrawDragRectangles(newRects, indent);
        }

        public static void DrawDragRectangles(Rectangle[] newRects, int indent)
        {
            if (newRects.Length > 0)
            {
                IntPtr dest = xa941746b2e12e6c1(newRects[0], indent);
                for (int i = 1; i < newRects.Length; i++)
                {
                    IntPtr ptr2 = xa941746b2e12e6c1(newRects[i], indent);
                    x31775329b2a4ff52.CombineRgn(dest, dest, ptr2, 3);
                    x31775329b2a4ff52.DeleteObject(ptr2);
                }
                IntPtr dC = x61467fe65a98f20c.GetDC(IntPtr.Zero);
                x31775329b2a4ff52.SelectClipRgn(dC, dest);
                xae4dd1cafd2eb77c.RECT rectBox = new xae4dd1cafd2eb77c.RECT();
                x31775329b2a4ff52.GetClipBox(dC, ref rectBox);
                IntPtr hObject = xc51886532d6aac02();
                IntPtr ptr5 = x31775329b2a4ff52.SelectObject(dC, hObject);
                x31775329b2a4ff52.PatBlt(dC, rectBox.left, rectBox.top, rectBox.right - rectBox.left, rectBox.bottom - rectBox.top, 0x5a0049);
                x31775329b2a4ff52.SelectObject(dC, ptr5);
                x31775329b2a4ff52.SelectClipRgn(dC, IntPtr.Zero);
                x31775329b2a4ff52.DeleteObject(dest);
                x61467fe65a98f20c.ReleaseDC(IntPtr.Zero, dC);
            }
        }

        public static void DrawPlainRaised(Graphics g, Rectangle boxRect, Color baseColor)
        {
            using (Pen pen = new Pen(ControlPaint.LightLight(baseColor)))
            {
                using (Pen pen2 = new Pen(ControlPaint.DarkDark(baseColor)))
                {
                    g.DrawLine(pen, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
                    g.DrawLine(pen, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
                    g.DrawLine(pen2, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
                    g.DrawLine(pen2, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
                }
            }
        }

        public static void DrawPlainRaisedBorder(Graphics g, Rectangle rect, Color lightLight, Color baseColor, Color dark, Color darkDark)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using (Pen pen = new Pen(lightLight))
                {
                    using (Pen pen2 = new Pen(baseColor))
                    {
                        using (Pen pen3 = new Pen(dark))
                        {
                            using (Pen pen4 = new Pen(darkDark))
                            {
                                int left = rect.Left;
                                int top = rect.Top;
                                int right = rect.Right;
                                int bottom = rect.Bottom;
                                g.DrawLine(pen2, right - 1, top, left, top);
                                g.DrawLine(pen, (int) (right - 2), (int) (top + 1), (int) (left + 1), (int) (top + 1));
                                g.DrawLine(pen2, (int) (right - 3), (int) (top + 2), (int) (left + 2), (int) (top + 2));
                                g.DrawLine(pen2, left, top, left, bottom - 1);
                                g.DrawLine(pen, (int) (left + 1), (int) (top + 1), (int) (left + 1), (int) (bottom - 2));
                                g.DrawLine(pen2, (int) (left + 2), (int) (top + 2), (int) (left + 2), (int) (bottom - 3));
                                g.DrawLine(pen4, (int) (right - 1), (int) (top + 1), (int) (right - 1), (int) (bottom - 1));
                                g.DrawLine(pen3, (int) (right - 2), (int) (top + 2), (int) (right - 2), (int) (bottom - 2));
                                g.DrawLine(pen2, (int) (right - 3), (int) (top + 3), (int) (right - 3), (int) (bottom - 3));
                                g.DrawLine(pen4, right - 1, bottom - 1, left, bottom - 1);
                                g.DrawLine(pen3, (int) (right - 2), (int) (bottom - 2), (int) (left + 1), (int) (bottom - 2));
                                g.DrawLine(pen2, (int) (right - 3), (int) (bottom - 3), (int) (left + 2), (int) (bottom - 3));
                            }
                        }
                    }
                }
            }
        }

        public static void DrawPlainRaisedBorderTopOrBottom(Graphics g, Rectangle rect, Color lightLight, Color baseColor, Color dark, Color darkDark, bool drawTop)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using (Pen pen = new Pen(lightLight))
                {
                    using (Pen pen2 = new Pen(baseColor))
                    {
                        using (Pen pen3 = new Pen(dark))
                        {
                            using (Pen pen4 = new Pen(darkDark))
                            {
                                int left = rect.Left;
                                int top = rect.Top;
                                int right = rect.Right;
                                int bottom = rect.Bottom;
                                if (drawTop)
                                {
                                    g.DrawLine(pen2, right - 1, top, left, top);
                                    g.DrawLine(pen, right - 1, top + 1, left, top + 1);
                                    g.DrawLine(pen2, right - 1, top + 2, left, top + 2);
                                }
                                else
                                {
                                    g.DrawLine(pen4, right - 1, bottom - 1, left, bottom - 1);
                                    g.DrawLine(pen3, right - 1, bottom - 2, left, bottom - 2);
                                    g.DrawLine(pen2, right - 1, bottom - 3, left, bottom - 3);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DrawPlainSunken(Graphics g, Rectangle boxRect, Color baseColor)
        {
            using (Pen pen = new Pen(ControlPaint.LightLight(baseColor)))
            {
                using (Pen pen2 = new Pen(ControlPaint.DarkDark(baseColor)))
                {
                    g.DrawLine(pen2, boxRect.Left, boxRect.Bottom, boxRect.Left, boxRect.Top);
                    g.DrawLine(pen2, boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Top);
                    g.DrawLine(pen, boxRect.Right, boxRect.Top, boxRect.Right, boxRect.Bottom);
                    g.DrawLine(pen, boxRect.Right, boxRect.Bottom, boxRect.Left, boxRect.Bottom);
                }
            }
        }

        public static void DrawPlainSunkenBorder(Graphics g, Rectangle rect, Color lightLight, Color baseColor, Color dark, Color darkDark)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using (Pen pen = new Pen(lightLight))
                {
                    using (Pen pen2 = new Pen(baseColor))
                    {
                        using (Pen pen3 = new Pen(dark))
                        {
                            using (Pen pen4 = new Pen(darkDark))
                            {
                                int left = rect.Left;
                                int top = rect.Top;
                                int right = rect.Right;
                                int bottom = rect.Bottom;
                                g.DrawLine(pen3, right - 1, top, left, top);
                                g.DrawLine(pen4, (int) (right - 2), (int) (top + 1), (int) (left + 1), (int) (top + 1));
                                g.DrawLine(pen2, (int) (right - 3), (int) (top + 2), (int) (left + 2), (int) (top + 2));
                                g.DrawLine(pen3, left, top, left, bottom - 1);
                                g.DrawLine(pen4, (int) (left + 1), (int) (top + 1), (int) (left + 1), (int) (bottom - 2));
                                g.DrawLine(pen2, (int) (left + 2), (int) (top + 2), (int) (left + 2), (int) (bottom - 3));
                                g.DrawLine(pen, (int) (right - 1), (int) (top + 1), (int) (right - 1), (int) (bottom - 1));
                                g.DrawLine(pen2, (int) (right - 2), (int) (top + 2), (int) (right - 2), (int) (bottom - 2));
                                g.DrawLine(pen2, (int) (right - 3), (int) (top + 3), (int) (right - 3), (int) (bottom - 3));
                                g.DrawLine(pen, right - 1, bottom - 1, left, bottom - 1);
                                g.DrawLine(pen2, (int) (right - 2), (int) (bottom - 2), (int) (left + 1), (int) (bottom - 2));
                                g.DrawLine(pen2, (int) (right - 3), (int) (bottom - 3), (int) (left + 2), (int) (bottom - 3));
                            }
                        }
                    }
                }
            }
        }

        public static void DrawPlainSunkenBorderTopOrBottom(Graphics g, Rectangle rect, Color lightLight, Color baseColor, Color dark, Color darkDark, bool drawTop)
        {
            if ((rect.Width > 2) && (rect.Height > 2))
            {
                using (Pen pen = new Pen(lightLight))
                {
                    using (Pen pen2 = new Pen(baseColor))
                    {
                        using (Pen pen3 = new Pen(dark))
                        {
                            using (Pen pen4 = new Pen(darkDark))
                            {
                                int left = rect.Left;
                                int top = rect.Top;
                                int right = rect.Right;
                                int bottom = rect.Bottom;
                                if (drawTop)
                                {
                                    g.DrawLine(pen3, right - 1, top, left, top);
                                    g.DrawLine(pen4, right - 1, top + 1, left, top + 1);
                                    g.DrawLine(pen2, right - 1, top + 2, left, top + 2);
                                }
                                else
                                {
                                    g.DrawLine(pen, right - 1, bottom - 1, left, bottom - 1);
                                    g.DrawLine(pen2, right - 1, bottom - 2, left, bottom - 2);
                                    g.DrawLine(pen2, right - 1, bottom - 3, left, bottom - 3);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DrawReverseString(Graphics g, string drawText, Font drawFont, Rectangle drawRect, Brush drawBrush, StringFormat drawFormat)
        {
            GraphicsContainer container = g.BeginContainer();
            g.TranslateTransform((float) ((drawRect.Left * 2) + drawRect.Width), (float) ((drawRect.Top * 2) + drawRect.Height));
            g.RotateTransform(180f);
            g.DrawString(drawText, drawFont, drawBrush, drawRect, drawFormat);
            g.EndContainer(container);
        }

        public static void DrawSeparatorCommand(Graphics g, xdbfa333b4cd503e0 direction, Rectangle drawRect, Color baseColor)
        {
            using (Pen pen = new Pen(ControlPaint.Dark(baseColor)))
            {
                if (direction == xdbfa333b4cd503e0.Horizontal)
                {
                    g.DrawLine(pen, drawRect.Left, drawRect.Top, drawRect.Left, drawRect.Bottom - 1);
                }
                else
                {
                    g.DrawLine(pen, drawRect.Left, drawRect.Top, drawRect.Right - 1, drawRect.Top);
                }
            }
        }

        protected static IntPtr xa941746b2e12e6c1(Rectangle x26545669838eb36e, int x94e516b5d3c734eb)
        {
            xae4dd1cafd2eb77c.RECT rect = new xae4dd1cafd2eb77c.RECT {
                left = x26545669838eb36e.Left,
                top = x26545669838eb36e.Top,
                right = x26545669838eb36e.Right,
                bottom = x26545669838eb36e.Bottom
            };
            IntPtr ptr = x31775329b2a4ff52.CreateRectRgnIndirect(ref rect);
            if (((x94e516b5d3c734eb <= 0) || (x26545669838eb36e.Width <= x94e516b5d3c734eb)) || (x26545669838eb36e.Height <= x94e516b5d3c734eb))
            {
                return ptr;
            }
            rect.left += x94e516b5d3c734eb;
            rect.top += x94e516b5d3c734eb;
            rect.right -= x94e516b5d3c734eb;
            rect.bottom -= x94e516b5d3c734eb;
            IntPtr ptr2 = x31775329b2a4ff52.CreateRectRgnIndirect(ref rect);
            xae4dd1cafd2eb77c.RECT rect2 = new xae4dd1cafd2eb77c.RECT {
                left = 0,
                top = 0,
                right = 0,
                bottom = 0
            };
            IntPtr dest = x31775329b2a4ff52.CreateRectRgnIndirect(ref rect2);
            x31775329b2a4ff52.CombineRgn(dest, ptr, ptr2, 3);
            x31775329b2a4ff52.DeleteObject(ptr);
            x31775329b2a4ff52.DeleteObject(ptr2);
            return dest;
        }

        protected static IntPtr xc51886532d6aac02()
        {
            if (halfToneBrush == IntPtr.Zero)
            {
                Bitmap bitmap = new Bitmap(8, 8, PixelFormat.Format32bppArgb);
                Color color = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
                Color color2 = Color.FromArgb(0xff, 0, 0, 0);
                bool flag = true;
                int x = 0;
                while (x < 8)
                {
                    int y = 0;
                    while (y < 8)
                    {
                        bitmap.SetPixel(x, y, flag ? color : color2);
                        y++;
                        flag = !flag;
                    }
                    x++;
                    flag = !flag;
                }
                IntPtr hbitmap = bitmap.GetHbitmap();
                x1439a41cfa24189f.LOGBRUSH brush = new x1439a41cfa24189f.LOGBRUSH {
                    lbStyle = 3,
                    lbHatch = (uint) ((int) hbitmap)
                };
                halfToneBrush = x31775329b2a4ff52.CreateBrushIndirect(ref brush);
            }
            return halfToneBrush;
        }
    }
}

