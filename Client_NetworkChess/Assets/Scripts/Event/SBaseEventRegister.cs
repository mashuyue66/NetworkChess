using ShineEngine;
using System;

/// <summary>
/// �����¼�ע��
/// </summary>
public class SBaseEventRegister<T>
{
    private IntIntMap _indexToTypeDic = new IntIntMap();

    private IntObjectMap<IntObjectMap<BaseEventAction>> _listenerDic = new IntObjectMap<IntObjectMap<BaseEventAction>>();
    //���
    private int _index = 0;

    //��Ӽ���
    public int addListener(int type, Action listener)
    {
        BaseEventAction func = new BaseEventAction();
        func.func = listener;

        return addListener(type, func);
    }

    //��Ӽ���
    public int addListener(int type, Action<T> listener)
    {
        BaseEventAction func = new BaseEventAction();
        func.func2 = listener;

        return addListener(type, func);
    }

    //��Ӽ���
    public int addListener(int type, BaseEventAction func)
    {
        IntObjectMap<BaseEventAction> dic = _listenerDic.get(type);

        if(dic == null)
            _listenerDic.put(type, dic = new IntObjectMap<BaseEventAction>());

        int index = ++_index;

        dic.put(index, func);
        _indexToTypeDic.put(index, type);

        return index;
    }

    //�Ƴ�����
    public void removeListener(int type, int index)
    {
        IntObjectMap<BaseEventAction> dic = _listenerDic.get(type);

        if (dic == null)
            return;

        dic.remove(index);
        _indexToTypeDic.remove(index);
    }

    //�Ƴ�����ͨ�����
    public void removeListener(int index)
    {
        int type = _indexToTypeDic.get(index);

        if(type > 0)
            removeListener(type, index);
    }

    //ɾ�����м���
    public void removeAllListener()
    {
        foreach (var v in _listenerDic)
            v.clear();
    }

    //�ɷ���Ϣ
    public void dispatch(int type, T data = default(T))
    {
        IntObjectMap<BaseEventAction> dic = _listenerDic.get(type);

        if (dic == null)
            return;
        int[] keys = dic.getKeys();
        BaseEventAction[] values = dic.getValues();
        int fv = dic.getFreeValue();
        int k;
        int safeIndex = dic.getLastFreeIndex();

        for(int i = safeIndex - 1; i != safeIndex; --i)
        {
            if (i < 0)
                i = values.Length;
            else if((k = keys[i]) != fv)
            {
                values[i].execute(data);

                if (k != keys[i])
                    ++i;
            }
        }

        if(data != null && data is IEvt)
        {
            ((IEvt)data).clear();
        }
    }

    public class BaseEventAction
    {
        public Action func;

        public Action<T> func2;

        public void execute(T data)
        {
            if(func != null)
            {
                func();
                return;
            }

            if(func2 != null)
            {
                func2(data);
                return;
            }
        }
    }
}
