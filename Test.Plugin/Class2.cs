using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals.Plugin;

namespace Test.Plugin
{
    class Class2:AbstractPlugin
    {
        public override void Exec()
        {
            Console.WriteLine("我的第二个代码插件！");
        }
    }
}
