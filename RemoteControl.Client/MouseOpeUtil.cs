using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;
using System.Drawing;

namespace RemoteControl.Client
{
    class MouseOpeUtil
    {
        public static void MouseMove(int x, int y)
        {
            //Win32API.mouse_event(Win32API.MOUSEEVENTF_ABSOLUTE | Win32API.MOUSEEVENTF_MOVE, x, y, 0, 0);
            Win32API.SetCursorPos(x, y);
        }

        public static void MouseMove(Point p)
        {
            MouseMove(p.X, p.Y);
        }

        public static void MouseDown(eMouseButtons button, Point p)
        {
            int btn = Win32API.MOUSEEVENTF_LEFTDOWN;
            if(button == eMouseButtons.Left)
            {
                btn = Win32API.MOUSEEVENTF_LEFTDOWN;
            }
            else if(button == eMouseButtons.Middle)
            {
                btn = Win32API.MOUSEEVENTF_MIDDLEDOWN;
            }
            else if (button == eMouseButtons.Right)
            {
                btn = Win32API.MOUSEEVENTF_RIGHTDOWN;
            }
            else
            {
                return;
            }
            MouseMove(p);
            Win32API.mouse_event(Win32API.MOUSEEVENTF_ABSOLUTE | btn, p.X, p.Y, 0, 0);
        }

        public static void MouseUp(eMouseButtons button, Point p)
        {
            int btn = Win32API.MOUSEEVENTF_LEFTUP;
            if (button == eMouseButtons.Left)
            {
                btn = Win32API.MOUSEEVENTF_LEFTUP;
            }
            else if (button == eMouseButtons.Middle)
            {
                btn = Win32API.MOUSEEVENTF_MIDDLEUP;
            }
            else if (button == eMouseButtons.Right)
            {
                btn = Win32API.MOUSEEVENTF_RIGHTUP;
            }
            else
            {
                return;
            }
            MouseMove(p);
            Win32API.mouse_event(Win32API.MOUSEEVENTF_ABSOLUTE | btn, p.X, p.Y, 0, 0);
        }

        public static void MousePress(eMouseButtons button, Point p)
        {
            MouseDown(button, p);
            MouseUp(button, p);
        }

        public static void MouseDoubleClick(eMouseButtons button, Point p)
        {
            MousePress(button, p);
            MousePress(button, p);
        }
    }
}
