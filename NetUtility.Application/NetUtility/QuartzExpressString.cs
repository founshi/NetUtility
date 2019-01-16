using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetUtility
{
    class QuartzExpressString
    {
        /// <summary>
        /// 间隔执行时间(单位秒)
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string CronSencond(int seconds)
        {
            if (seconds <= 0) throw new Exception("间隔秒数不可小于等于0");
            return "*/" + seconds.ToString() + " * * * * ?";
        }
        /// <summary>
        /// 间隔执行时间(单位分钟)
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string CronMinute(int minutes)
        {
            if (minutes <= 0) throw new Exception("间隔分钟数不可小于等于0");
            return "0 */" + minutes.ToString() + " * * * ?";
        }
        /// <summary>
        /// 间隔执行时间(单位小时)
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static string CronHour(int hours)
        {
            if (hours <= 0) throw new Exception("间隔小时数不可小于等于0");
            return "0 0 */" + hours.ToString() + " * * ?";
        }
    }
}
