﻿using UnityEngine;
using Multiplay;

public class NetworkGameplay : MonoBehaviour
{
    //单例
    private NetworkGameplay() { }
    public static NetworkGameplay Instance { get; private set; }

    [SerializeField]
    private GameObject _blackChess;  //需要实例化的黑棋
    [SerializeField]
    private GameObject _whiteChess;  //需要实例化的白棋

    //锚点
    [SerializeField]
    private GameObject _leftTop;    //左上
    [SerializeField]
    private GameObject _leftBottom;    //左下
    [SerializeField]
    private GameObject _rightTop;     //右上

    private Vector2[,] _chessPos;     //储存棋子世界坐标
    private float _gridWidth;           //网格宽度
    private float _gridHeight;          //网格高度

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        _chessPos = new Vector2[15, 15];

        //把四个节点从世界坐标转成屏幕坐标
        Vector3 leftTop = _leftTop.transform.position;
        Vector3 leftBottom = _leftBottom.transform.position;
        Vector3 rightTop = _rightTop.transform.position;

        //初始化每一个格子  （一共14个） 的宽度和高度
        _gridWidth = (rightTop.x - leftTop.x) / 14;
        _gridHeight = (leftTop.y - leftBottom.y) / 14;

        //初始化每个下棋点的位置
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                _chessPos[i, j] = new Vector2(leftBottom.x + _gridWidth * i, leftBottom.y + _gridHeight * j);
            }
        }
    }

    //下棋
    public Vec2 PlayChess()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //如果用户点中棋盘
        if(Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("ChessBoard")))
        {
            //计算鼠标点击位置
            for(int i = 0; i < 15; i++)
            {
                for(int j = 0; j < 15; j++)
                {
                    //计算鼠标点击点与下棋点的距离  （只算x,y平面）
                    float distance = _Distance(hit.point, _chessPos[i, j]);
                    //鼠标点击
                    if (distance < (_gridWidth / 2))
                    {
                        return new Vec2(i, j);
                    }
                }
            }
        }

        //未点中棋盘
        return new Vec2(-1, -1);
    }

    //实例化棋子
    public void InstChess(Chess chess, Vec2 pos)
    {
        Vector2 vec2 = _chessPos[pos.X, pos.Y];
        //计算棋子坐标，棋子的z轴坐标不能与棋盘一致，
        //必须更靠近摄像机，否则， 可能与棋盘重叠，导致棋子不可见
        Vector3 chessPos = new Vector3(vec2.x, vec2.y, -1);

        if (chess == Chess.Black)
            Instantiate(_blackChess, chessPos, Quaternion.identity);
        else
            Instantiate(_whiteChess, chessPos, Quaternion.identity);
    }

    //计算两个Vector2 的距离
    private float _Distance(Vector2 a, Vector2 b)
    {
        Vector2 distance = b - a;
        return distance.magnitude;
    }
}