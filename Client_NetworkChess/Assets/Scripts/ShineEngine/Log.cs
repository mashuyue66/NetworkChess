using System;
using System.Text;
using UnityEngine;

namespace ShineEngine
{
    public class Log
    {
        //显示调用栈字符长度
        private static int _stackStrLength = 2000;
        //sendLogMax
        private static int _sendLogMax = 4000;
        //固定时间
        private static long _fixedTimer;
        //当前帧序号  初始值为-2  为了 与profiler对应
        private static int _currentFrame = -2;

        private static Action<string> _printFunc;
        //发送日志回调
        private static Action<int, string> _sendLogFunc;
        //日志暂停
        private static bool _logPause = false;
        //正在log 中
        private static int _selfLoggingCount = 0;

        // 获取系统时间
        public static long getTimer()
        {
            return Environment.TickCount;
        }

        //获取毫微秒
        public static long getNanoTime()
        {
            return DateTime.Now.Ticks;
        }

        //获取固定系统时间
        public static long getFixedTimer()
        {
            return _fixedTimer;
        }

        //获取当前帧序号 调试profiler用
        public static int getCurrentFrame()
        {
            return _currentFrame;
        }

        //设置固定系统时间
        public static void makeFixDirty()
        {
            ++_currentFrame;
            _fixedTimer = getTimer();
        }

        public static void setLogPause(bool value)
        {
            _logPause = value;
        }

        //设置输出方法
        public static void setPrintFunc(Action<string> func)
        {
            _printFunc = func;
        }

        //设置输出方法
        public static void setSendLogFunc(Action<int, string> func)
        {
            _sendLogFunc = func;
        }

        //异常转string 
        public static void writeExceptionToString(StringBuilder sb, string str, Exception ex)
        {
            if (ex == null)
                ex = new Exception("void");

            if (!str.isEmpty())
            {
                sb.Append(str);
                sb.Append(' ');
            }

            sb.Append(ex.Message);
            sb.Append("\n");
            sb.Append(stackTraceToString(ex));
            sb.Append("\n");
        }

        //输出到控制台
        private static void doPrintToConsole(string str, bool isError)
        {
            if (GlobalSetting.consoleNeedTimestamp)
            {
                str = TimeUtils.getTimeStr(DateControl.getTimeMillis()) + " " + str;
            }

            ++_selfLoggingCount;

            if (isError)
            {
                Debug.LogError(str);
            }
            else
            {
                Debug.Log(str);
            }

            --_selfLoggingCount;

            if (_printFunc != null)
                _printFunc(str);
        }

        //发送Log
        private static void toSendLog(int type, string str)
        {
            if (type == SLogType.Local)
                return;

            if (!GlobalSetting.needLogSendServer || _logPause)
                return;

            if (str.Length > _sendLogMax)
                str = str.Substring(0, _sendLogMax) + "...";

            if (_sendLogFunc != null)
            {
                ThreadControl.addMainFunc(() =>
                {
                    _sendLogFunc(type, str);
                });
            }
        }

        //警告日志
        public static void warnLog(string str)
        {
            toLog(str, SLogType.Warning);
        }

        public static void warnLog(params object[] args)
        {
            toLog(StringUtils.objectsToString(args), SLogType.Warning);
        }

        public static void toLog(string str, int type)
        {
            if (!GlobalSetting.needLog)
                return;

            if (GlobalSetting.logNeedConsole)
            {
                doPrintToConsole(str, SLogType.isErrorLog(type));
            }

            toSendLog(type, str);
        }

        public static void toLog(StringBuilder sb, int type)
        {
            string str = sb.ToString();

            if (GlobalSetting.logNeedConsole)
                doPrintToConsole(str, SLogType.isErrorLog(type));

            toSendLog(type, str);
        }

        //打印
        public static void print(string str)
        {
            toLog(str, SLogType.Normal);
        }

        public static void print(params object[] args)
        {
            if (!GlobalSetting.needLog)
                return;

            toLog(StringUtils.objectsToString(args), SLogType.Normal);
        }

        //普通日志
        public static void log(string str)
        {
            toLog(str, SLogType.Normal);
        }

        public static void log(params object[] objs)
        {
            if (!GlobalSetting.needLog)
                return;

            toLog(StringUtils.objectsToString(objs), SLogType.Normal);
        }

        //客户端本地普通日志
        public static void localLog(string str)
        {
            toLog(str, SLogType.Local);
        }

        public static void localLog(params object[] objs)
        {
            if (!GlobalSetting.needLog)
                return;

            toLog(StringUtils.objectsToString(objs), SLogType.Local);
        }

