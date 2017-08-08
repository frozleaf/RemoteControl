namespace Sunisoft.IrisSkin
{
    using System;

    internal class x6106db8218fc38c0
    {
        public static int line;

        public static void WiteConsoleMsg(int msg)
        {
            string str = "";
            switch (((uint) msg))
            {
                case 0:
                    str = "WM_NULL";
                    break;

                case 1:
                    str = "WM_CREATE";
                    break;

                case 2:
                    str = "WM_DESTROY";
                    break;

                case 3:
                    str = "WM_MOVE";
                    break;

                case 5:
                    str = "WM_SIZE";
                    break;

                case 6:
                    str = "WM_ACTIVATE";
                    break;

                case 7:
                    str = "WM_SETFOCUS";
                    break;

                case 8:
                    str = "WM_KILLFOCUS";
                    break;

                case 10:
                    str = "WM_ENABLE";
                    break;

                case 11:
                    str = "WM_SETREDRAW";
                    break;

                case 12:
                    str = "WM_SETTEXT";
                    break;

                case 13:
                    str = "WM_GETTEXT";
                    break;

                case 14:
                    str = "WM_GETTEXTLENGTH";
                    break;

                case 15:
                    str = "WM_PAINT";
                    break;

                case 0x10:
                    str = "WM_CLOSE";
                    break;

                case 0x11:
                    str = "WM_QUERYENDSESSION";
                    break;

                case 0x12:
                    str = "WM_QUIT";
                    break;

                case 0x13:
                    str = "WM_QUERYOPEN";
                    break;

                case 20:
                    str = "WM_ERASEBKGND";
                    break;

                case 0x15:
                    str = "WM_SYSCOLORCHANGE";
                    break;

                case 0x16:
                    str = "WM_ENDSESSION";
                    break;

                case 0x18:
                    str = "WM_SHOWWINDOW";
                    break;

                case 0x1a:
                    str = "WM_WININICHANGE";
                    break;

                case 0x1b:
                    str = "WM_DEVMODECHANGE";
                    break;

                case 0x1c:
                    str = "WM_ACTIVATEAPP";
                    break;

                case 0x1d:
                    str = "WM_FONTCHANGE";
                    break;

                case 30:
                    str = "WM_TIMECHANGE";
                    break;

                case 0x1f:
                    str = "WM_CANCELMODE";
                    break;

                case 0x20:
                    str = "WM_SETCURSOR";
                    break;

                case 0x21:
                    str = "WM_MOUSEACTIVATE";
                    break;

                case 0x22:
                    str = "WM_CHILDACTIVATE";
                    break;

                case 0x23:
                    str = "WM_QUEUESYNC";
                    break;

                case 0x81:
                    str = "WM_NCCREATE";
                    break;

                case 130:
                    str = "WM_NCDESTROY";
                    break;

                case 0x83:
                    str = "WM_NCCALCSIZE";
                    break;

                case 0x84:
                    str = "WM_NCHITTEST";
                    break;

                case 0x85:
                    str = "WM_NCPAINT";
                    break;

                case 0x86:
                    str = "WM_NCACTIVATE";
                    break;

                case 0x87:
                    str = "WM_GETDLGCODE";
                    break;

                case 0x88:
                    str = "WM_SYNCPAINT";
                    break;

                case 160:
                    str = "WM_NCMOUSEMOVE";
                    break;

                case 0xa1:
                    str = "WM_NCLBUTTONDOWN";
                    break;

                case 0xa2:
                    str = "WM_NCLBUTTONUP";
                    break;

                case 0xa3:
                    str = "WM_NCLBUTTONDBLCLK";
                    break;

                case 0xa4:
                    str = "WM_NCRBUTTONDOWN";
                    break;

                case 0xa5:
                    str = "WM_NCRBUTTONUP";
                    break;

                case 0xa6:
                    str = "WM_NCRBUTTONDBLCLK";
                    break;

                case 0xa7:
                    str = "WM_NCMBUTTONDOWN";
                    break;

                case 0xa8:
                    str = "WM_NCMBUTTONUP";
                    break;

                case 0xa9:
                    str = "WM_NCMBUTTONDBLCLK";
                    break;

                case 0xab:
                    str = "WM_NCXBUTTONDOWN";
                    break;

                case 0xac:
                    str = "WM_NCXBUTTONUP";
                    break;

                case 0xad:
                    str = "WM_NCXBUTTONDBLCLK";
                    break;

                case 0xff:
                    str = "WM_INPUT";
                    break;

                case 0x100:
                    str = "WM_KEYFIRST";
                    break;

                case 0x101:
                    str = "WM_KEYUP";
                    break;

                case 0x102:
                    str = "WM_CHAR";
                    break;

                case 0x103:
                    str = "WM_DEADCHAR";
                    break;

                case 260:
                    str = "WM_SYSKEYDOWN";
                    break;

                case 0x105:
                    str = "WM_SYSKEYUP";
                    break;

                case 0x106:
                    str = "WM_SYSCHAR";
                    break;

                case 0x107:
                    str = "WM_SYSDEADCHAR";
                    break;

                case 0x109:
                    str = "WM_UNICHAR";
                    break;

                case 0x10d:
                    str = "WM_IME_STARTCOMPOSITION";
                    break;

                case 270:
                    str = "WM_IME_ENDCOMPOSITION";
                    break;

                case 0x10f:
                    str = "WM_IME_COMPOSITION";
                    break;

                case 0x110:
                    str = "WM_INITDIALOG";
                    break;

                case 0x111:
                    str = "WM_COMMAND";
                    break;

                case 0x112:
                    str = "WM_SYSCOMMAND";
                    break;

                case 0x113:
                    str = "WM_TIMER";
                    break;

                case 0x114:
                    str = "WM_HSCROLL";
                    break;

                case 0x115:
                    str = "WM_VSCROLL";
                    break;

                case 0x116:
                    str = "WM_INITMENU";
                    break;

                case 0x117:
                    str = "WM_INITMENUPOPUP";
                    break;

                case 0x11f:
                    str = "WM_MENUSELECT";
                    break;

                case 0x120:
                    str = "WM_MENUCHAR";
                    break;

                case 0x121:
                    str = "WM_ENTERIDLE";
                    break;

                case 290:
                    str = "WM_MENURBUTTONUP";
                    break;

                case 0x123:
                    str = "WM_MENUDRAG";
                    break;

                case 0x124:
                    str = "WM_MENUGETOBJECT";
                    break;

                case 0x125:
                    str = "WM_UNINITMENUPOPUP";
                    break;

                case 0x126:
                    str = "WM_MENUCOMMAND";
                    break;

                case 0x127:
                    str = "WM_CHANGEUISTATE";
                    break;

                case 0x128:
                    str = "WM_UPDATEUISTATE";
                    break;

                case 0x129:
                    str = "WM_QUERYUISTATE";
                    break;

                case 0x132:
                    str = "WM_CTLCOLORMSGBOX";
                    break;

                case 0x133:
                    str = "WM_CTLCOLOREDIT";
                    break;

                case 0x134:
                    str = "WM_CTLCOLORLISTBOX";
                    break;

                case 0x135:
                    str = "WM_CTLCOLORBTN";
                    break;

                case 310:
                    str = "WM_CTLCOLORDLG";
                    break;

                case 0x137:
                    str = "WM_CTLCOLORSCROLLBAR";
                    break;

                case 0x138:
                    str = "WM_CTLCOLORSTATIC";
                    break;

                case 0x1e1:
                    str = "MN_GETHMENU";
                    break;

                case 0x210:
                    str = "WM_PARENTNOTIFY";
                    break;

                case 0x211:
                    str = "WM_ENTERMENULOOP";
                    break;

                case 530:
                    str = "WM_EXITMENULOOP";
                    break;

                case 0x213:
                    str = "WM_NEXTMENU";
                    break;

                case 0x214:
                    str = "WM_SIZING";
                    break;

                case 0x215:
                    str = "WM_CAPTURECHANGED";
                    break;

                case 0x216:
                    str = "WM_MOVING";
                    break;

                case 0xf00f:
                    str = "SC_SEPARATOR";
                    break;

                case 0xf010:
                    str = "SC_MOVE";
                    break;

                case 0xf020:
                    str = "SC_MINIMIZE";
                    break;

                case 0xf000:
                    str = "SC_SIZE";
                    break;

                case 0xf050:
                    str = "SC_PREVWINDOW";
                    break;

                case 0xf060:
                    str = "SC_CLOSE";
                    break;

                case 0xf030:
                    str = "SC_MAXIMIZE";
                    break;

                case 0xf040:
                    str = "SC_NEXTWINDOW";
                    break;

                case 0xf100:
                    str = "SC_KEYMENU";
                    break;

                case 0xf110:
                    str = "SC_ARRANGE";
                    break;

                case 0xf120:
                    str = "SC_RESTORE";
                    break;

                case 0xf070:
                    str = "SC_VSCROLL";
                    break;

                case 0xf080:
                    str = "SC_HSCROLL";
                    break;

                case 0xf090:
                    str = "SC_MOUSEMENU";
                    break;

                case 0xf130:
                    str = "SC_TASKLIST";
                    break;

                case 0xf140:
                    str = "SC_SCREENSAVE";
                    break;

                case 0xf150:
                    str = "SC_HOTKEY";
                    break;

                case 0xf180:
                    str = "SC_CONTEXTHELP";
                    break;

                case 0xffff:
                    str = "UNICODE_NOCHAR";
                    break;

                case 0xf160:
                    str = "SC_DEFAULT";
                    break;

                case 0xf170:
                    str = "SC_MONITORPOWER";
                    break;

                default:
                    str = msg.ToString("x");
                    break;
            }
            Console.WriteLine("{0}:  {1}", line, str);
            line++;
        }
    }
}

