namespace ShineEngine
{
    public class BaseHash : Base
    {
        protected const int _minCapacity = 4;

        protected int _version=0;

        //上个free序号
        protected int _lastFreeIndex = -1;

        //只清空size （前提是其他的数据自行清理了，只底层用）
        public override void justClearSize()
        {
            _size = 0;
            ++_version;
            _lastFreeIndex = -1;
        }

        //尺寸减1   系统用
        public void minusSize()
        {
            --_size;
        }

        public static int hashChar(char arg)
        {
            int h = arg * -1640531527;
            return h ^ h >> 10;
        }

        public static int hashLong(long arg)
        {
            long h = arg * -7046029254386353131L;
            h ^= h >> 32;
            return (int)(h ^ h >> 16);
        }

        public static int hashInt(int arg)
        {
            int h = arg * -1640531527;
            return h ^ h >> 16;
        }

        public static int hashObj(object obj)
        {
            int hash = obj.GetHashCode();
            return hash ^ hash >> 16;
        }

        //拷贝基础， 为了 clone 系列
        public void copyBase(BaseHash target)
        {
            _size = target._size;
            _capacity = target._capacity;
            _version = 0;
        }

        //清空 */
        public override void clear()
        {
            _size = 0;
            ++_version;
            _lastFreeIndex = -1;
        }

        public void reset()
        {
            if (_size == 0)
            {
                if (_capacity == _minCapacity)
                    return;
            }
            else
            {
                justClearSize();
            }

            init(_minCapacity);
        }

        //扩容
        public void ensureCapacity(int capacity)
        {
            if (capacity > _capacity)
            {
                rehash(countCapacity(capacity));
            }
        }

        //缩容
        public void shrink()
        {
            if (_size == 0)
            {
                if (_capacity == _minCapacity)
                    return;

                init(_minCapacity);
            }
            else
            {
                int capacity = countCapacity(_size);

                if (capacity < _capacity)
                {
                    rehash(capacity);
                }
            }
        }

        protected virtual void rehash(int size)
        {
            ++_version;
            _lastFreeIndex = -1;
        }

        protected void postInsertHook(int index)
        {
            ++_version;

            if(_lastFreeIndex >= 0 && _lastFreeIndex == index)
            {
                _lastFreeIndex = -1;
            }

            if ((++_size) > _capacity)
            {
                rehash(_capacity << 1);
            }
        }

        protected void postRemoveHook(int index)
        {
            --_size;
            ++_version;

            if (_lastFreeIndex >= 0 && index > _lastFreeIndex)
            {
                _lastFreeIndex=-1;
            }
        }

        //获取上个  自由index
        public int getLastFreeIndex()
        {
            if (isEmpty())
                return -1;

            if (_lastFreeIndex == -1)
                return _lastFreeIndex = toGetLastFreeIndex();

            return _lastFreeIndex;
        }

        protected virtual int toGetLastFreeIndex()
        {
            return -1;
        }

        protected void getLastFreeIndexError()
        {
            Log.errorLog("impossible");
        }
    }
}