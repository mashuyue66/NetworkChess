using UnityEngine;

namespace ShineEngine
{
    public class SLogType
    {
		//普通日志
		public const int Normal = 1;
		//警告日志
		public const int Warning = 2;
		//错误日志
		public const int Error = 3;
		//debug日志
		public const int Debug = 4;
		//客户端本地普通日志(不发送到服务器)
		public const int Local = 5;

		public static bool isErrorLog(int type)
		{
			return type == Error;
		}

		public static int getSLogType(LogType type)
		{
			switch (type)
			{
				case LogType.Error:
				case LogType.Exception:
					return Error;
				case LogType.Warning:
					return Warning;
			}

			return Normal;
		}
	}
}