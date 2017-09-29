using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals.Utilities
{
    /// <summary>
    /// 计划任务操作工具类
    /// </summary>
    public class SchTaskUtil
    {
        /*
         * SCHTASKS /Create [/S system [/U username [/P [password]]]]
                    [/RU username [/RP password]] /SC schedule [/MO modifier] [/D day]
                    [/M months] [/I idletime] /TN taskname /TR taskrun [/ST starttime]
                    [/RI interval] [ {/ET endtime | /DU duration} [/K] [/XML xmlfile] [/V1]]
                    [/SD startdate] [/ED enddate] [/IT | /NP] [/Z] [/F]
         * 
         * /SC   schedule     指定计划频率。
                       有效计划任务:  MINUTE、 HOURLY、DAILY、WEEKLY、 
                       MONTHLY, ONCE, ONSTART, ONLOGON, ONIDLE, ONEVENT.
         */

        const string SchTasksFile = "schtasks.exe";

        /// <summary>
        /// 删除计划任务
        /// </summary>
        /// <param name="scheduleName"></param>
        public static void DeleteSchedule(string scheduleName)
        {
            string arguments = string.Format("/delete /tn \"{0}\" /f", scheduleName);
            Run(arguments);
        }

        /// <summary>
        /// 立即运行计划任务
        /// </summary>
        /// <param name="scheduleName"></param>
        public static void RunSchedule(string scheduleName)
        {
            string arguments = string.Format("/run /tn \"{0}\"", scheduleName);
            Run(arguments);
        }

        /// <summary>
        /// 创建登录时的计划任务
        /// </summary>
        /// <param name="scheduleName"></param>
        /// <param name="filePath"></param>
        public static void CreateScheduleOnLogon(string scheduleName, string filePath)
        {
            CreateSchedule(scheduleName, filePath, "ONLOGON");
        }

        /// <summary>
        /// 创建计划任务
        /// </summary>
        /// <param name="scheduleName"></param>
        /// <param name="filePath"></param>
        /// <param name="schedul"></param>
        public static void CreateSchedule(string scheduleName, string filePath, string schedul)
        {
            string arguments = string.Format("/create /tn \"{0}\" /tr \"{1}\" /sc {2}", scheduleName, filePath, schedul);
            Run(arguments);
        }

        /// <summary>
        /// 根据参数调用计划任务程序
        /// </summary>
        /// <param name="arguments"></param>
        private static void Run(string arguments)
        {
            var t = ProcessUtil.Run(SchTasksFile, arguments, true);
            t.Join();
        }
    }
}
