namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;

    internal class x31775329b2a4ff52
    {
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern int CombineRgn(IntPtr dest, IntPtr src1, IntPtr src2, int flags);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateBrushIndirect(ref x1439a41cfa24189f.LOGBRUSH brush);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateFontIndirect(ref x40255b11ef821fa3.LOGFONT lplf);
        [DllImport("Gdi32.DLL", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateRectRgnIndirect(ref xae4dd1cafd2eb77c.RECT rect);
        [DllImport("Gdi32.DLL", CharSet=CharSet.Auto)]
        public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("Gdi32.DLL", CharSet=CharSet.Auto)]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern int GetClipBox(IntPtr hDC, ref xae4dd1cafd2eb77c.RECT rectBox);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern bool PatBlt(IntPtr hDC, int x, int y, int width, int height, uint flags);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [DllImport("Gdi32.dll", CharSet=CharSet.Auto)]
        public static extern int SetBkMode(IntPtr hdc, int iBkMode);
    }
}

