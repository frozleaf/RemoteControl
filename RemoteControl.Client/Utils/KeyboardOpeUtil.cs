using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;

namespace RemoteControl.Client
{
    class KeyboardOpeUtil
    {
        public static void KeyOpe(eKeyboardKeys key, eKeyboardOpe ope)
        {
            Win32API.keybd_event((byte)key, 0, ope == eKeyboardOpe.KeyDown ? 0 : 2, 0);
        }
    }
}
