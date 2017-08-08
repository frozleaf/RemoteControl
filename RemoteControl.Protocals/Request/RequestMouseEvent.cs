using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public class RequestMouseEvent
    {
        public eMouseButton MouseButton;
        public eMouseOperation MouseOperation;
        public int X;
        public int Y;
    }

    public enum eMouseButton
    {
        None,
        ButtonLeft,
        ButtonRight,
        ButtonMiddle
    }

    public enum eMouseOperation
    {
        MouseDown,
        MouseUp,
        MouseMove
    }
}
