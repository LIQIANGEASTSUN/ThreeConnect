using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public int _layer;
    private int _row;
    private int _col;
    private int _tableId;
    public CardData(int layer, int row, int col)
    {
        _layer = layer;
        _row = row;
        _col = col;
    }

    public int Row
    {
        get { return _row; }
    }

    public int Col
    {
        get { return _col; }
    }

    public int TableId
    {
        get { return _tableId; }
        set { _tableId = value; }
    }

}

/// <summary>
/// 卡牌层类型
/// </summary>
public enum CardLayerType
{
    /// <summary>
    /// 5行 8列 的层
    /// </summary>
    _5_8 = 0,
    /// <summary>
    /// 5行 7列的层
    /// </summary>
    _5_7 = 1,
}

public class CardLayerData
{
    private int _layer;
    private CardLayerType _cardLayerType;
    private int _row;
    private int _col;
    private Dictionary<int, CardData> _cardDic = new Dictionary<int, CardData>();
    private List<int> _keyList = new List<int>();

    public CardLayerData(CardLayerType layerType, int layer)
    {
        _cardLayerType = layerType;
        _layer = layer;
        CardCreate(_layer);
    }

    public int Layer
    {
        get { return _layer; }
    }

    public CardLayerType CardLayerType 
    { 
        get { return _cardLayerType; } 
    }

    private void CardCreate(int layer)
    {
        CardLayoutDataController.CardLayerRowCol(CardLayerType, ref _row, ref _col);

        for (int i = 0; i < _row; ++i)
        {
            for (int j = 0; j < _col; ++j)
            {
                CreateItem(layer, i, j);
            }
        }
    }

    private int randomMax = 1000;
    private void CreateItem(int layer, int row, int col)
    {
        // 66.6% 的概率生成数据
        int value = Random.Range(0, randomMax) % 3;
        if (value == 0)
        {
            return;
        }

        CardData cardData = new CardData(layer, row, col);
        int index = RowColToIndex(row, col);
        _cardDic[index] = cardData;
        _keyList.Add(index);
    }

    public void IndexToRowCol(int index, ref int row, ref int col)
    {
        row = index / _col;
        col = index % _col;
    }

    public int RowColToIndex(int row, int col)
    {
        int index = row * _col + col;
        return index;
    }

    public int Count()
    {
        return _cardDic.Count;
    }

    public void Remove(int row, int col)
    {
        int index = RowColToIndex(row, col);
        Remove(index);
    }

    public void Remove(int index)
    {
        if (_cardDic.ContainsKey(index))
        {
            _cardDic.Remove(index);
            _keyList.Remove(index);
        }
    }

    public void RandomRemove()
    {
        if (_keyList.Count <= 0)
        {
            return;
        }
        int value = Random.Range(0, 1000) % _keyList.Count;
        int index = _keyList[value];
        Remove(index);
    }

    public IEnumerable<CardData> GetData()
    {
        foreach(var kv in _cardDic)
        {
            yield return kv.Value;
        }
    }

    public int Row
    {
        get { return _row; }
    }

    public int Col
    { 
        get { return _col; } 
    }
}
