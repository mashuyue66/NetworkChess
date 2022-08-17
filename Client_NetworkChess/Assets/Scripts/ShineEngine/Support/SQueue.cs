using System;
using System.Collections;
using System.Collections.Generic;

namespace ShineEngine
{
    /// <summary>
    /// ����
    /// </summary>
    public class SQueue<V> : BaseQueue, IEnumerable<V>
    {

        private V[] _values;

        public SQueue()
        {
            init(_minSize);
        }

        public SQueue(int capacity)
        {
            init(countCapacity(capacity));
        }

        public V[] getValues()
        {
            return _values;
        }

        protected override void init(int capacity)
        {
            if (capacity < _minSize)
                capacity = _minSize;

            _capacity = capacity;

            _values = new V[capacity];
            _mark = capacity - 1;
        }

        protected override void remake(int capacity)
        {
            V[] oldArr = _values;
            init(capacity);

            if (_size != 0)
            {
                V[] values = _values;

                if(_start < _end)
                {
                    Array.Copy(oldArr, _start, values, 0, _end - _start);
                }
                else
                {
                    int d = oldArr.Length - _start;
                    Array.Copy(oldArr, _start, values, 0, d);
                    Array.Copy(oldArr, 0, values, d, _end);
                }
            }

            _start = 0;
            _end = _size;
        }

        public void add(V v)
        {
            this.offer(v);
        }

        public bool offer(V v)
        {
            if (_size == _values.Length)
                remake(_values.Length << 1);

            int end = _end;

            _values[end++] = v;

            if(end == _values.Length)
            {
                end = 0;
            }

            _end = end;

            ++_size;

            return true;
        }

        //��ͷ����
        public bool unshift(V v)
        {
            if (_size == _values.Length)
                remake(_values.Length << 1);

            int start = _start;

            if ((--start) < 0)
            {
                start = _values.Length - 1;
            }

            _values[start] = v;

            _start = start;

            ++_size;

            return true;
        }

        //�鿴��Ԫ��
        public V peek()
        {
            if(_size == 0)
                return default(V);

            return _values[_start];
        }

        //ȡ��
        public V poll()
        {
            if (_size == 0)
                return default(V);

            int start = _start;

            V v = _values[start];
            _values[start] = default(V);

            if(++start == _values.Length)
            {
                start = 0;
            }

            _start = start;

            --_size;

            return v;
        }

        //�鿴��Ԫ��
        public V tail()
        {
            if (_size == 0)
                return default(V);

            int index = _end == 0 ? _values.Length - 1 : _end;
            return _values[index];
        }

        //ȡ��ĩβԪ��
        public V pop()
        {
            if (_size == 0)
                return default(V);

            int index = _end == 0 ? _values.Length - 1 : _end - 1;
            V v = _values[index];
            _values[index] = default(V);

            _end = index;

            --_size;

            return v;
        }

        public V get(int index)
        {
            if(index >= _size)
                return default(V);

            return _values[(_start + index) & _mark];
        }

        //�Ƴ�ǰ��len
        public void remoceFront(int len)
        {
            if (isEmpty())
                return;

            if (len > _size)
            {
                Log.throwError("indexOutOfBound");
                return;
            }

            int last = _size - len;

            while(_size > last)
            {
                poll();
            }
        }

        //�Ƴ���  index ��ĩβ ����Ϊlen
        public void removeBack(int len)
        {
            if (isEmpty())
                return;

            if(len > _size)
            {
                Log.throwError("indexOutOfBound");
                return;
            }

            int last = _size - len;

            while(_size > last)
            {
                pop();
            }
        }

        public override void clear()
        {
            if (_size == 0)
                return;

            V[] values = _values;

            for(int i= values.Length-1; i>=0; i--)
            {
                values[i] = default(V);
            }

            _size = 0;
            _start = 0;
            _end = 0;
        }

        public SList<V> toList()
        {
            SList<V> list = new SList<V>(_size);

            if (_size == 0)
                return list;

            V[] rValues = list.getValues();

            V[] values = _values;

            //������
            if (_end > _start)
                Array.Copy(values, _start, rValues, 0, _end - _start);
            else
            {
                int d = values.Length - _start;

                Array.Copy(values, _start, rValues, 0, d);
                Array.Copy(values, 0, rValues, d, _end);
            }

            list.justSetSize(_size);

            return list;
        }

        public void forEach(Action<V> consumer)
        {
            if (_size == 0)
                return;

            V[] values = _values;

            //������
            if (_end > _start)
            {
                for (int i = _start, end = _end; i < end; ++i)
                {
                    consumer(values[i]);
                }
            }
            else
            {
                for (int i = _start, end = values.Length; i < end; ++i)
                {
                    consumer(values[i]);
                }

                for (int i = 0, end = _end; i < end; ++i)
                {
                    consumer(values[i]);
                }
            }
        }

        public ForEachIterator GetEnumerator()
        {
            return new ForEachIterator(this);
        }

        IEnumerator<V> IEnumerable<V>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ForEachIterator(this);
        }

        public struct ForEachIterator : IEnumerator<V>
        {
            private int _index;
            private int _tMark;
            private int _tEnd;
            private int _tSize;
            private V[] _tValues;

            private V _v;

            public ForEachIterator(SQueue<V> queue)
            {
                _index = queue._start;
                _tMark = queue._mark;
                _tEnd = queue._end;
                _tSize = queue._size;
                _tValues = queue._values;
                _v = default(V);
            }

            public void Reset()
            {

            }

            public void Dispose()
            {

            }

            //????????
            public bool MoveNext()
            {
                if (_tSize > 0 && _index != _tEnd)
                {
                    _v = _tValues[_index];
                    _index = (_index + 1) & _tMark;
                    return true;
                }

                _v = default(V);
                return false;
            }

            public V Current
            {
                get { return _v; }
            }

            object IEnumerator.Current
            {
                get { return _v; }
            }
        }
    }
}