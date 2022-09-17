using UnityEngine;

/*
offsetMin ： 对应Left、Bottom
offsetMax ： 对应Right、Top
*/

/// <summary>
/// 垂直滚动
/// </summary>
public class UIScrollRow : UIScrollBase
{
    /// <summary>
    /// 总列数
    /// </summary>
    private int _totalCol;
    /// <summary>
    /// 显示区域能显示几列
    /// </summary>
    private float _pageCol;
    public UIScrollRow(LoopScrollView loopScrollView) : base(loopScrollView)
    {

    }

    public override void ItemCount(int count)
    {
        CalculateContent(count);
        base.ItemCount(count);
        _loopScrollView.SetScrollRectPos(new Vector2(0, 0));
    }

    protected override void CalculateContent(int count)
    {
        _totalCol = TotalCol(count);
        PageCol();

        float width = _loopScrollView._cellSize.x * _totalCol + _loopScrollView._spacing.x * (_totalCol - 1);
        float height = _loopScrollView.ScrollTR.sizeDelta.y;

        Vector2 sizeDelta = new Vector2(width, height);
        _loopScrollView.ContentRect.anchorMin = new Vector2(0, 0);
        _loopScrollView.ContentRect.anchorMax = new Vector2(0, 1);
        _loopScrollView.ContentRect.pivot = new Vector2(0, 1);
        _loopScrollView.ContentRect.sizeDelta = sizeDelta;
        _loopScrollView.ContentRect.anchoredPosition3D = new Vector3(0, 0, 0);

        Vector2 offsetMin = _loopScrollView.ContentRect.anchorMin;
        offsetMin.y = 0;
        _loopScrollView.ContentRect.offsetMin = offsetMin;

        Vector2 offsetMax = _loopScrollView.ContentRect.offsetMax;
        offsetMax.y = 0;
        _loopScrollView.ContentRect.offsetMax = offsetMax;
    }

    private int TotalCol(int count)
    {
        int totalCol = Mathf.CeilToInt(count * 1.0f / _loopScrollView._fixedCount);
        return totalCol;
    }

    protected override void IndexToRowCol(int index, ref int row, ref int col)
    {
        row = index % _loopScrollView._fixedCount;
        col = index / _loopScrollView._fixedCount;
    }

    private void PageCol()
    {
        _pageCol = _loopScrollView.ScrollTR.sizeDelta.x / (_loopScrollView._cellSize.x + _loopScrollView._spacing.x);
    }

    public override void ScrollChange(Vector2 pos)
    {
        int min = 0;
        int max = 0;
        CurrentPageMinMax(ref min, ref max);
        Create(min, max);
    }

    private float OffsetXToCol(float x)
    {
        float col = (x) / (_loopScrollView._cellSize.x + _loopScrollView._spacing.x);
        col *= -1;
        return col;
    }

    public override void GoToIndex(int index)
    {
        int row = 0;
        int col = 0;
        IndexToRowCol(index, ref row, ref col);

        float value = col * 1.0f / (_totalCol - _pageCol);
        value = Mathf.Clamp01(value);
        Vector2 position = new Vector2(value, 0);
        _loopScrollView.SetScrollRectPos(position);
    }

    protected override void PageMinMax(float x, ref int min, ref int max)
    {
        float endX = x - _loopScrollView.ScrollTR.sizeDelta.x;
        float startCol = OffsetXToCol(x);
        float endCol = OffsetXToCol(endX);

        startCol = Mathf.FloorToInt(startCol);
        endCol = Mathf.CeilToInt(endCol);

        min = Mathf.FloorToInt(startCol * _loopScrollView._fixedCount);
        max = Mathf.CeilToInt(endCol * _loopScrollView._fixedCount) - 1;
    }

    protected override void CurrentPageMinMax(ref int min, ref int max)
    {
        float startX = _loopScrollView.ContentRect.anchoredPosition.x;
        PageMinMax(startX, ref min, ref max);
    }

    protected override CellShowType GetCellShowType(int index, int pageMin, int pageMax)
    {
        if (index < pageMin)
        {
            return CellShowType.Left;
        }
        else
        {
            return CellShowType.Right;
        }
    }

}
