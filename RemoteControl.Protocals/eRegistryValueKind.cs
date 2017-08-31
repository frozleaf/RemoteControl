using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public enum eRegistryValueKind
    {
        None = -1,
        Unknown = 0,
        REG_SZ = 1,
        REG_EXPAND_SZ = 2,
        REG_BINARY = 3,
        REG_DWORD = 4,
        RED_MULTI_SZ = 7,
        RED_QWORD = 11,
    }
}
