using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class NetworkUtils
{

    //如果物体标记为[serializable] 则执行序列化，否则返回空
    public static byte[] Serialize(object obj)
    {
        if(obj == null || !obj.GetType().IsSerializable)
            return null;
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            return data;
        }
    }

    //如果obj被标记为[serializable]则可以执行反序列化，否则返回空
    public static T Deserialize<T>(byte[] data) where T : class
    {
        if (data == null || !typeof(T).IsSerializable)
            return null;
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(data))
        {
            object obj = formatter.Deserialize(stream);
            return obj as T;
        }
    }

    //获取本机的IPv4，获取失败返回null
    public static string GetLocalIPv4()
    {
        string hostName = Dns.GetHostName();
        IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
        //从 IP地址列表中筛选出 ipv4类型的 ip
        for(int i = 0; i < iPHostEntry.AddressList.Length; i++)
        {
            if (iPHostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                return iPHostEntry.AddressList[i].ToString();
        }
        return null;
    }

    //将比特数组转为字符串
    public static string Byte2String(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    //将字符串转为比特数组
    public static byte[] String2Byte(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }
}
