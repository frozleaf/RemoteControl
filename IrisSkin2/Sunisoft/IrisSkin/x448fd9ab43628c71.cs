namespace Sunisoft.IrisSkin
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class x448fd9ab43628c71
    {
        public const int AlignWidth = 0;
        public const byte Encript1 = 0x44;
        public static byte Encript2 = 0x38;
        public const byte Encript3 = 0x21;
        public static byte Encript4 = 0x33;
        public const byte IV1 = 0x63;
        public static byte IV2 = 0x76;
        public static byte IV3 = 0xa1;
        public static byte IV4 = 0xa6;
        private static bool x02a9729195580525 = false;
        internal static Hashtable x1c12011307e0a753;
        private static Color x27d07b3cbf2c9479 = Color.FromArgb(0xff, 0, 0xff);
        private static StringFormat x488379a1c8afe803;
        private static Pen x4f0034703b20af0f;
        private static bool x605ccaa3586218f6 = false;
        private static ImageAttributes x68a7ff53010868dc = new ImageAttributes();
        private static ImageAttributes xa11b666f50e9398f = new ImageAttributes();
        private static StringFormat xad5d7fbaf75245c8;
        private static ImageAttributes xc7766c1739984987 = new ImageAttributes();
        private static StringFormat xd02bd90925d5d5ca;
        private static StringFormat xe8c22659bee459d5;

        static x448fd9ab43628c71()
        {
            IV4 = 0xb5;
            x4f0034703b20af0f = new Pen(Color.Black, 1f);
            x4f0034703b20af0f.DashStyle = DashStyle.Dot;
            x488379a1c8afe803 = new StringFormat();
            x488379a1c8afe803.Alignment = StringAlignment.Center;
            x488379a1c8afe803.LineAlignment = StringAlignment.Center;
            x488379a1c8afe803.HotkeyPrefix = HotkeyPrefix.Show;
            xd02bd90925d5d5ca = new StringFormat();
            xd02bd90925d5d5ca.Alignment = StringAlignment.Near;
            xd02bd90925d5d5ca.LineAlignment = StringAlignment.Center;
            xd02bd90925d5d5ca.HotkeyPrefix = HotkeyPrefix.Show;
            xe8c22659bee459d5 = new StringFormat();
            xe8c22659bee459d5.Alignment = StringAlignment.Near;
            xe8c22659bee459d5.LineAlignment = StringAlignment.Center;
            xe8c22659bee459d5.HotkeyPrefix = HotkeyPrefix.Show;
            xe8c22659bee459d5.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            xad5d7fbaf75245c8 = new StringFormat();
            xad5d7fbaf75245c8.Alignment = StringAlignment.Far;
            xad5d7fbaf75245c8.LineAlignment = StringAlignment.Center;
            xad5d7fbaf75245c8.HotkeyPrefix = HotkeyPrefix.None;
            xc7766c1739984987.SetWrapMode(WrapMode.TileFlipX);
            x68a7ff53010868dc.SetWrapMode(WrapMode.TileFlipY);
            xa11b666f50e9398f.SetNoOp();
            xa11b666f50e9398f.SetWrapMode(WrapMode.TileFlipXY);
            IV2 = 0x77;
            Encript2 = 0x44;
            x1c12011307e0a753 = new Hashtable();
            x1c12011307e0a753.Add("flcRzsRtDNr2XBiCoY7NrAC352AiFA/4YuLs4nDCyOHZX5xvWtgH/g==", null);
            x1c12011307e0a753.Add("isBx30VBCC0GTnJuOKmQ0jK7I8NqeyOjDigXGIG5v1dN7aw4qc3Ogw==", null);
        }

        private x448fd9ab43628c71()
        {
        }

        public static void CalcLayoutRect(ContentAlignment align, Rectangle clientRect, RightToLeft rightToLeft, out Rectangle rect, out StringFormat format)
        {
            format = new StringFormat();
            if (rightToLeft == RightToLeft.Yes)
            {
                format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            }
            format.HotkeyPrefix = HotkeyPrefix.Show;
            rect = Rectangle.FromLTRB(clientRect.Left, clientRect.Top, clientRect.Right, clientRect.Bottom);
            switch (align)
            {
                case ContentAlignment.TopLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    return;

                case ContentAlignment.TopCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    return;

                case ContentAlignment.TopRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    return;

                case ContentAlignment.MiddleLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    return;

                case ContentAlignment.BottomCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    return;

                case ContentAlignment.BottomRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    return;

                case ContentAlignment.MiddleRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    return;

                case ContentAlignment.BottomLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    return;
            }
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
        }

        public static Rectangle CalcLayoutRect(ContentAlignment align, RightToLeft rightToLeft, Rectangle clientRect, int width, int height)
        {
            return CalcLayoutRect(align, rightToLeft, clientRect, width, height, 0);
        }

        public static Rectangle CalcLayoutRect(ContentAlignment align, RightToLeft rightToLeft, Rectangle clientRect, int width, int height, int alignWidth)
        {
            return CalcLayoutRect(align, rightToLeft, clientRect, width, height, alignWidth, alignWidth);
        }

        public static Rectangle CalcLayoutRect(ContentAlignment align, RightToLeft rightToLeft, Rectangle clientRect, int width, int height, int alignW, int alignH)
        {
            int num = alignW;
            int num2 = alignH;
            switch (align)
            {
                case ContentAlignment.TopLeft:
                    if ((rightToLeft & RightToLeft.Yes) != RightToLeft.Yes)
                    {
                        return new Rectangle(clientRect.Left + num, clientRect.Top + num2, width, height);
                    }
                    return new Rectangle((clientRect.Right - num) - width, clientRect.Top + num2, width, height);

                case ContentAlignment.TopCenter:
                    return new Rectangle((clientRect.Width - width) / 2, clientRect.Top + num2, width, height);

                case ContentAlignment.TopRight:
                    if ((rightToLeft & RightToLeft.Yes) != RightToLeft.Yes)
                    {
                        return new Rectangle((clientRect.Right - num) - width, clientRect.Top + num2, width, height);
                    }
                    return new Rectangle(clientRect.Left + num, clientRect.Top + num2, width, height);

                case ContentAlignment.MiddleLeft:
                    if ((rightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                    {
                        return new Rectangle((clientRect.Right - num) - width, (clientRect.Height - height) / 2, width, height);
                    }
                    return new Rectangle(clientRect.Left + num, (clientRect.Height - height) / 2, width, height);

                case ContentAlignment.MiddleCenter:
                    return new Rectangle((clientRect.Width - width) / 2, (clientRect.Height - height) / 2, width, height);

                case ContentAlignment.BottomCenter:
                    return new Rectangle((clientRect.Width - width) / 2, (clientRect.Bottom - num2) - height, width, height);

                case ContentAlignment.BottomRight:
                    if ((rightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                    {
                        return new Rectangle(clientRect.Left + num, (clientRect.Bottom - num2) - height, width, height);
                    }
                    return new Rectangle((clientRect.Right - num) - width, (clientRect.Bottom - num2) - height, width, height);

                case ContentAlignment.MiddleRight:
                    if ((rightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                    {
                        return new Rectangle(clientRect.Left + num, (clientRect.Height - height) / 2, width, height);
                    }
                    return new Rectangle((clientRect.Right - num) - width, (clientRect.Height - height) / 2, width, height);

                case ContentAlignment.BottomLeft:
                    if ((rightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
                    {
                        return new Rectangle((clientRect.Right - num) - width, (clientRect.Bottom - num2) - height, width, height);
                    }
                    return new Rectangle(clientRect.Left + num, (clientRect.Bottom - num2) - height, width, height);
            }
            return new Rectangle((clientRect.Width - width) / 2, (clientRect.Height - height) / 2, width, height);
        }

        public static Color CalculateColor(Color front, Color back, int alpha)
        {
            Color color = Color.FromArgb(0xff, front);
            Color color2 = Color.FromArgb(0xff, back);
            float r = color.R;
            float g = color.G;
            float b = color.B;
            float num4 = color2.R;
            float num5 = color2.G;
            float num6 = color2.B;
            float num7 = ((r * alpha) / 255f) + (num4 * (((float) (0xff - alpha)) / 255f));
            float num8 = ((g * alpha) / 255f) + (num5 * (((float) (0xff - alpha)) / 255f));
            float num9 = ((b * alpha) / 255f) + (num6 * (((float) (0xff - alpha)) / 255f));
            byte red = (byte) num7;
            byte green = (byte) num8;
            byte blue = (byte) num9;
            return Color.FromArgb(0xff, red, green, blue);
        }

        public static string ConvertBase64StringToUpperCase(string str)
        {
            str = str.ToUpper();
            string str2 = "";
            foreach (char ch in str)
            {
                char ch2 = ch;
                if (ch2 == '+')
                {
                    str2 = str2 + 'J';
                }
                else if (ch2 != '/')
                {
                    if (ch2 != '=')
                    {
                        goto Label_0061;
                    }
                    str2 = str2 + 'Z';
                }
                else
                {
                    str2 = str2 + 'F';
                }
                continue;
            Label_0061:
                str2 = str2 + ch;
            }
            return str2;
        }

        public static void DrawArrowDown(Graphics g, int x, int y, bool enabled)
        {
            Pen white;
            Brush gray;
            Point[] points = new Point[3];
            if (!enabled)
            {
                white = Pens.White;
                gray = Brushes.White;
                points[0] = new Point(x + 1, y + 1);
                points[1] = new Point(x + 7, y + 1);
                points[2] = new Point(x + 4, y + 4);
                g.DrawPolygon(white, points);
                g.FillPolygon(gray, points);
                white = Pens.Gray;
                gray = Brushes.Gray;
            }
            else
            {
                gray = Brushes.Black;
                white = Pens.Black;
            }
            points[0] = new Point(x, y);
            points[1] = new Point(x + 6, y);
            points[2] = new Point(x + 3, y + 3);
            g.DrawPolygon(white, points);
            g.FillPolygon(gray, points);
        }

        public static void DrawArrowLeft(Graphics g, int x, int y, bool enabled)
        {
            Brush white;
            Point[] points = new Point[3];
            if (!enabled)
            {
                white = Brushes.White;
                points[0] = new Point(x + 1, y + 1);
                points[1] = new Point(x + 4, y + 4);
                points[2] = new Point(x + 4, y - 2);
                g.FillPolygon(white, points);
                white = Brushes.Gray;
            }
            else
            {
                white = Brushes.Black;
            }
            points[0] = new Point(x, y);
            points[1] = new Point(x + 3, y + 3);
            points[2] = new Point(x + 3, y - 3);
            g.FillPolygon(white, points);
        }

        public static void DrawArrowRight(Graphics g, int x, int y, bool enabled)
        {
            Brush white;
            Point[] points = new Point[3];
            if (!enabled)
            {
                white = Brushes.White;
                points[0] = new Point(x + 1, y + 1);
                points[1] = new Point(x + 1, y + 7);
                points[2] = new Point(x + 4, y + 4);
                g.FillPolygon(white, points);
                white = Brushes.Gray;
            }
            else
            {
                white = Brushes.Black;
            }
            points[0] = new Point(x, y);
            points[1] = new Point(x, y + 6);
            points[2] = new Point(x + 3, y + 3);
            g.FillPolygon(white, points);
        }

        public static void DrawArrowUp(Graphics g, int x, int y, bool enabled)
        {
            Pen white;
            Brush gray;
            Point[] points = new Point[3];
            if (!enabled)
            {
                white = Pens.White;
                gray = Brushes.White;
                points[0] = new Point(x + 1, y + 1);
                points[1] = new Point(x + 7, y + 1);
                points[2] = new Point(x + 4, y - 2);
                g.DrawPolygon(white, points);
                g.FillPolygon(gray, points);
                white = Pens.Gray;
                gray = Brushes.Gray;
            }
            else
            {
                gray = Brushes.Black;
                white = Pens.Black;
            }
            points[0] = new Point(x, y);
            points[1] = new Point(x + 6, y);
            points[2] = new Point(x + 3, y - 3);
            g.DrawPolygon(white, points);
            g.FillPolygon(gray, points);
        }

        public static string FormatStringWithWidth(Graphics g, string s, Font font, int width)
        {
            string text = s;
            Font font2 = font;
            int num = (int) g.MeasureString(s, font2).Width;
            if (width > num)
            {
                return text;
            }
            for (int i = 1; i < s.Length; i++)
            {
                text = s.Substring(0, i) + "...";
                num = (int) g.MeasureString(text, font2).Width;
                if (num >= width)
                {
                    return text;
                }
            }
            return s;
        }

        public static Font GetCaptionFont()
        {
            return new Font(SystemInformation.MenuFont, FontStyle.Bold);
        }

        public static string GetCommonDialogVersion()
        {
            x02a9729195580525 = true;
            string fullName = Assembly.GetAssembly(typeof(CommonDialog)).FullName;
            string str2 = null;
            int index = fullName.IndexOf("Version=");
            if (index != -1)
            {
                index += 8;
                int num2 = fullName.IndexOf(",", index);
                if (num2 == -1)
                {
                    num2 = fullName.Length - 1;
                }
                try
                {
                    str2 = fullName.Substring(index, num2 - index);
                }
                catch
                {
                }
            }
            return str2;
        }

        public static StringFormat GetControlCaptionOutputStringFormat(ContentAlignment align, Control control)
        {
            StringFormat format = new StringFormat();
            if ((control.RightToLeft & RightToLeft.Yes) == RightToLeft.Yes)
            {
                format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            }
            format.HotkeyPrefix = HotkeyPrefix.Show;
            switch (align)
            {
                case ContentAlignment.TopLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                    return format;

                case ContentAlignment.TopCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Near;
                    return format;

                case ContentAlignment.TopRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
                    return format;

                case ContentAlignment.MiddleLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    return format;

                case ContentAlignment.MiddleCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    return format;

                case ContentAlignment.BottomCenter:
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Far;
                    return format;

                case ContentAlignment.BottomRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Far;
                    return format;

                case ContentAlignment.MiddleRight:
                    format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Center;
                    return format;

                case ContentAlignment.BottomLeft:
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Far;
                    return format;
            }
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            return format;
        }

        public static Font GetMessageFont()
        {
            return SystemInformation.MenuFont;
        }

        public static string GetShortCutString(Shortcut shortcut)
        {
            char ch = (char) ((ushort) (shortcut & ((Shortcut) 0xffff)));
            if ((ch < '0') || (ch > '9'))
            {
                return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString((Keys) shortcut);
            }
            string str = "";
            int num = (int) (((long) shortcut) & 0xffff0000L);
            if ((num & 0x10000) != 0)
            {
                str = str + "Shift+";
            }
            if ((num & 0x20000) != 0)
            {
                str = str + "Ctrl+";
            }
            if ((num & 0x40000) != 0)
            {
                str = str + "Alt+";
            }
            return (str + ch);
        }

        public static bool InRect(Point p, xae4dd1cafd2eb77c.RECT r)
        {
            return ((((p.X >= r.left) && (p.X <= r.right)) && (p.Y >= r.top)) && (p.Y <= r.bottom));
        }

        public static bool InRect(Point p, Rectangle r)
        {
            return ((((p.X >= r.Left) && (p.X <= r.Right)) && (p.Y >= r.Top)) && (p.Y <= r.Bottom));
        }

        public static bool InRect(int x, int y, Rectangle r)
        {
            return ((((x >= r.Left) && (x <= r.Right)) && (y >= r.Top)) && (y <= r.Bottom));
        }

        public static Point MakePoint(IntPtr lParam)
        {
            int num = lParam.ToInt32();
            return new Point(num | 0xff, (num | 0xff00) >> 4);
        }

        public static void OffsetRect(ref xae4dd1cafd2eb77c.RECT rect, int dx, int dy)
        {
            rect.left += dx;
            rect.right += dx;
            rect.top += dy;
            rect.bottom += dy;
        }

        public static void OffsetRect(ref Rectangle rect, int dx, int dy)
        {
            rect.X += dx;
            rect.Y += dy;
        }

        public static void PaintSlider(SkinEngine Engine, Graphics g, Rectangle r, bool vScroll)
        {
            Bitmap bitmap;
            Rectangle rectangle;
            Rectangle rectangle2;
            int bottom;
            int top;
            int right;
            int left;
            int num = 0;
            if (vScroll)
            {
                num = 0;
            }
            else
            {
                num = 1;
            }
            if (vScroll)
            {
                if (Engine.Res.ScrollBarRes.IsMacOS && (r.Height > 0x10))
                {
                    SpitDrawVertical(Engine.Res.Bitmaps.SKIN2_SCROLLBAR, g, r, false);
                }
                else
                {
                    bitmap = Engine.Res.ScrollBarRes.Slider[num, 0];
                    if (r.Height <= (bitmap.Height * 2))
                    {
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = new Rectangle(r.Left, r.Top, r.Width, r.Height / 2);
                        g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                        bitmap = Engine.Res.ScrollBarRes.Slider[num, 4];
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = Rectangle.FromLTRB(r.Left, rectangle2.Bottom, r.Right, r.Bottom);
                        g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    }
                    else if (r.Height <= ((bitmap.Height * 2) + Engine.Res.ScrollBarRes.Slider[num, 2].Height))
                    {
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = new Rectangle(r.Left, r.Top, r.Width, bitmap.Height);
                        bottom = rectangle2.Bottom;
                        g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                        bitmap = Engine.Res.ScrollBarRes.Slider[num, 4];
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = new Rectangle(r.Left, r.Bottom - bitmap.Height, r.Width, bitmap.Height);
                        g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                        top = rectangle2.Top;
                        bitmap = Engine.Res.ScrollBarRes.Slider[num, 2];
                        rectangle2 = Rectangle.FromLTRB(r.Left, bottom, r.Right, top);
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = new Rectangle(r.Left, r.Top, r.Width, bitmap.Height);
                        bottom = rectangle2.Bottom;
                        g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                        bitmap = Engine.Res.ScrollBarRes.Slider[num, 2];
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = new Rectangle(r.Left, r.Top + ((r.Height - bitmap.Height) / 2), r.Width, bitmap.Height);
                        top = rectangle2.Top;
                        right = rectangle2.Bottom;
                        g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                        bitmap = Engine.Res.ScrollBarRes.Slider[num, 4];
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = new Rectangle(r.Left, r.Bottom - bitmap.Height, r.Width, bitmap.Height);
                        left = rectangle2.Top;
                        g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                        bitmap = Engine.Res.ScrollBarRes.Slider[num, 1];
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = Rectangle.FromLTRB(r.Left, bottom, r.Right, top);
                        g.DrawImage(bitmap, rectangle2, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, DrawImageAttrTileY);
                        bitmap = Engine.Res.ScrollBarRes.Slider[num, 3];
                        rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        rectangle2 = Rectangle.FromLTRB(r.Left, right, r.Right, left);
                        g.DrawImage(bitmap, rectangle2, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, DrawImageAttrTileY);
                    }
                }
            }
            else if (Engine.Res.ScrollBarRes.IsMacOS && (r.Width > 0x10))
            {
                SpitDrawHorizontal(Engine.Res.Bitmaps.SKIN2_SCROLLBAR, g, r, false);
            }
            else
            {
                bitmap = Engine.Res.ScrollBarRes.Slider[num, 0];
                if (r.Width <= (bitmap.Width * 2))
                {
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = new Rectangle(r.Left, r.Top, r.Width / 2, r.Height);
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    bitmap = Engine.Res.ScrollBarRes.Slider[num, 4];
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = Rectangle.FromLTRB(rectangle2.Right, r.Top, r.Right, r.Bottom);
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
                else if (r.Width <= ((bitmap.Width * 2) + Engine.Res.ScrollBarRes.Slider[num, 2].Width))
                {
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = new Rectangle(r.Left, r.Top, bitmap.Width, r.Height);
                    bottom = rectangle2.Right;
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    bitmap = Engine.Res.ScrollBarRes.Slider[num, 4];
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = new Rectangle(r.Right - bitmap.Width, r.Top, bitmap.Width, r.Height);
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    top = rectangle2.Left;
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    bitmap = Engine.Res.ScrollBarRes.Slider[num, 2];
                    rectangle2 = Rectangle.FromLTRB(bottom, r.Top, top, r.Bottom);
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
                else
                {
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = new Rectangle(r.Left, r.Top, bitmap.Width, r.Height);
                    bottom = rectangle2.Right;
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    bitmap = Engine.Res.ScrollBarRes.Slider[num, 2];
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = new Rectangle(r.Left + ((r.Width - bitmap.Width) / 2), r.Top, bitmap.Width, r.Height);
                    top = rectangle2.Left;
                    right = rectangle2.Right;
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    bitmap = Engine.Res.ScrollBarRes.Slider[num, 4];
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = new Rectangle(r.Right - bitmap.Width, r.Top, bitmap.Width, r.Height);
                    left = rectangle2.Left;
                    g.DrawImage(bitmap, rectangle2, rectangle, GraphicsUnit.Pixel);
                    bitmap = Engine.Res.ScrollBarRes.Slider[num, 1];
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = Rectangle.FromLTRB(bottom, r.Top, top, r.Bottom);
                    g.DrawImage(bitmap, rectangle2, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, DrawImageAttrTileX);
                    bitmap = Engine.Res.ScrollBarRes.Slider[num, 3];
                    rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    rectangle2 = Rectangle.FromLTRB(right, r.Top, left, r.Bottom);
                    g.DrawImage(bitmap, rectangle2, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, DrawImageAttrTileX);
                }
            }
        }

        public static void RemoveAllControl(Control control)
        {
            if ((control != null) && (control.Controls.Count > 0))
            {
                Button button = null;
                Form form = control.FindForm();
                if (form != null)
                {
                    button = new Button {
                        Visible = false
                    };
                    form.Controls.Add(button);
                    form.ActiveControl = button;
                }
                control.Controls.Clear();
                if (form != null)
                {
                    button.Dispose();
                    form.Controls.Remove(button);
                }
            }
        }

        public static void RemoveControl(Control.ControlCollection coll, Control item)
        {
            if ((coll != null) && (item != null))
            {
                Button button = null;
                Form form = item.FindForm();
                if (form != null)
                {
                    button = new Button {
                        Visible = false
                    };
                    form.Controls.Add(button);
                    form.ActiveControl = button;
                }
                coll.Remove(item);
                if (form != null)
                {
                    button.Dispose();
                    form.Controls.Remove(button);
                }
            }
        }

        public static Bitmap RoundPicture(Bitmap src)
        {
            Bitmap bitmap = new Bitmap(src.Height, src.Width);
            for (int i = 0; i < src.Height; i++)
            {
                for (int j = 0; j < src.Width; j++)
                {
                    bitmap.SetPixel(i, (src.Width - j) - 1, src.GetPixel(j, i));
                }
            }
            return bitmap;
        }

        public static int ShowSystemMessageBox(IntPtr hWnd, string l, string c, uint u)
        {
            return x61467fe65a98f20c.MessageBox(hWnd, l, c, u);
        }

        public static void SkinShowMessageBox()
        {
        }

        public static void SpitDrawHorizontal(Bitmap bmp, Graphics g, Rectangle r, bool transParent)
        {
            Rectangle rectangle;
            Rectangle rectangle2;
            int width = bmp.Width / 3;
            if (width > r.Width)
            {
                rectangle = new Rectangle(0, 0, bmp.Width, bmp.Height);
                rectangle2 = r;
                g.DrawImage(bmp, rectangle2, rectangle, GraphicsUnit.Pixel);
                rectangle = new Rectangle(bmp.Width - 2, 0, 2, bmp.Height);
                rectangle2 = new Rectangle(r.Right - 2, 0, 2, bmp.Height);
                g.DrawImage(bmp, rectangle2, rectangle, GraphicsUnit.Pixel);
            }
            else
            {
                rectangle2 = new Rectangle(r.X, r.Y, width, r.Height);
                rectangle = new Rectangle(0, 0, width, bmp.Height);
                g.DrawImage(bmp, rectangle2, rectangle, GraphicsUnit.Pixel);
                rectangle2 = Rectangle.FromLTRB(r.X + width, r.Y, r.Right - width, r.Bottom);
                rectangle = new Rectangle(width, 0, width, bmp.Height);
                g.DrawImage(bmp, rectangle2, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, DrawImageAttrTileX, null);
                rectangle2 = new Rectangle(r.Right - width, r.Y, width, r.Height);
                rectangle = new Rectangle(bmp.Width - width, 0, width, bmp.Height);
                g.DrawImage(bmp, rectangle2, rectangle, GraphicsUnit.Pixel);
            }
        }

        public static void SpitDrawVertical(Bitmap bmp, Graphics g, Rectangle r, bool transParent)
        {
            Rectangle rectangle;
            Rectangle rectangle2;
            int height = bmp.Height / 3;
            if (height > r.Height)
            {
                rectangle = new Rectangle(0, 0, bmp.Width, bmp.Height);
                rectangle2 = r;
                g.DrawImage(bmp, rectangle2, rectangle, GraphicsUnit.Pixel);
                rectangle = new Rectangle(0, bmp.Height - 2, bmp.Width, 2);
                rectangle2 = new Rectangle(r.Left, r.Bottom - 2, r.Width, 2);
                g.DrawImage(bmp, rectangle2, rectangle, GraphicsUnit.Pixel);
            }
            else
            {
                rectangle2 = new Rectangle(r.X, r.Y, r.Width, height);
                rectangle = new Rectangle(0, 0, bmp.Width, height);
                g.DrawImage(bmp, rectangle2, rectangle, GraphicsUnit.Pixel);
                rectangle2 = Rectangle.FromLTRB(r.X, r.Y + height, r.Right, r.Bottom - height);
                rectangle = new Rectangle(0, height, bmp.Width, height);
                g.DrawImage(bmp, rectangle2, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, GraphicsUnit.Pixel, DrawImageAttrTileX, null);
                rectangle2 = new Rectangle(r.X, r.Bottom - height, r.Width, height);
                rectangle = new Rectangle(0, bmp.Height - height, bmp.Width, height);
                g.DrawImage(bmp, rectangle2, rectangle, GraphicsUnit.Pixel);
            }
        }

        public static Bitmap SplitBitmap(Bitmap source, int count, int index)
        {
            if ((source == null) || (count == 0))
            {
                return null;
            }
            float width = ((float) source.Width) / ((float) count);
            float height = source.Height;
            RectangleF rect = new RectangleF(new PointF(width * (index - 1), 0f), new SizeF(width, height));
            return source.Clone(rect, source.PixelFormat);
        }

        public static void SplitDraw(Bitmap src, Graphics gDest, Rectangle rect)
        {
            Color fuchsia = Color.Fuchsia;
            if (rect.Bottom >= rect.Top)
            {
                Rectangle rectangle7;
                int width = rect.Width;
                int height = rect.Height;
                Rectangle rectangle4 = new Rectangle(0, 0, 0, 0);
                Rectangle rectangle3 = rectangle4;
                Rectangle rectangle5 = rectangle4;
                Rectangle rectangle6 = rectangle4;
                Rectangle srcRect = Rectangle.FromLTRB(0, 0, src.Width / 2, src.Height / 2);
                Rectangle destRect = Rectangle.FromLTRB(0, 0, width / 2, height / 2);
                int num3 = Math.Min(srcRect.Width, destRect.Width);
                if (num3 < 0)
                {
                    num3 = 0;
                }
                int num4 = Math.Min(srcRect.Height, destRect.Height);
                if (num4 < 0)
                {
                    num4 = 0;
                }
                if ((num3 != 0) && (num4 != 0))
                {
                    if (num3 < srcRect.Width)
                    {
                        srcRect.Width = num3;
                    }
                    if (num4 < srcRect.Height)
                    {
                        srcRect.Height = num4;
                    }
                    rectangle7 = new Rectangle(0, 0, num3, num4);
                    gDest.DrawImage(src, rectangle7, srcRect, GraphicsUnit.Pixel);
                    rectangle3 = new Rectangle(0, 0, num3, num4);
                }
                srcRect = Rectangle.FromLTRB(0, (src.Height - (src.Height / 2)) + 1, src.Width / 2, src.Height);
                destRect = Rectangle.FromLTRB(0, (height - (height / 2)) + 1, width / 2, height);
                if (destRect.Bottom > destRect.Top)
                {
                    num3 = Math.Min(srcRect.Width, destRect.Width);
                    if (num3 < 0)
                    {
                        num3 = 0;
                    }
                    num4 = Math.Min(srcRect.Height, destRect.Height);
                    if (num4 < 0)
                    {
                        num4 = 0;
                    }
                    if ((num3 != 0) && (num4 != 0))
                    {
                        if (num4 < srcRect.Height)
                        {
                            srcRect.Y += srcRect.Height - num4;
                            srcRect.Height = num4;
                        }
                        if (num3 < srcRect.Width)
                        {
                            srcRect.Width = num3;
                        }
                        rectangle7 = new Rectangle(0, rect.Bottom - num4, num3, num4);
                        gDest.DrawImage(src, rectangle7, srcRect, GraphicsUnit.Pixel);
                        rectangle4 = new Rectangle(0, rect.Bottom - num4, num3, num4);
                    }
                    srcRect = Rectangle.FromLTRB(0, src.Height / 2, src.Width / 2, src.Height - (src.Height / 2));
                    destRect = Rectangle.FromLTRB(0, rectangle3.Bottom, rectangle3.Right, rectangle4.Top);
                    if (destRect.Bottom > destRect.Top)
                    {
                        num3 = destRect.Width;
                        num4 = destRect.Height;
                        if ((num3 != 0) && (num4 != 0))
                        {
                            if (num3 < srcRect.Width)
                            {
                                srcRect.Width = num3;
                            }
                            rectangle7 = new Rectangle(0, 0, num3, num4);
                            gDest.DrawImage(src, destRect, srcRect, GraphicsUnit.Pixel);
                        }
                    }
                    srcRect = Rectangle.FromLTRB((src.Width - (src.Width / 2)) + 1, 0, src.Width, src.Height / 2);
                    destRect = Rectangle.FromLTRB((width - (width / 2)) + 1, 0, width, height / 2);
                    if (destRect.Bottom > destRect.Top)
                    {
                        num3 = Math.Min(srcRect.Width, destRect.Width);
                        if (num3 < 0)
                        {
                            num3 = 0;
                        }
                        num4 = Math.Min(srcRect.Height, destRect.Height);
                        if (num4 < 0)
                        {
                            num4 = 0;
                        }
                        if ((num4 != 0) && (num3 != 0))
                        {
                            if (num3 < srcRect.Width)
                            {
                                srcRect.X += srcRect.Width - num3;
                                srcRect.Width = num3;
                            }
                            if (num4 < srcRect.Height)
                            {
                                srcRect.Height = num4;
                            }
                            rectangle7 = new Rectangle(rect.Right - num3, 0, num3, num4);
                            gDest.DrawImage(src, rectangle7, srcRect, GraphicsUnit.Pixel);
                            rectangle5 = new Rectangle(rect.Right - num3, 0, num3, num4);
                        }
                        srcRect = Rectangle.FromLTRB((src.Width - (src.Width / 2)) + 1, (src.Height - (src.Height / 2)) + 1, src.Width, src.Height);
                        destRect = Rectangle.FromLTRB((width - (width / 2)) + 1, (height - (height / 2)) + 1, width, height);
                        if (destRect.Bottom > destRect.Top)
                        {
                            num3 = Math.Min(srcRect.Width, destRect.Width);
                            if (num3 < 0)
                            {
                                num3 = 0;
                            }
                            num4 = Math.Min(srcRect.Height, destRect.Height);
                            if (num4 < 0)
                            {
                                num4 = 0;
                            }
                            if ((num3 != 0) && (num4 != 0))
                            {
                                if (num3 < srcRect.Width)
                                {
                                    srcRect.X += srcRect.Width - num3;
                                    srcRect.Width = num3;
                                }
                                if (num4 < srcRect.Height)
                                {
                                    srcRect.Y += srcRect.Height - num4;
                                    srcRect.Height = num4;
                                }
                                rectangle7 = new Rectangle(rect.Right - num3, rect.Bottom - num4, num3, num4);
                                gDest.DrawImage(src, rectangle7, srcRect, GraphicsUnit.Pixel);
                                rectangle6 = new Rectangle(rect.Right - num3, rect.Bottom - num4, num3, num4);
                            }
                            srcRect = Rectangle.FromLTRB((src.Width - (src.Width / 2)) + 1, src.Height - (src.Height / 2), src.Width, (src.Height / 2) + 2);
                            destRect = Rectangle.FromLTRB(rectangle5.Left, rectangle5.Bottom, rectangle5.Right, rectangle6.Top);
                            if (destRect.Bottom > destRect.Top)
                            {
                                num3 = destRect.Width;
                                num4 = destRect.Height;
                                if ((num3 != 0) && (num4 != 0))
                                {
                                    if (num3 < srcRect.Width)
                                    {
                                        srcRect.X += srcRect.Width - num3;
                                        srcRect.Width = num3;
                                    }
                                    rectangle7 = new Rectangle(destRect.X, destRect.Y, num3, num4);
                                    gDest.DrawImage(src, rectangle7, srcRect, GraphicsUnit.Pixel);
                                }
                            }
                            srcRect = Rectangle.FromLTRB(src.Width / 2, 0, (src.Width - (src.Width / 2)) + 1, src.Height / 2);
                            destRect = Rectangle.FromLTRB(rectangle3.Right, 0, rectangle5.Left, rectangle3.Bottom);
                            if (destRect.Bottom > destRect.Top)
                            {
                                num3 = destRect.Width;
                                num4 = destRect.Height;
                                if ((num3 != 0) && (num4 != 0))
                                {
                                    if (num4 < srcRect.Height)
                                    {
                                        srcRect.Height = num4;
                                    }
                                    rectangle7 = new Rectangle(destRect.X, destRect.Y, num3, num4);
                                    gDest.DrawImage(src, rectangle7, srcRect, GraphicsUnit.Pixel);
                                }
                            }
                            srcRect = Rectangle.FromLTRB(src.Width / 2, src.Height / 2, (src.Width - (src.Width / 2)) + 1, src.Height - (src.Height / 2));
                            destRect = Rectangle.FromLTRB(rectangle3.Right, rectangle3.Bottom, rectangle5.Left, rectangle6.Top - 1);
                            if (destRect.Bottom > destRect.Top)
                            {
                                num3 = destRect.Width;
                                num4 = destRect.Height;
                                if ((num3 != 0) && (num3 != 0))
                                {
                                    rectangle7 = new Rectangle(destRect.X, destRect.Y, num3, num4);
                                    gDest.DrawImage(src, rectangle7, srcRect, GraphicsUnit.Pixel);
                                }
                            }
                            srcRect = Rectangle.FromLTRB(src.Width / 2, src.Height - (src.Height / 2), (src.Width - (src.Width / 2)) + 1, src.Height);
                            destRect = Rectangle.FromLTRB(rectangle3.Right - 1, rectangle4.Top - 1, rectangle6.Left, rectangle6.Bottom);
                            if (destRect.Bottom > destRect.Top)
                            {
                                num3 = destRect.Width;
                                num4 = destRect.Height;
                                if ((num3 != 0) && (num4 != 0))
                                {
                                    if (num4 < srcRect.Height)
                                    {
                                        srcRect.Y += srcRect.Height - num4;
                                        srcRect.Height = num4;
                                    }
                                    rectangle7 = new Rectangle(destRect.X, destRect.Y, num3, num4);
                                    gDest.DrawImage(src, rectangle7, srcRect, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void StretchDrawWithHeader(Graphics g, Bitmap bmp, Rectangle dest, Rectangle src, int header)
        {
            Rectangle srcRect = new Rectangle(src.X, src.Y, src.Width, header);
            Rectangle destRect = new Rectangle(dest.X, dest.Y, dest.Width, header);
            g.DrawImage(bmp, destRect, srcRect, GraphicsUnit.Pixel);
            srcRect = new Rectangle(src.X, src.Y + header, src.Width, src.Height - header);
            destRect = new Rectangle(dest.X, dest.Y + header, dest.Width, dest.Height - header);
            g.DrawImage(bmp, destRect, srcRect, GraphicsUnit.Pixel);
        }

        public static Color TabBackgroundFromBaseColor(Color backColor)
        {
            if (((backColor.R == 0xd4) && (backColor.G == 0xd0)) && (backColor.B == 200))
            {
                return Color.FromArgb(0xf7, 0xf3, 0xe9);
            }
            if (((backColor.R == 0xec) && (backColor.G == 0xe9)) && (backColor.B == 0xd8))
            {
                return Color.FromArgb(0xff, 0xfb, 0xe9);
            }
            int red = 0xff - ((0xff - backColor.R) / 2);
            int green = 0xff - ((0xff - backColor.G) / 2);
            int blue = 0xff - ((0xff - backColor.B) / 2);
            return Color.FromArgb(red, green, blue);
        }

        public static Color DefaultTransparentColor
        {
            get
            {
                return x27d07b3cbf2c9479;
            }
        }

        public static ImageAttributes DrawImageAttrTileX
        {
            get
            {
                return xc7766c1739984987;
            }
        }

        public static ImageAttributes DrawImageAttrTileXY
        {
            get
            {
                return xa11b666f50e9398f;
            }
        }

        public static ImageAttributes DrawImageAttrTileY
        {
            get
            {
                return x68a7ff53010868dc;
            }
        }

        public static Pen FocusRectanglePen
        {
            get
            {
                return x4f0034703b20af0f;
            }
        }

        public static bool IsNeedHookCommonDialog
        {
            get
            {
                if (!x02a9729195580525)
                {
                    switch (GetCommonDialogVersion())
                    {
                        case "1.0.3300.0":
                        case "1.0.5000.0":
                            x605ccaa3586218f6 = false;
                            goto Label_0035;
                    }
                    x605ccaa3586218f6 = true;
                }
            Label_0035:
                return x605ccaa3586218f6;
            }
        }

        public static bool IsVista
        {
            get
            {
                return (((Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major == 6)) && (Environment.OSVersion.Version.Minor == 0));
            }
        }

        public static StringFormat MainMenuStringFormat
        {
            get
            {
                return x488379a1c8afe803;
            }
        }

        public static StringFormat MenuItemShortcutFormat
        {
            get
            {
                return xad5d7fbaf75245c8;
            }
        }

        public static StringFormat MenuItemStringFormat
        {
            get
            {
                return xd02bd90925d5d5ca;
            }
        }

        public static StringFormat MenuItemStringFormatR
        {
            get
            {
                return xe8c22659bee459d5;
            }
        }

        public static Font TitleFont
        {
            get
            {
                return GetCaptionFont();
            }
        }
    }
}

