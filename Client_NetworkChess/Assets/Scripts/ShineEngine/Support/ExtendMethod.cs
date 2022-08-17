namespace ShineEngine
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    static class ExtendMethod
    {
        public static bool isEmpty(this string str)
        {
            return str.Length == 0;
        }

        //字符串拆分
        public static string slice(this string str, int startIndex)
        {
            return str.Substring(startIndex);
        }

        //字符串拆分
        public static string slice(this string str,int startIndex, int endIndex)
        {
            return str.Substring(startIndex, endIndex - startIndex);
        }
    }
}
