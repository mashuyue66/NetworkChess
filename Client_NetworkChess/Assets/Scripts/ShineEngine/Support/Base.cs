namespace ShineEngine
{
    public class Base
    {
		protected static int _minSize = 4;

		/** 容量 */
		protected int _capacity = 0;

		protected int _size = 0;

		protected int countCapacity(int capacity)
		{
			capacity = MathUtils.getPowerOf2(capacity);

			if (capacity < _minSize)
				capacity = _minSize;

			return capacity;
		}

		protected virtual void init(int capacity)
		{
			_capacity = capacity;
		}

		public virtual void clear()
		{
			_size = 0;
		}

		public int capacity()
		{
			return _capacity;
		}

		public int size()
		{
			return _size;
		}

		public bool isEmpty()
		{
			return _size == 0;
		}

		//只清空size(前提是其他数据自行清理了,只看懂底层的用) 
		public virtual void justClearSize()
		{
			Log.throwError("should be override");
		}

		public virtual void reset()
		{
			Log.throwError("should be override");
		}
	}
}
