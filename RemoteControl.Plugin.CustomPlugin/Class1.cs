using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals.Plugin;
using Microsoft.Win32;

namespace RemoteControl.Plugin.CustomPlugin
{
    public class Class1 : AbstractPlugin
    {
        public override void Exec()
        {
            try
            {
                RegistryKey rootKey = RegistryKey.OpenBaseKey(
                                                            RegistryHive.CurrentUser,
                                                            Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);

                // 注意：调用OpenSubKey函数时，将第二参数设备true，才可以修改value值
                RegistryKey runKey = rootKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                bool runExists = runKey.GetValueNames().ToList().Contains("rc");
                if (runExists)
                {
                    runKey.DeleteValue("rc");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
