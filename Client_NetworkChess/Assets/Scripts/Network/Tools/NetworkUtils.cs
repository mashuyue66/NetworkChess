using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class NetworkUtils
{

    //���������Ϊ[serializable] ��ִ�����л������򷵻ؿ�
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

    //���obj�����Ϊ[serializable]�����ִ�з����л������򷵻ؿ�
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

    //��ȡ������IPv4����ȡʧ�ܷ���null
    public static string GetLocalIPv4()
    {
        string hostName = Dns.GetHostName();
        IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
        //�� IP��ַ�б���ɸѡ�� ipv4���͵� ip
        for(int i = 0; i < iPHostEntry.AddressList.Length; i++)
        {
            if (iPHostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                return iPHostEntry.AddressList[i].ToString();
        }
        return null;
    }

    //����������תΪ�ַ���
    public static string Byte2String(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    //���ַ���תΪ��������
    public static byte[] String2Byte(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }
}
