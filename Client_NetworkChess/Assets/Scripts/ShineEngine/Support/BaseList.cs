namespace ShineEngine
{
    public class BaseList : Base
    {
        //清空 + 缩容
        public override void reset()
        {
			if (_size == 0)
			{
				if (_capacity <= _minSize)
					return;
			}
			else
			{
				_size = 0;
			}

			init(0);
		}

		protected virtual void remake(int size)
		{
			Log.throwError("should be override");
		}

		// 扩容 
		public void ensureCapacity(int capacity)
		{
			if (capacity > _capacity)
			{
				remake(countCapacity(capacity));
			}
		}

		//缩容
		public void shrink()
		{
			if (_size == 0)
			{
				if (_capacity <= _minSize)
					return;

				init(0);
			}
			else
			{
				int capacity = countCapacity(_size);

				if (capacity < _capacity)
				{
					remake(capacity);
				}
			}
		}

		protected void addCapacity()
		{
			if (_capacity == 0)
				init(_minSize);
			else if (_size == _capacity)
				remake(_capacity << 1);
		}

		protected void addCapacity(int n)
		{
			if (_capacity == 0)
				init(_minSize);
			else if (_size + n > _capacity)
				remake(_capacity << 1);
		}

		public override void justClearSize()
		{
			_size = 0;
		}

		public void justSetSize(int value)
		{
			_size = value;
		}
	}
}
