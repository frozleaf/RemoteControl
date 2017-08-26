using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace RemoteControl.Protocals.Plugin
{
    /// <summary>
    /// 插件加载器
    /// </summary>
    public class PluginLoader
    {
        /// <summary>
        /// 是否为插件文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsPlugin(string filePath)
        {
            Assembly ass = System.Reflection.Assembly.LoadFrom(filePath);
            return IsPlugin(ass);
        }

        /// <summary>
        /// 是否为插件程序集
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static bool IsPlugin(Assembly ass)
        {
            try
            {
                Type[] types = ass.GetTypes();
                foreach (var type in types)
                {
                    if (IsPlugin(type))
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 是否为插件类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsPlugin(Type type)
        {
            if ((type.GetInterface(typeof(IPlugin).FullName) != null && type != typeof(AbstractPlugin)) ||
                       type.IsSubclassOf(typeof(AbstractPlugin)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="filePath"></param>
        public static void LoadPlugin(string filePath, EventHandler fireQuitEventHandler)
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            LoadPlugin(fileData, fireQuitEventHandler);
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="fileData"></param>
        public static void LoadPlugin(byte[] fileData, EventHandler fireQuitEventHandler)
        {
            Assembly ass = Assembly.Load(fileData);
            LoadPlugin(ass, fileData, fireQuitEventHandler);
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="assembly"></param>
        public static void LoadPlugin(Assembly ass, byte[] fileData, EventHandler fireQuitEventHandler)
        {
            if (IsPlugin(ass))
            {
                Type[] types = ass.GetTypes();
                foreach (var type in types)
                {
                    if (IsPlugin(type))
                    {
                        // 创建插件对象
                        IPlugin p = (IPlugin)ass.CreateInstance(type.FullName);
                        if (p != null)
                        {
                            if (fireQuitEventHandler!=null)
                            {
                                p.FireQuitEvent += fireQuitEventHandler; 
                            }
                            try
                            {
                                // 执行插件
                                p.Exec();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            try
                            {
                                // 执行插件
                                p.Exec(fileData);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }
    }
}
