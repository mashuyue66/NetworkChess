using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ShineEngine
{
    /// <summary>
    /// 字符串方法
    /// </summary>
    public class StringUtils
    {
        private static int[] _intSizeTable = new int[]
            { 9, 99, 999, 9999, 99999, 999999, 9999999, 99999999, 999999999, int.MaxValue};

        private static Regex _letterAndNumberReg = new Regex(@"(^[A-Za-z0-9]+$)");
        private static Regex _noneContentStrReg = new Regex("[^ ^\\n^\\r^\\t]+?");

        //正数  hashCode 
        public static int positiveHashCode(string str)
        {
            return Math.Abs(str.GetHashCode());
        }

        //字符串转 bool
        public static bool strToBoolean(string str)
        {
            if(str == null || str.isEmpty())
                return false;
            return str.Equals("true") || str.Equals("1");
        }

        //字符串是否是空
        public static bool isNullOrEmpty(string str)
        {
            return str == null || str.isEmpty();
        }

        //字符串是否是空的
        public static bool isNoneContent(string str)
        {
            return !_noneContentStrReg.Match(str).Success;
        }

        //字符串  转 bool
        public static string booleanToStr(bool value)
        {
            return value ? "1" : "0";
        }

        //返回机器字数目
        public static int getCharMachineNum(string str)
        {
            int re = 0;
            int len = str.Length;
            int code;

            for(int i = 0; i < len; ++i)
            {
                code = str[i];

                if (code >= 0 && code <= 255)
                    ++re;
                else
                    re += 2;
            }

            return re;
        }

        //字符是否为  0-9 
        public static bool isCharIsNumber(char ch)
        {
            return ch >= 48 && ch <= 57;
        }

        //对象组转字符串
        public static string objectsToString(object[] objs)
        {
            if (objs.Length == 0)
                return "";

            if (objs.Length == 1)
            {
                if (objs[0] == null)
                    return "null";
                else
                    return objs[0].ToString();
            }

            StringBuilder sb = StringBuilderPool.createForThread();

            writeObjectsToStringBuilder(sb, objs);

            return StringBuilderPool.releaseStrForThread(sb);
        }

        //对象组转字符串
        public static void writeObjectsToStringBuilder(StringBuilder sb, object[] objs)
        {
            for(int i = 0; i < objs.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(',');
                }

                if (objs[i] == null)
                {
                    sb.Append("null");
                }
                else
                {
                    if (objs[i] is Object[])
                    {
                        writeObjectsToStringBuilder(sb, objs[i] as object[]);
                    }
                    else
                    {
                        sb.Append(objs[i].ToString());
                    }
                }
            }
        }

        private static int intStrSize(int arg)
        {
            int arg0;

            for (arg0 = 0; arg > _intSizeTable[arg0]; ++arg0)
            {
                ;
            }

            return arg0 + 1;
        }

        //以固定位数写入数字
        public static void writeIntWei(StringBuilder sb, int num, int wei)
        {
            int size = intStrSize(num);

            int last = wei - size;

            for (int i = 0; i < last; ++i)
                sb.Append('0');

            sb.Append(num);
        }

        //md5hash
        public static string md5(string str)
        {
            return md5(Encoding.UTF8.GetBytes(str));
        }

        public static string md5(byte[] bytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < result.Length; i++)
            {
                sb.Append(Convert.ToString(result[i], 16));
            }

            return sb.ToString();
        }

        //转成几位小数
        public static string roundStrF(float value, int wei)
        {
            return value.ToString("f" + wei);
        }

        //转成几位小数
        public static string roundStrD(double value, int wei)
        {
            return value.ToString("f" + wei);
        }

        //转成2为小数  四舍五入
        public static string roundStrF2(float value)
        {
            return value.ToString("f2");
        }

        //转成2为小数  四舍五入
        public static string roundStrD2(double value)
        {
            return value.ToString("f2");
        }

        //转成2位小数(向下取整)
		public static string floorStrF2(float value)
		{
			return (Math.Floor(value * 100) / 100).ToString();
		}

		//转成2位小数(向下取整)
		public static string floorStrD2(double value)
		{
			return (Math.Floor(value * 100) / 100).ToString();
		}

        //转化自接到显示
        public static string toMBString(long num)
        {
            return toMBString(num, 2);
        }

        //转化自接到显示
        public static string toMBString(long num, int wei)
        {
            if (num < 1024)
                return num + "B";

            if (num < 1048576)
                return roundStrF(num / 1024f, wei) + "KB";

            if (num < 1073741824)
                return roundStrF(num / 1048576f, wei) + "MB";

            return roundStrF(num / 1073741824f, wei) + "GB";
        }

        //根据标记进行替换
        public static string replaceWithSign(string str, string[] args)
        {
            if(args == null)
                return str;

            if(str.IndexOf('$') == -1)
                return str;

            for (int i = 0; i < args.Length; i++)
            {
                str = str.Replace("$" + (i + 1), args[i]);
            }

            return str;
        }

        //查找一组字符串中，任意一个出现的位置
        public static int indexOf(string str, string[] arr, int fromIndex)
        {
            return indexOfEx(str, arr, fromIndex).index;
        }

        //查找一组字符串中，任意一个出现的位置
        private static IndexOfResult indexOfEx(string str, string[] arr, int fromIndex)
        {
            IndexOfResult result = new IndexOfResult();

            int temp;

            for(int i = 0;i < arr.Length; i++)
            {
                temp = str.IndexOf(arr[i], fromIndex);

                if(temp != -1)
                {
                    if(result.index == -1)
                    {
                        result.index = temp;
                        result.content = arr[i];
                    }
                    else
                    {
                        if(temp < result.index)
                        {
                            result.index = temp;
                            result.content = arr[i];
                        }
                    }
                }
            }

            return result;
        }

        //IndexOf的结果
        private class IndexOfResult
        {
            //找到的序号
            public int index = -1;

            //查询单元内容
            public string content = "";
        }

        //找到另个对应字符的位置  次数计数
        public static int getAnotherIndex(string str, string left, string right, int fromIndex)
        {
            int re= -1;
            int start = str.IndexOf(left, fromIndex);
            int lastIndex = start + left.Length;
            int num = 1;

            string[] arr = new string[] { left, right };

            while (true)
            {
                IndexOfResult temp = indexOfEx(str, arr, lastIndex);

                if(temp.index != -1)
                {
                    if (temp.content == left)
                        ++num;
                    else if (temp.content == right)
                        --num;


                    lastIndex = temp.index + temp.content.Length;

                    if(num == 0)
                    {
                        re = temp.index;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return re;
        }

        public static string trim(string str)
        {
            if(str ==null || str.isEmpty())
                return "";

            char c = str[0];

            int left = 0;
            int len = str.Length;

            while(left < len && str[left] == ' ')
            {
                ++left;
            }

            int right = len - 1;

            while (right >= 0 && str[right] == ' ')
            {
                --right;
            }

            if(left == 0 && right == (len - 1))
                return str;

            return  str.Substring(left, right - left + 1);
        }

        //判断字符串是否为数字跟字母
        public static bool isLetterAndNumber(string str)
        {
            return _letterAndNumberReg.IsMatch(str);
        }

        //获取双位数字，小于10 用0补齐
        public static string getDoubleNumStr(int num)
        {
            if (num < 10)
                return "0" + num;

            return num.ToString();
        }

        //去掉所有空格
        public static string trimAll(string str)
        {
            return Regex.Replace(str, @"\s", "");
        }

        //使用正则表达式将U+编码字符串转换为string字符串（包括中文、英文、数字、emoji表情）
        public static String replaceUnicodePlusToString(String str)
        {
            Regex regex = new Regex("U\\+[0-9a-fA-F]{4,5}");

            foreach(Match match in regex.Matches(str))
            {
                string matStr = match.Groups[0].Value;
                string utf32Str = char.ConvertFromUtf32(int.Parse(matStr.Split('+')[1], NumberStyles.HexNumber));
                str = str.Replace(matStr, utf32Str);
            }

            return str;
        }

        //string 字符串（包括中文、英文、数字、emoji表情) 转换为Unicode编码字符串
        public static String stringToUnicode(String s)
        {
            String str = "";

            char[] charArray = s.ToCharArray();

            for(int i = 0; i < charArray.Length; i++)
            {
                int ch = (int) charArray[i];
                if(ch > 255)
                    str += "\\u" + Convert.ToString(ch, 16);
                else
                    str += ch.ToString();
            }
            return str;
        }

        //切割字符串  快速
        public static string[] split(string str, string parten)
        {
            IntList dds = new IntList();

            int index = 0;

            int len = parten.Length;

            while (true)
            {
                index = str.IndexOf(parten, index);

                if(index != -1)
                {
                    dds.add(index);
                    index += len;
                }
                else
                {
                    break;
                }
            }

            if (dds.isEmpty())
                return new string[] { str };

            string[] reArgs = new string[dds.size() + 1];

            int s = 0;
            for(int i = 0; i < dds.size(); i++)
            {
                reArgs[i] = str.slice(s, index = dds.get(i));
                s = index + len;
            }

            reArgs[dds.size()] = str.slice(s);

            return reArgs;
        }

        //切割字符串  快速
        public static string[] split(string str, char parten)
        {
            IntList dds = new IntList();
            char cc;
            for (int i = 0, len = str.Length; i < len; i++)
            {
                cc = str[i];

                if (cc == parten)
                    dds.add(i);
            }

            if (dds.isEmpty())
                return new string[] { str };

            string[] reArgs = new string[dds.size() + 1];

            int s = 0;
            int index;
            for(int i= 0; i < dds.size(); i++)
            {
                reArgs[i] = str.slice(s, index = dds.get(i));
                s = index + 1;
            }

            reArgs[dds.size()] = str.slice(s);

            return reArgs;
        }
    }
}
