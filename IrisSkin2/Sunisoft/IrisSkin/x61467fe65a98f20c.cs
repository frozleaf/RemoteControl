namespace Sunisoft.IrisSkin
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    internal class x61467fe65a98f20c
    {
        private x61467fe65a98f20c()
        {
        }

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool AnimateWindow(IntPtr hwnd, uint dwTime, uint dwFlags);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr BeginPaint(IntPtr hwnd, out x40255b11ef821fa3.PAINTSTRUCT lpPaint);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr CallWindowProc(xc405cb1a6ec3e1f6 lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref x555516122dcc901e.POINT lpPoint);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern IntPtr DefWindowProcA(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr DispatchMessage(ref x40255b11ef821fa3.MSG lpmsg);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyWidth, uint istepIfAniCur, IntPtr hbrFlickerFreeDraw, uint diFlags);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool EndPaint(IntPtr hWnd, ref x40255b11ef821fa3.PAINTSTRUCT lpPaint);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int FrameRect(IntPtr hDC, ref xae4dd1cafd2eb77c.RECT lprc, IntPtr hbr);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetCapture();
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern uint GetClassLong(IntPtr hWnd, int nIndex);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetClientRect(IntPtr hWnd, ref xae4dd1cafd2eb77c.RECT lpRect);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetClientRect(IntPtr hWnd, ref Rectangle lpRect);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref x40255b11ef821fa3.COMBOBOXINFO cbi);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetCursorPos(ref Point lpPoint);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern int GetDlgCtrlID(IntPtr hwndCtl);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetFocus();
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern ushort GetKeyState(int virtKey);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetMessage(ref x40255b11ef821fa3.MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int GetMessagePos();
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetScrollBarInfo(IntPtr hwnd, uint idObject, ref x40255b11ef821fa3.SCROLLBARINFO psbi);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref x40255b11ef821fa3.SCROLLINFO lpsi);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool GetScrollRange(IntPtr hWnd, int nBar, ref int lpMinPos, ref int lpMaxPos);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern int GetSysColor(int nIndex);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetTitleBarInfo(IntPtr hwnd, ref x40255b11ef821fa3.TITLEBARINFO pti);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint wCmd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref x40255b11ef821fa3.WINDOWINFO pwi);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern int GetWindowPlacement(IntPtr hWnd, ref x40255b11ef821fa3.WINDOWPLACEMENT lpwndpl);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref xae4dd1cafd2eb77c.RECT lpRect);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);
        [DllImport("User32.dll", CharSet=CharSet.Ansi)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern int GetWindowTextLengthA(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool HideCaret(IntPtr hWnd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hWnd, ref xae4dd1cafd2eb77c.RECT lpRect, bool bErase);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool InvalidateRect(IntPtr hWnd, ref xae4dd1cafd2eb77c.RECT lpRect, int bErase);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool IsZoomed(IntPtr hWnd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, uint cursor);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PeekMessage(ref x40255b11ef821fa3.MSG msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PeekMessage(ref Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref x555516122dcc901e.POINT lpPoint);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool ScrollDC(IntPtr hDC, int dx, int dy, ref xae4dd1cafd2eb77c.RECT lprcScroll, ref xae4dd1cafd2eb77c.RECT lprcClip, IntPtr hrgnUpdate, out xae4dd1cafd2eb77c.RECT lprcUpdate);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool ScrollWindow(IntPtr hWnd, int XAmount, int YAmount, IntPtr lpRect, IntPtr lpClipRect);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int ScrollWindowEx(IntPtr hWnd, int dx, int dy, IntPtr prcScroll, IntPtr prcClip, IntPtr hrgnUpdate, IntPtr prcUpdate, uint flags);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr SetCapture(IntPtr hWnd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern uint SetClassLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetCursor(IntPtr hCursor);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetScrollInfo(IntPtr hwnd, int fnBar, ref x40255b11ef821fa3.SCROLLINFO lpsi, bool fRedraw);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern xc405cb1a6ec3e1f6 SetWindowLong(IntPtr hWnd, int nIndex, xc405cb1a6ec3e1f6 dwNewLong);
        [DllImport("User32.DLL", CharSet=CharSet.Auto)]
        public static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int idHook, x6161963e817c3cff lpfn, int hMod, int dwThreadId);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int idHook, xe4ad4a23d5a4f544 lpfn, int hMod, int dwThreadId);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ShowCaret(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, short cmdShow);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SystemParametersInfoA(uint uiAction, uint uiParam, ref x40255b11ef821fa3.NONCLIENTMETRICS pvParam, uint fWinIni);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SystemParametersInfoA(uint uiAction, uint uiParam, ref int bRetValue, uint fWinINI);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TrackMouseEvent(ref x40255b11ef821fa3.TRACKMOUSEEVENTS tme);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TranslateMessage(ref x40255b11ef821fa3.MSG lpMsg);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref x555516122dcc901e.POINT pptDst, ref x555516122dcc901e.SIZE psize, IntPtr hdcSrc, ref x555516122dcc901e.POINT pptSrc, int crKey, ref x1439a41cfa24189f.BLENDFUNCTION pblend, int dwFlags);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool UpdateWindow(IntPtr hWnd);
        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool WaitMessage();
    }
}

