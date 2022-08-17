using System;
using System.Text;

namespace ShineEngine
{
    public class TimeUtils
    {
        //一天的毫秒数
        public static long dayTime = 60 * 60 *24 * 1000;
        //一小时的毫秒数
        public static long hourTime = 60 * 60 * 1000;
        //一分钟的毫秒数
        public static long minuteTime = 60 * 1000;
        //一秒的毫秒数
        public static long secondTime = 1000;
        //C#时间缩放
        public static int timeScale = 10000;
        //起始时间 ms 除过的
        public static long startTime = new DateTime(1970, 1, 1).Ticks / timeScale;

        //1970年1月1日8点
        private static DateTimeOffset _zeroDate = new DateTimeOffset(0L, TimeSpan.Zero);

        //时间偏移  默认东八区
        public static int zoneOffset = 28800000;

        //当前服务器时区   用于计算活动
        public static TimeZoneInfo curTimeZone;

        //通过DateTime获取毫秒数
        public static long getTimeMillisByDateTime(DateTime date)
        {
            return date.Ticks / timeScale - startTime;
        }

        //通过DateTime获取秒数
        public static long getTimeSecondsByDateTime(DateTime date)
        {
            return (date.Ticks / timeScale - startTime) / 1000L;
        }

        //创建cron表达式
        //public static CronExpression createCronExpression(string cron)
        //{

        //}

        //获取下一个日期  ms  每天0点
        public static long getNextDailyTime(long now)
        {
            return getNextDailyTime(now, zoneOffset);
        }

        //获取指定时区下一个日期 ms  每天0点
        public static long getNextDailyTime(long now, long zoneOff)
        {
            if(GlobalSetting.needCheckDaylightSavings && curTimeZone != null && curTimeZone.IsDaylightSavingTime(DateTime.Now))
                //夏令时特殊处理一下,调快1个小时
                zoneOff += hourTime;

            //先从utc转成对应时区的时间
            now += zoneOff;

            //计算对应时区的0点
            long zero = (now / dayTime + 1) * dayTime;
            //转换成utc 时间
            zero -= zoneOff;
            return zero;
        }

        //获取明天0点 ms
        public static long getNextDailyTime()
        {
            return getNextDailyTime(DateControl.getTimeMillis());
        }

        //获取当天0点 ms
        public static long getNowDailyTime()
        {
            return getNextDailyTime(DateControl.getTimeMillis()) - dayTime;
        }

        /// <summary>
		/// 根据秒数获取时间显示，格式XX:XX:XX
		/// </summary>
		/// <param name="second">秒数</param>
		/// <param name="autoLength">是否自动长度，如果否，则永远显示XX:XX:XX</param>
		/// <param name="needHour">是否需要小时</param>
		/// <returns></returns>
        public static string getTimeStringBySecond(int seconds, bool autoLength = false, bool needHour = true)
        {
            int hour = seconds / 3600;
            int minute = (seconds - hour * 3600) / 60;
            int second = seconds - hour * 3600 - minute * 60;

            StringBuilder sb = new StringBuilder();
            if (autoLength)
            {
                if(needHour && hour > 0)
                {
                    sb.Append(hour.ToString("D2"));
                    sb.Append(":");
                }
                if (minute > 0)
                {
                    sb.Append(minute.ToString("D2"));
                    sb.Append(":");
                }
                if (seconds >= 10)
                {
                    sb.Append(second.ToString("D2"));
                }
                else
                {
                    sb.Append(second.ToString());
                }
            }
            else
            {
                if (needHour && hour > 0)
                {
                    sb.Append(hour.ToString("D2"));
                    sb.Append(":");
                }

                sb.Append(minute.ToString("D2"));
                sb.Append(":");
                sb.Append(second.ToString("D2"));
            }

            return sb.ToString();
        }

        /// <summary>
		/// 根据毫秒数获取时间显示，格式XX:XX:XX
		/// </summary>
		/// <param name="second">毫秒数</param>
		/// <param name="autoLength">是否自动长度，如果否，则永远显示XX:XX:XX</param>
		/// <returns></returns>
        public static string getTimeStringByMillis(long time, bool autoLength = false, bool needHour = true)
        {
            return getTimeStringBySecond((int)(time / 1000), autoLength, needHour);
        }

        //根据毫秒数获得时间显示，格式 xx day OR XX:XX:XX
        public static string getDayOrTimeStringByMillis(long time, bool autoLength = false, bool needHour = true)
        {
            int day = (int)(time / 86400000);
            if(day > 2)
            {
                return day + "days";
            }
            else
            {
                return TimeUtils.getTimeStringByMillis(time, autoLength, needHour);
            }
        }

        //写入时间字符串表示 ms onlyDay仅仅需要显示到天
        public static void writeTimeStr(StringBuilder sb, long time, bool onlyDay = false)
        {
            
        }

        //获取字符串时间表达 ms
        public static string getTimeStr(long time, bool onlyDay = false)
        {
            StringBuilder sb = StringBuilderPool.create();
            writeTimeStr(sb, time, onlyDay);
            return StringBuilderPool.releaseStr(sb);
        }


    }
}
