using UnityEngine;

namespace ShineEngine
{
    public class GlobalSetting
    {
        public const float spritePixel = 100f;
        public const float spritePixelT = 0.01f;

        //---系统相关
        //是否在Editor工程
        public static bool isEditor = false;
        //是否发布模式(不是本地调试运行)(打出手机包)
        public static bool isRelease = false;
        //是否正式线上
        public static bool isOfficial = false;
        //是否是完整客户端，否则只有数据，配置，协议等非显示内容。
        public static bool isWholeClient = true;
        //是否需要跑错
        public static bool needError = true;
        //是否启用对象池
        public static bool useObjectPool = true;
        //是否开启问题检测
        public static bool openCheck = true;
        //是否需要GM指令界面
        public static bool needGMCommandUI = true;
        //默认池大小(客户端的要小一些)
        public static int defaultPoolSize = 128;
        //系统时区
        public static string timeZone = "Asia/Shanghai";
        //是否启用夏令时检测
        public static bool needCheckDaylightSavings = false;
        //stringBuilder池保存尺寸上限
        public static int stringBuilderPoolKeepSize = 512;
        //stringBuilder池保存尺寸上限
        public static int bytesWriteStreamPoolKeepSize = 512;
        //stringBuilder使用池
        public static bool stringBuilderUsePool = true;
        //通信消息是否池化(客户端默认启用池化) */
        public static bool messageUsePool = true;
        //文件是否启用安全读写(客户端默认关) */
        public static bool fileReadWriteUseSafe = false;
        //ac字符串匹配是否保留英文字完整(默认开) */
        public static bool acStringFilterKeepWholeWords = true;

        //是否使用配置表读写tag模式 */
        public static bool configUseSwitchTagReadWrite = false;

        //---日志相关
        //是否需要日志(全部,包括print)
        public static bool needLog = true;
        //是否需要调试日志
        public static bool needDebugLog = true;
        //日志是否需要控制台输出
        public static bool logNeedConsole = true;
        //控制台是否需要时间戳
        public static bool consoleNeedTimestamp = true;
        //日志是否需要发送到服务器
        public static bool needLogSendServer = false;
        //是否需要data日志在一行
        public static bool needDataStringOneLine = false;

        //--线程/时间相关--//
        //系统帧率(各种系统计时器用)
        public static int systemFPS = 30;
        //系统间隔(毫秒)(各种系统计时器用)
        public static int systemFrameDelay = 1000 / systemFPS;
        //系统每份时间(1秒10次,逻辑用)
        public static int pieceTime = 100;
        //1秒3次时间
        public static int threeTime = 300;
        //默认线程sleep间隔(毫秒)
        public static int defaultThreadSleepDelay = 5;
        //日期时间戳更新间隔(毫秒)
        public static int dateFixDelay = 500;
        //是否需要线程func唤醒
        public static bool needThreadNotify = false;
    }
}