using System;

namespace ShineEngine
{
    /// <summary>
    /// 方法队列
    /// </summary>
    public class SFuncQueue : SBatchQueue<Action>
    { 
        public SFuncQueue() : base(runFunc)
        {

        }

        private static void runFunc(Action func)
        {
            try
            {
                func();
            }
            catch(Exception e)
            {
                Log.errorLogForIO(e);
            }
        }
    }
}
