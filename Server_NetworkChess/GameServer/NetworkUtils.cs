using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

public static class NetworkUtils
{
    //序列化
    public static byte[] Serialize(object obj)
    {
        //物体不为空可以被序列化
        if(obj == null || !obj.GetType().IsSerializable)
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            return data;
        }
    }

    //反序列化
    public static T Deserialize<T>(byte[] data) where T : class
    {
        //数据不为空且T是可序列化的类型
        if (data == null || !typeof(T).IsSerializable)
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(data))
        {
            object obj = formatter.Deserialize(stream);
            return obj as T;
        }
    }

    //获取本机IPv4, 失败返回null
    public static string GetLocalIPv4()
    {
        string hostName = Dns.GetHostName(); // 得到主机名
        IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
        for (int i = 0; i < iPHostEntry.AddressList.Length; i++)
        {
            //从ip中筛选出IPv4类型的地址
            if (iPHostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
            {
                return iPHostEntry.AddressList[i].ToString();
            }
        }
        return null;
    }
}
