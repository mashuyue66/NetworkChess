using ShineEngine;
using System;

/// <summary>
/// UI逻辑体基类
/// </summary>
public class UILogicBase : SEventRegister
{
    private const int LoadState_None = 0;
    private const int LoadState_Preloading = 1;
    private const int LoadState_Creating = 2;
    private const int LoadState_Ready = 3;

    //逻辑 ID
    public int id = -1;

    //逻辑体组
    private IntObjectMap<UILogicBase> _logicDic = new IntObjectMap<UILogicBase>();

    //父逻辑体
    private UILogicBase _parent;

    //是否初始化好    （有资源才有）
    private bool _inited = false;
    //是否析构了
    private bool _disposed = true;

    //config
    //逻辑体配置   可以为空
    //protected UILogicConfig _logicConfig;
}