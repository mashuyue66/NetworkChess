using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public sealed partial class Utils
{
    private static bool CanBuild(string filePath, Dictionary<string, string> filters)
    {
        if (filePath.Contains(".meta") || filePath.Contains(".DS_Store") || filePath.Contains("~")) return false;
        if (filters == null) return true;

        string ext = Path.GetExtension(filePath);
        foreach(KeyValuePair<string, string> filter in filters)
        {
            if(filePath.IndexOf(filter.Key) > -1)
            {
                if (ext != "" && filter.Value.IndexOf(ext) > -1)
                {
                    EditorUtility.DisplayProgressBar("读取目录", string.Format("正在读取{0}", Path.GetFileName(filePath)), 100);
                    return true;
                }
                else
                    return false;
            }
        }
        return true;
    }

    //遍历目录及子目录
    public static void RecursivePath(string path, Dictionary<string, string> filter, List<string> fileList)
    {
        if (Directory.Exists(path))
        {
            string[] filePaths = Directory.GetFiles(path);
            string[] paths = Directory.GetDirectories(path);
            foreach (string filePath in filePaths)
                if (CanBuild(filePath, filter))
                    fileList.Add(filePath);
            foreach (string dir in paths)
                RecursivePath(dir, filter, fileList);
        }
        EditorUtility.ClearProgressBar();
    }

    //网络可用
    public static bool NetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    //是否无线
    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }

    //验证电话号码
    public static bool IsTelephone(string telephone)
    {
        return Regex.IsMatch(telephone, @"^(\d{3,4}-)?\d{6,8}$");
    }
    
    //验证手机号码
    public static bool IsHandset(string handset)
    {
        return Regex.IsMatch(handset, @"^[1]+[3,5]+\d{9}");
    }

    //验证身份证号
    public static bool IsIDcard(string idcard)
    {
        return Regex.IsMatch(idcard, @"(^\d{18}$)|(^\d{15}$)");
    }

    //验证输入为数字
    public static bool IsNumber(string number)
    {
        return Regex.IsMatch(number, @"^[0-9]*$");
    }

    //验证邮编
    public static bool IsPostalcode(string postalcode)
    {
        return Regex.IsMatch(postalcode, @"^\d{6}$");
    }

    //验证邮箱
    public static bool IsEmail(string email)
    {
        return Regex.IsMatch(email, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
    }
}