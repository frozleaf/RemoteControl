using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RemoteControl.Protocals
{
    public class RequestMouseEvent
    {
        public eMouseButtons MouseButton;
        public eMouseOperations MouseOperation;
        public Point MouseLocation;
    }

    public enum eMouseButtons
    {
        Left = 0x100000,
        Middle = 0x400000,
        None = 0,
        Right = 0x200000,
        XButton1 = 0x800000,
        XButton2 = 0x1000000
    }

    public enum eMouseOperations
    {
        MouseDown,
        MouseUp,
        MousePress,
        MouseDoubleClick,
        MouseMove
    }
}
