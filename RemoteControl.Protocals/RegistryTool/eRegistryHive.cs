using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    /// <summary>
    /// 表示外部计算机上的顶级节点的可能值。
    /// </summary>
    public enum eRegistryHive
    {
        ClassesRoot = -2147483648,
        CurrentUser = -2147483647,
        LocalMachine = -2147483646,
        Users = -2147483645,
        PerformanceData = -2147483644,
        CurrentConfig = -2147483643,
        DynData = -2147483642,
    }
}
