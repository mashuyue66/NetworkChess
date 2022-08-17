using System;

namespace ShineEngine 
{
    /// <summary>
    /// 对象池 线程非安全
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T>
    {
        //是否开启
        protected bool _enable = true;
        //入池是否需要执行clear
        private bool _needClear = true;
        //列表
        private SQueue<T> _queue;
        //最大尺寸
        private int _maxSize;

        private Func<T> _createFunc;

        private SSet<T> _checkSet;

        private SMap<T, string> _callStackDic;

        //析构函数  
        private Action<T> _releaseFunc;

        public ObjectPool(Func<T> createFunc) : this(createFunc, GlobalSetting.defaultPoolSize)
        {

        }

        public ObjectPool(Func<T> createFunc, int size)
        {
            _maxSize = size;

            _queue = new SQueue<T>();

            _createFunc = createFunc;

            if (GlobalSetting.openCheck)
            {
                _checkSet = new SSet<T>();
                _callStackDic = new SMap<T, string>();
            }

            if(!GlobalSetting.useObjectPool)
                _enable = false;
        }

        public void setEnable(bool value)
        {
            if (!GlobalSetting.useObjectPool)
                _enable = false;
            else
                _enable = value;
        }

        public void setNeedClear(bool value)
        {
            _needClear = value;
        }

        //设置析构回调
        public void setReleaseFunc(Action<T> func)
        {
            _releaseFunc = func;
        }

        //清空
        public void clear()
        {
            if(_queue.isEmpty())
                return;

            foreach (T item in _queue)
            {
                if(_releaseFunc != null)
                    _releaseFunc(item);
            }

            _queue.clear();

            if (GlobalSetting.openCheck)
            {
                _checkSet.clear();
                _callStackDic.clear();
            }
        }

        //取出一个
        public virtual T getOne()
        {
            if (!_queue.isEmpty())
            {
                T obj = _queue.pop();

                if (GlobalSetting.openCheck)
                {
                    _checkSet.remove(obj);
                    _callStackDic.remove(obj);
                }

                return obj;
            }
            else
            {
                return _createFunc();
            }
        }

        //放回一个
        public virtual void back(T obj)
        {
            if (obj == null)
            {
                Log.throwError("对象池添加空对象");
                return;
            }

            if (!_enable || _queue.size() >= _maxSize)
            {
                if (_releaseFunc != null)
                {
                    _releaseFunc(obj);
                }

                return;
            }

            if (_needClear)
            {
                if( obj is IPoolObject)
                {
                    ((IPoolObject)obj).clear();
                }
            }

            if (GlobalSetting.openCheck)
            {
                if (_checkSet.contains(obj))
                {
                    Log.print("上次调用", _callStackDic.get(obj));
                    Log.throwError("对象池重复添加!", obj);
                    return;
                }
            }
        }
    }
}