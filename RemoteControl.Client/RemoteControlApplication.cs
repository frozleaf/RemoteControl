using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Client
{
    internal static class RemoteControlApplication
    {
        public static event EventHandler QuitEvent;

        public static void FireQuitEvent()
        {
            QuitEvent?.Invoke(null, null);
        }
    }
}
