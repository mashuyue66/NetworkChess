using System;

namespace ShineEngine 
{
    /// <summary>
    /// ����� �̷߳ǰ�ȫ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T>
    {
        //�Ƿ���
        protected bool _enable = true;
        //����Ƿ���Ҫִ��clear
        private bool _needClear = true;
        //�б�
        private SQueue<T> _queue;
        //���ߴ�
        private int _maxSize;

        private Func<T> _createFunc;

        private SSet<T> _checkSet;

        private SMap<T, string> _callStackDic;

        //��������  
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

        //���������ص�
        public void setReleaseFunc(Action<T> func)
        {
            _releaseFunc = func;
        }

        //���
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

        //ȡ��һ��
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

        //�Ż�һ��
        public virtual void back(T obj)
        {
            if (obj == null)
            {
                Log.throwError("�������ӿն���");
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
                    Log.print("�ϴε���", _callStackDic.get(obj));
                    Log.throwError("������ظ����!", obj);
                    return;
                }
            }
        }
    }
}