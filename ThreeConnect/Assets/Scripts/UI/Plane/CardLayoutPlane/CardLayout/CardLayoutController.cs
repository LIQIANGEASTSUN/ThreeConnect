using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayoutController
{
    private Dictionary<int, CardLayerData> _layerDic = new Dictionary<int, CardLayerData>();
    private int _layerCount = 0;

    public CardLayoutController()
    {

    }

    public void ReLayout()
    {
        _layerCount = Random.Range(2, 4);
        _layerDic.Clear();

        int layer = Random.Range(0, 1000) % 2;
        for (int i = 0; i < _layerCount; i++)
        {
            CardLayerType cardLayer = (CardLayerType)layer;
            ++layer;
            layer %= 2;

            CardLayerData cardLayerData = new CardLayerData(cardLayer);
            _layerDic[i] = cardLayerData;
        }

        while (TotalCard() % 3 != 0)
        {
            int key = Random.Range(0, 1000) % _layerCount;
            _layerDic[key].RandomRemove();
        }
    }

    private int TotalCard()
    {
        int count = 0;
        foreach(var kv in _layerDic)
        {
            count += kv.Value.Count();
        }
        return count;
    }

    public Dictionary<int, CardLayerData> LayerDic
    {
        get { return _layerDic; }
    }

    public int LayerCount
    {
        get { return _layerCount; }
    }

    public static void CardLayerRowCol(CardLayerType type, ref int row, ref int col)
    {
        row = 5;
        if (type == CardLayerType._5_8)
        {
            col = 8;
        }
        else if (type == CardLayerType._5_7)
        {
            col = 7;
        }
    }
}