        private static void toDebugLog(string str)
        {
            if (GlobalSetting.logNeedConsole)
                doPrintToConsole(str, false);

            toSendLog(SLogType.Normal, str);
        }

        public static void debugLog(string str)
        {
            if (!GlobalSetting.needDebugLog)
                return;

            toDebugLog(str);
        }

        //debug日志
        public static void debugLog(params object[] args)
        {
            if (!GlobalSetting.needLog)
                return;

            if (!GlobalSetting.needDebugLog)
                return;

            toDebugLog(StringUtils.objectsToString(args));
        }

        private static String exceptionToString(string str, Exception e)
        {
            StringBuilder sb = StringBuilderPool.createForThread();

            writeExceptionToString(sb, str, e);

            return StringBuilderPool.releaseStrForThread(sb);
        }

        //错误日志
        public static void errorLog(string str)
        {
            toLog(str, SLogType.Error);
        }

        //错误日志
        public static void errorLog(Exception e)
        {
            if (!GlobalSetting.needLog)
                return;

            toLog(exceptionToString("", e), SLogType.Error);
        }

        //错误日志
        public static void errorLog(string str, Exception e)
        {
            if (!GlobalSetting.needLog)
                return;

            toLog(exceptionToString(str, e), SLogType.Error);
        }

        //错误日志
        public static void errorLog(params object[] args)
        {
            if (!GlobalSetting.needLog)
                return;

            toLog(StringUtils.objectsToString(args), SLogType.Error);
        }

        //抛出异常
        public static void throwError(string str)
        {
            throwError(str, null);
        }

        //抛出异常
        public static void throwError(params object[] args)
        {
            StringBuilder sb = StringBuilderPool.createForThread();

            StringUtils.writeObjectsToStringBuilder(sb, args);

            toThrowError(sb, null);
        }

        //抛出异常
        public static void throwError(string str, Exception e)
        {
            StringBuilder sb = StringBuilderPool.createForThread();

            if (str != null)
            {
                sb.Append(str);
            }

            toThrowError(sb, e);
        }

        public static void toThrowError(StringBuilder sb, Exception e)
        {
            if (sb.Length > 0)
            {
                sb.Append("\n");
            }

            if (e != null)
            {
                sb.Append(e.Message);
                sb.Append("\n");
            }
            else
            {
                e = new Exception(sb.ToString());
            }

            string es = null;

            if (GlobalSetting.needError)
            {
                es = sb.ToString();
            }

            sb.Append(stackTraceToString(e));
            sb.Append("\n");

            string str = StringBuilderPool.releaseStrForThread(sb);

            toLog(str, SLogType.Error);

            if (GlobalSetting.needError)
            {
                throw new Exception(es);
            }
        }

        public static void impossible()
        {
            throwError("不该出现的情况");
        }

        public static void impossible(string str)
        {
            throwError("不该出现的情况: " + str);
        }

        //stringTrace 转string
        private static string stackTraceToString(Exception e)
        {
            string str = e.ToString();

            if (str == null)
                return "";

            if (str.Length > _stackStrLength)
                str = str.Substring(0, _stackStrLength) + "...";

            return str;
        }

        //打印当前调用栈
        public static void printNowStackTrace()
        {
            print(stackTraceToString(new Exception()));
        }

        public static string getStackTrace()
        {
            return stackTraceToString(new Exception());
        }

        public static void onSystemLog(string content, string stackTrace, LogType type)
        {
            if (_selfLoggingCount > 0)
                return;

            if (!GlobalSetting.needLog)
                return;

            string str = content + "\n" + stackTrace;
            toLog(str, SLogType.getSLogType(type));
        }

        //io
        public static void printForIO(string str)
        {
            ThreadControl.addMainFunc(() =>
            {
                toLog(str, SLogType.Normal);
            });
        }

        public static void printForIO(params object[] args)
        {
            if (!GlobalSetting.needLog)
                return;
            printForIO(StringUtils.objectsToString(args));
        }

        public static void printExceptionForIO(Exception e)
        {
            ThreadControl.addMainFunc(() =>
            {
                toLog(exceptionToString("", e), SLogType.Normal);
            });
        }

        public static void debugLogForIO(params object[] args)
        {
            ThreadControl.addMainFunc(() => { debugLog(args); });
        }

        //错误日志
        public static void errorLogForIO(params object[] args)
        {
            ThreadControl.addMainFunc(() => { errorLog(args); });
        }

        public static void warnLogForIO(params object[] args)
        {
            ThreadControl.addMainFunc(() => { warnLog(args); });
        }
    }
}