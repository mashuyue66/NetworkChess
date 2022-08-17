using UnityEngine;

namespace ShineEngine
{
    public class GlobalSetting
    {
        public const float spritePixel = 100f;
        public const float spritePixelT = 0.01f;

        //---ϵͳ���
        //�Ƿ���Editor����
        public static bool isEditor = false;
        //�Ƿ񷢲�ģʽ(���Ǳ��ص�������)(����ֻ���)
        public static bool isRelease = false;
        //�Ƿ���ʽ����
        public static bool isOfficial = false;
        //�Ƿ��������ͻ��ˣ�����ֻ�����ݣ����ã�Э��ȷ���ʾ���ݡ�
        public static bool isWholeClient = true;
        //�Ƿ���Ҫ�ܴ�
        public static bool needError = true;
        //�Ƿ����ö����
        public static bool useObjectPool = true;
        //�Ƿ���������
        public static bool openCheck = true;
        //�Ƿ���ҪGMָ�����
        public static bool needGMCommandUI = true;
        //Ĭ�ϳش�С(�ͻ��˵�ҪСһЩ)
        public static int defaultPoolSize = 128;
        //ϵͳʱ��
        public static string timeZone = "Asia/Shanghai";
        //�Ƿ���������ʱ���
        public static bool needCheckDaylightSavings = false;
        //stringBuilder�ر���ߴ�����
        public static int stringBuilderPoolKeepSize = 512;
        //stringBuilder�ر���ߴ�����
        public static int bytesWriteStreamPoolKeepSize = 512;
        //stringBuilderʹ�ó�
        public static bool stringBuilderUsePool = true;
        //ͨ����Ϣ�Ƿ�ػ�(�ͻ���Ĭ�����óػ�) */
        public static bool messageUsePool = true;
        //�ļ��Ƿ����ð�ȫ��д(�ͻ���Ĭ�Ϲ�) */
        public static bool fileReadWriteUseSafe = false;
        //ac�ַ���ƥ���Ƿ���Ӣ��������(Ĭ�Ͽ�) */
        public static bool acStringFilterKeepWholeWords = true;

        //�Ƿ�ʹ�����ñ��дtagģʽ */
        public static bool configUseSwitchTagReadWrite = false;

        //---��־���
        //�Ƿ���Ҫ��־(ȫ��,����print)
        public static bool needLog = true;
        //�Ƿ���Ҫ������־
        public static bool needDebugLog = true;
        //��־�Ƿ���Ҫ����̨���
        public static bool logNeedConsole = true;
        //����̨�Ƿ���Ҫʱ���
        public static bool consoleNeedTimestamp = true;
        //��־�Ƿ���Ҫ���͵�������
        public static bool needLogSendServer = false;
        //�Ƿ���Ҫdata��־��һ��
        public static bool needDataStringOneLine = false;

        //--�߳�/ʱ�����--//
        //ϵͳ֡��(����ϵͳ��ʱ����)
        public static int systemFPS = 30;
        //ϵͳ���(����)(����ϵͳ��ʱ����)
        public static int systemFrameDelay = 1000 / systemFPS;
        //ϵͳÿ��ʱ��(1��10��,�߼���)
        public static int pieceTime = 100;
        //1��3��ʱ��
        public static int threeTime = 300;
        //Ĭ���߳�sleep���(����)
        public static int defaultThreadSleepDelay = 5;
        //����ʱ������¼��(����)
        public static int dateFixDelay = 500;
        //�Ƿ���Ҫ�߳�func����
        public static bool needThreadNotify = false;
    }
}