using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals.Plugin;

namespace Test.Plugin
{
    public class Class1 : AbstractPlugin
    {
        public void Exec()
        {
            Console.WriteLine("我的第一个代码插件");
        }
    }
}
