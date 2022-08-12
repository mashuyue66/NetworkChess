using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Multiplay;

public delegate void CallBack(byte[] data);

public static class NetworkClient
{
    private class NetworkCoroutine : MonoBehaviour
    {
        private event Action ApplicationQuitEvent;

        private static NetworkCoroutine _instance;

        public static NetworkCoroutine Instance
        {
            get
            {
                if (!_instance)
                {
                    GameObject socketClientObj = new GameObject("NetworkCoroutine");
                    _instance = socketClientObj.AddComponent<NetworkCoroutine>();
                    DontDestroyOnLoad(socketClientObj);
                }
                return _instance;
            }
        }

        public void SetQuitEvent(Action func)
        {
            if (ApplicationQuitEvent != null)
                return;
            ApplicationQuitEvent += func;
        }

        private void OnApplicationQuit()
        {
            if (ApplicationQuitEvent != null)
            {
                ApplicationQuitEvent();
            }
        }
    }

    //客户端网络状态
    private enum ClientState
    {
        None,
        Connected,
    }

    //消息类型与回调字典
    private static Dictionary<MessageType, CallBack> _callBacks = new Dictionary<MessageType, CallBack>();
    //待发送消息队列
    private static Queue<byte[]> _messages;
    //当前状态
    private static ClientState _curState;
    //向服务器建立TCP连接并获取网络通讯流
    private static TcpClient _client;
    //在网络通讯流中读写数据
    private static NetworkStream _stream;

    //目标ip
    private static IPAddress _address;
    //端口号
    private static int _port;

    //心跳包机制
    private const float HEARTBEAT_TIME = 3;//心跳包发送时间间隔
    private static float _timer = HEARTBEAT_TIME; //距离上次接受心跳包的时间
    public static bool Received = true; //收到心跳包回信

    private static IEnumerator _Connect()
    {
        _client = new TcpClient();

        //异步连接
        IAsyncResult async = _client.BeginConnect(_address, _port, null, null);
        while (!async.IsCompleted)
        {
            Debug.Log("服务器连接中");
            yield return null;
        }

        //异常处理
        try
        {
            _client.EndConnect(async);
        }
        catch (Exception ex)
        {
            Info.Instance.Print("服务器连接失败：" + ex.Message, true);
            yield break;
        }

        //获取信息流
        try
        {
            _stream = _client.GetStream();
        }
        catch (Exception ex)
        {
            Info.Instance.Print("服务器连接失败：" + ex.Message, true);
            yield break;
        }
        if (_stream == null)
        {
            Info.Instance.Print("服务器连接失败：数据流为空", true);
            yield break;
        }

        _curState = ClientState.Connected;
        _messages = new Queue<byte[]>();
        Info.Instance.Print("服务器连接成功");

        //设置异步发送消息
        NetworkCoroutine.Instance.StartCoroutine(_Send());
        //设置异步接受消息
        NetworkCoroutine.Instance.StartCoroutine(_Receive());
        //设置退出事件
        NetworkCoroutine.Instance.SetQuitEvent(() => { _client.Close(); _curState = ClientState.None; });
    }

