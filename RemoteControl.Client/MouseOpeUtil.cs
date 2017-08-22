using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;

namespace RemoteControl.Client
{
    class MouseOpeUtil
    {
        public static void MouseTo(int x, int y)
        {
            Win32API.mouse_event(Win32API.MOUSEEVENTF_ABSOLUTE | Win32API.MOUSEEVENTF_MOVE, x, y, 0, 0);
        }

        public static void MouseDown(eMouseButton button, int x, int y)
        {
            Win32API.mouse_event(Win32API.MOUSEEVENTF_ABSOLUTE | Win32API.MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
        }

        private static int MouseButtonTypeToValue(eMouseButton button)
        {
            return 0;
        }
    }
}
