using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace RemoteControl.Server
{
    class Settings
    {
        public int ServerPort;
        public string SkinPath;

        private const string SettingFileName = "config.json";
        public static Settings CurrentSettings = new Settings();

        static Settings()
        {
            try
            {
                string json = System.IO.File.ReadAllText(SettingFileName);
                Settings.CurrentSettings = JsonConvert.DeserializeObject<Settings>(json);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SaveSettings()
        {
            if (Settings.CurrentSettings == null)
                return;
            string json = JsonConvert.SerializeObject(Settings.CurrentSettings);
            System.IO.File.WriteAllText(SettingFileName, json);
        }
    }
}
