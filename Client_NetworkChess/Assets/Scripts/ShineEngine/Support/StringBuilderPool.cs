using System;
using System.Text;
using System.Threading;

namespace ShineEngine
{
    /// <summary>
    /// stringBuilder池
    /// </summary>
    public class StringBuilderPool : ObjectPool<StringBuilder>
    {
        private static StringBuilderPool _instance = new StringBuilderPool();

        private static SMap<Thread, StringBuilderPool> _dic = new SMap<Thread, StringBuilderPool>();

        public StringBuilderPool() : base(()=>new StringBuilder())
        {

        }

        public override void back(StringBuilder obj)
        {
            if (!GlobalSetting.stringBuilderUsePool)
                return;
            if (obj.Length > GlobalSetting.stringBuilderPoolKeepSize)
                return;

            obj.Length = 0;

            base.back(obj);
        }

        private static StringBuilderPool getPoolByThread(Thread thread)
        {
            StringBuilderPool pool = _dic.get(thread);

            if(pool == null)
                _dic.put(thread, pool = new StringBuilderPool());
            return pool;
        }

        //取一个
        public static StringBuilder create()
        {
            if(!GlobalSetting.stringBuilderUsePool)
                return new StringBuilder();

            return _instance.getOne();
        }

        //退还一个
        public static void release(StringBuilder obj)
        {
            if (!GlobalSetting.stringBuilderUsePool)
                return;

            _instance.back(obj);
        }

        //退还一个  并返回该 stringbuilder 的字符串
        public static string releaseStr(StringBuilder obj)
        {
            string v = obj.ToString();

            if (GlobalSetting.stringBuilderUsePool)
                _instance.back(obj);

            return v;
        }

        //取出一个
        public static StringBuilder createForThread()
        {
            if (!GlobalSetting.stringBuilderUsePool)
                return new StringBuilder();

            return getPoolByThread(Thread.CurrentThread).getOne();
        }

        //退还一个
        public static void releaseForThread(StringBuilder obj)
        {
            if (!GlobalSetting.stringBuilderUsePool)
                return;

            getPoolByThread(Thread.CurrentThread).back(obj);
        }

        //退还一个  并返回该 stringbuilder 的字符串
        public static string releaseStrForThread(StringBuilder obj)
        {
            string v = obj.ToString();

            if (GlobalSetting.stringBuilderUsePool)
                getPoolByThread(Thread.CurrentThread).back(obj);

            return v;
        }
    }
}
