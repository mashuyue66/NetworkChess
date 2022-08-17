namespace ShineEngine
{
    public class BaseQueue : BaseList
    {
        protected int _mark = 0;

        protected int _start = 0;

        protected int _end = 0;

        //��ȡ���ȱ��
        public int getMark()
        {
            return _mark;
        }

        public int getStart()
        {
            return _start;
        }

        public int getEnd()
        {
            return _end;
        }
    }
}