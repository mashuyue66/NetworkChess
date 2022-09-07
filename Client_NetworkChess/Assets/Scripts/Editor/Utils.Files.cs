using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public sealed partial class Utils
{
    //复制一个目录下的所有文件到另一个目录
    public static void CopyDirectory(string oldDirectory, string newDirectory, bool overwrite = true)
    {
        List<string> list = new List<string>();
        RecursivePath(oldDirectory, null, list);
        string[] pathArray = list.ToArray();
        for(int i = 0; i < pathArray.Length; i++)
        {
            string newPath = pathArray[i].Replace(oldDirectory, newDirectory);
            string s = Path.GetDirectoryName(newPath);
            if (!Directory.Exists(s))
            {
                Directory.CreateDirectory(s);
            }
            File.Copy(pathArray[i], newPath, overwrite);
        }
    }

    public static void MoveFile(string oldFilePath, string newFilePath, bool overwrite = true)
    {
        if(!File.Exists(oldFilePath) || oldFilePath == newFilePath)
            return;

        string s = Path.GetDirectoryName(newFilePath);
        if(!Directory.Exists(s))
            Directory.CreateDirectory(s);
        File.Copy(oldFilePath, newFilePath, overwrite);
        DeleteFile(oldFilePath);
    }

    public static string LoadTextFileByPath(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("path dont exists ! : " + path);
            return "";
        }

        StreamReader sr = File.OpenText(path);
        StringBuilder line = new StringBuilder();
        string tmp = "";
        while((tmp = sr.ReadLine()) != null)
            line.Append(tmp);
        sr.Close();
        sr.Dispose();

        return line.ToString();
    }

    public static byte[] LoadByteFileByPath(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("path dont exists ! : " + path);
            return null;
        }

        FileStream fs = new FileStream(path, FileMode.Open);

        byte[] bytes = new byte[fs.Length];

        fs.Read(bytes, 0, bytes.Length);
        fs.Close();

        return bytes;
    }

    //加载 txt  返回每一行数据
    public static string[] LoadTextFileLineByPath(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("path dont exists ! : " + path);
            return null;
        }

        StreamReader sr = File.OpenText(path);
        List<string> line = new List<string>();
        string tmp = "";
        while((tmp = sr.ReadLine()) != null)
            line.Add(tmp);

        sr.Close();
        sr.Dispose();

        return line.ToArray();
    }

    public static bool DeleteFile(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        try
        {
            if(fileInfo.Exists)
                fileInfo.Delete();
            Debug.Log("File Delete: " + path);
        }
        catch(Exception e)
        {
            Debug.LogError("File delete fail: " + path + "  ---:" + e);
            return false;
        }

        return true;
    }

    public static void DeleteDirectory(string path)
    {
        if(Directory.Exists(path))
            Directory.Delete(path, true);
    }

    //保存文件数据
    public static bool CreateTextFile(string path, string _data)
    {
        byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(_data);

        return CreateFile(path, bytes);
    }

    public static bool CreateFile(string path, byte[] _data)
    {
        if (string.IsNullOrEmpty(path))
            return false;
        string temp = Path.GetDirectoryName(path);
        if (!Directory.Exists(temp))
            Directory.CreateDirectory(temp);
        try
        {
            if(File.Exists(path))
                File.Delete(path);
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            stream.Write(_data, 0, _data.Length);
            stream.Close();

            Debug.Log("File written: " + path);
        }
        catch (Exception e)
        {
            Debug.LogError("File written fail: " + path + "  ---:" + e);
            return false;
        }

        return true;
    }

    public static IEnumerator LoadTxtFileIEnumerator(string path, Action<string> callback)
    {
        WWW www = new WWW(path);
        yield return www;

        string data = "";
        if (string.IsNullOrEmpty(www.error))
            data = www.text;
        if(callback != null)
            callback(data);
        yield return new WaitForEndOfFrame();
    }
}
