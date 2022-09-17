using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class UIScrollBase
{
    protected LoopScrollView _loopScrollView;
    protected List<UIScrollItem> _itemList = new List<UIScrollItem>();
    protected Dictionary<int, UIScrollItem> _dic = new Dictionary<int, UIScrollItem>();
    protected Transform _cloneItem;
    protected int _totalCount;

    private Action<Transform, int> _refreshItemCallBack;

    public UIScrollBase(LoopScrollView loopScrollView)
    {
        _loopScrollView = loopScrollView;
        _cloneItem = _loopScrollView.ContentRect.GetChild(0);
    }

    public virtual void ItemCount(int count)
    {
        _totalCount = count;
        int min = 0;
        int max = 0;
        PageMinMax(0, ref min, ref max);
        Create( min, max);
    }

    public void SetRefreshCallBack(Action<Transform, int> callBack)
    {
        _refreshItemCallBack = callBack;
    }

    protected void Create(int min, int max)
    {
        min = Mathf.Max(0, min);
        max = Mathf.Min(_totalCount - 1, max);

        List<int> invalidKeyList = new List<int>();
        foreach(var kv in _dic)
        {
            int key = kv.Key;
            if (min > key || key > max)
            {
                invalidKeyList.Add(key);
            }
        }

        for (int i = min; i <= max; ++i)
        {
            UIScrollItem item = null;
            if (_dic.TryGetValue(i, out item))
            {
                continue;
            }

            if (invalidKeyList.Count > 0)
            {
                int key = invalidKeyList[invalidKeyList.Count - 1];
                invalidKeyList.RemoveAt(invalidKeyList.Count - 1);
                _dic.TryGetValue(key, out item);
                _dic.Remove(key);
            }
            else
            {
                item = new UIScrollItem();
                item._index = i;
                Transform tr = CreateItem();
                item.SetItemTr(tr);
            }

            _dic[i] = item;
            Vector2 position = CalculateItemPosition(i);
            item.SetPosition(position);
            _refreshItemCallBack?.Invoke(item._itemTr, i);
        }
    }

    protected abstract void CalculateContent(int count);

    protected Vector2 CalculateItemPosition(int index)
    {
        int row = 0;
        int col = 0;
        IndexToRowCol(index, ref row, ref col);

        Vector2 position = new Vector2(_loopScrollView._left, -_loopScrollView._top);
        position.x += (col + 0.5f) * _loopScrollView._cellSize.x + col * _loopScrollView._spacing.x;
        position.y -= (row + 0.5f) * _loopScrollView._cellSize.y + row * _loopScrollView._spacing.y;
        return position;
    }

    protected abstract void IndexToRowCol(int index, ref int row, ref int col);

    protected Transform CreateItem()
    {
        GameObject go = GameObject.Instantiate(_cloneItem.gameObject);
        Transform item = go.transform;
        item.SetParent(_loopScrollView.ContentRect.transform);
        item.localScale = Vector3.one;
        item.localRotation = Quaternion.identity;
        item.localPosition = Vector3.zero;
        SetActive(item.gameObject, true);
        return item;
    }

    protected void SetActive(GameObject go, bool value)
    {
        if (go.activeInHierarchy != value)
        {
            go.SetActive(value);
        }
    }

    public abstract void ScrollChange(Vector2 pos);

    public abstract void GoToIndex(int index);

    protected abstract void PageMinMax(float y, ref int min, ref int max);

    protected abstract void CurrentPageMinMax(ref int min, ref int max);

    public CellShowType GetCellShowType(int index)
    {
        int min = 0;
        int max = 0;
        CurrentPageMinMax(ref min, ref max);

        if (min <= index && index <= max)
        {
            return CellShowType.Center;
        }

        return GetCellShowType(index, min, max);
    }

    protected abstract CellShowType GetCellShowType(int index, int pageMin, int pageMax);

}