    private static IEnumerator _Send()
    {
        //持续发送消息
        while (_curState == ClientState.Connected)
        {
            _timer += Time.deltaTime;

            //有待发送消息
            if (_messages.Count > 0)
            {
                byte[] data = _messages.Dequeue();
                yield return _Write(data);
            }

            //心跳包机制 （每隔一段时间向服务器发送心跳包）
            if (_timer >= HEARTBEAT_TIME)
            {
                //如果没有收到上一次发送心跳包的回复
                if (!Received)
                {
                    _curState = ClientState.None;
                    Info.Instance.Print("心跳包接受失败，断开连接", true);
                    yield break;
                }
                _timer = 0;
                //封装消息
                byte[] data = _Pack(MessageType.HeartBeat);
                //发送消息
                yield return _Write(data);

                Debug.Log("已发送心跳包");
            }
            yield return null;//防止死循环
        }
    }
    private static IEnumerator _Receive()
    {
        //持续接受消息
        while (_curState == ClientState.Connected)
        {
            //解析数据包过程（客户端与服务器需要严格按照一定的协议指定数据包）
            byte[] data = new byte[4];

            int length; //消息长度
            MessageType type; //消息类型
            int receive = 0; //接受长度

            //异步读取
            IAsyncResult async = _stream.BeginRead(data, 0, data.Length, null, null);
            while (!async.IsCompleted)
            {
                yield return null;
            }
            //异常处理
            try
            {
                receive = _stream.EndRead(async);
            }
            catch (Exception ex)
            {
                _curState = ClientState.None;
                Info.Instance.Print("消息包头接受失败：" + ex.Message, true);
                yield break;
            }
            if (receive < data.Length)
            {
                _curState = ClientState.None;
                Info.Instance.Print("消息包头接受失败", true);
                yield break;
            }

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(stream, Encoding.UTF8); //utf-8 格式解析
                try
                {
                    length = reader.ReadInt16();
                    type = (MessageType)reader.ReadUInt16();
                }
                catch (Exception ex)
                {
                    _curState = ClientState.None;
                    Info.Instance.Print("消息包头接受失败：" + ex.Message, true);
                    yield break;
                }
            }

            //如果有包体
            if (length - 4 > 0)
            {
                data = new byte[length - 4];
                //异步读取
                async = _stream.BeginRead(data, 0, data.Length, null, null);
                while (!async.IsCompleted)
                {
                    yield return null;
                }
                //异常处理
                try
                {
                    receive = _stream.EndRead(async);
                }
                catch (Exception ex)
                {
                    _curState = ClientState.None;
                    Info.Instance.Print("消息包体接受失败：" + ex.Message, true);
                    yield break;
                }
                if (receive < data.Length)
                {
                    _curState = ClientState.None;
                    Info.Instance.Print("消息包体接受失败", true);
                    yield break;
                }
            }
            //没有包体
            else
            {
                data = new byte[0];
                receive = 0;
            }

            if (_callBacks.ContainsKey(type))
            {
                //执行回调时事件
                CallBack method = _callBacks[type];
                method(data);
            }
            else
            {
                Debug.Log("未注册该类型的回调事件");
            }
        }
    }

    private static IEnumerator _Write(byte[] data)
    {
        //如果服务器下线，客户端依然会继续发送消息
        if (_curState != ClientState.Connected || _stream == null)
        {
            Info.Instance.Print("连接失败，无法发送消息", true);
            yield break;
        }

        //异步发送消息
        IAsyncResult async = _stream.BeginWrite(data, 0, data.Length, null, null);
        while (!async.IsCompleted)
        {
            yield return null;
        }
        //异常处理
        try
        {
            _stream.EndWrite(async);
        }
        catch(Exception ex)
        {
            _curState = ClientState.None;
            Info.Instance.Print("发送消息失败：" + ex.Message, true);
        }
    }

    //连接服务器
    public static void Connect(string address = null, int port = 8848)
    {
        //连接上，不能重复连接
        if (_curState == ClientState.Connected)
        {
            Info.Instance.Print("服务器已经连接");
            return;
        }
        if(address == null)
            address = NetworkUtils.GetLocalIPv4();

        //获取失败则取消连接
        if (!IPAddress.TryParse(address, out _address))
        {
            Info.Instance.Print("IP地址错误，请重新尝试", true);
            return;
        }

        _port = port;
        //与服务器建立连接
        NetworkCoroutine.Instance.StartCoroutine(_Connect());//连接ip跟端口号成功不保证网络流建立成功
    }

    //注册消息回调事件
    public static void Register(MessageType type, CallBack method)
    {
        if (!_callBacks.ContainsKey(type))
            _callBacks.Add(type, method);
        else
            Debug.LogWarning("注册了相同的回调事件");
    }

    //加入消息队列
    public static void Enqueue(MessageType type, byte[] data = null)
    {
        //把数据封装
        byte[] bytes = _Pack(type, data);

        if(_curState == ClientState.Connected)
            //加入队列
            _messages.Enqueue(bytes);
    }

    //数据封装
    private static byte[] _Pack(MessageType type, byte[] data = null)
    {
        MessagePacker packer = new MessagePacker();
        if(data != null)
        {
            packer.Add((ushort)(4 + data.Length));//消息长度
            packer.Add((ushort)type);//消息类型
            packer.Add(data);//消息内容
        }
        else
        {
            packer.Add(4); //消息长度
            packer.Add((ushort)type);//消息内容
        }
        return packer.Package;
    }
}
