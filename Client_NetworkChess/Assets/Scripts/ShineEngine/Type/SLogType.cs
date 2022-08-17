using UnityEngine;

namespace ShineEngine
{
    public class SLogType
    {
		//��ͨ��־
		public const int Normal = 1;
		//������־
		public const int Warning = 2;
		//������־
		public const int Error = 3;
		//debug��־
		public const int Debug = 4;
		//�ͻ��˱�����ͨ��־(�����͵�������)
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