using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 固定行/列
/// </summary>
public enum FixedRowColType
{
    FixedRow,
    FixedCol,
}

/// <summary>
/// Cell 子项显示类型
/// 在 ScrollView 显示区域的 上方、下方、左边、右边、中间(当前显示)
/// </summary>
public enum CellShowType
{
    /// <summary>
    /// 上方（当前不显示）
    /// </summary>
    Top,
    /// <summary>
    /// 下方（当前不显示）
    /// </summary>
    Bottom,
    /// <summary>
    /// 左边（当前不显示）
    /// </summary>
    Left,
    /// <summary>
    /// 右边（当前不显示）
    /// </summary>
    Right,
    /// <summary>
    /// 中间（当前显示）
    /// </summary>
    Center,
}

public class LoopScrollView : MonoBehaviour
{
    private UIScrollBase _uIScrollBase;
    private RectTransform _contentRect;
    public ScrollRect _scrollRect;
    private RectTransform _scrollTR;

    /// <summary>
    /// 子物体距离左侧距离
    /// </summary>
    public float _left;
    /// <summary>
    /// 子物体距离上方距离
    /// </summary>
    public float _top;
    /// <summary>
    /// 子物体尺寸
    /// </summary>
    public Vector2 _cellSize = new Vector2(100, 100);
    /// <summary>
    /// 子物体间隔
    /// </summary>
    public Vector2 _spacing = Vector2.zero;

    /// <summary>
    /// 固定行/列
    /// 竖直滑动选择固定列
    /// 水平滑动选择固定行
    /// </summary>
    public FixedRowColType _fixedRowCol = FixedRowColType.FixedCol;

    /// <summary>
    /// 每行/列固定的个数，最小为 1
    /// </summary>
    [Range(1, 100)]
    public int _fixedCount = 1;

    public void Init(Action<Transform, int> callBack)
    {
        _contentRect = GetComponent<RectTransform>();
        _scrollRect.onValueChanged.AddListener(ScrollChange);
        _scrollTR = _scrollRect.transform.GetComponent<RectTransform>();
        UIScrollBase.SetRefreshCallBack(callBack);
    }

    public void ItemCount(int count)
    {
        UIScrollBase.ItemCount(count);
    }

    private void ScrollChange(Vector2 pos)
    {
        UIScrollBase.ScrollChange(pos);
    }

    public RectTransform ContentRect {
        get { return _contentRect; }
    }

    public RectTransform ScrollTR
    {
        get { return _scrollTR; }
    }

    public void SetScrollRectPos(Vector2 position)
    {
        _scrollRect.normalizedPosition = position;
    }

    /// <summary>
    /// 跳转到第 index 个
    /// 横向的第 index 显示在最左侧
    /// 竖向的第 index 显示在最上方
    /// </summary>
    /// <param name="index"></param>
    public void GoToIndex(int index)
    {
        UIScrollBase.GoToIndex(index);
    }

    /// <summary>
    /// 获取某一个子物体显示类型
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CellShowType GetCellShowType(int index)
    {
        return UIScrollBase.GetCellShowType(index);
    }

    private UIScrollBase UIScrollBase
    {
        get
        {
            if (null == _uIScrollBase)
            {
                if (_fixedRowCol == FixedRowColType.FixedRow)
                {
                    _uIScrollBase = new UIScrollRow(this);
                }
                else
                {
                    _uIScrollBase = new UIScrollCol(this);
                }
            }

            return _uIScrollBase;
        }
    }

}
