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

            CardLayerData cardLayerData = new CardLayerData(cardLayer, i);
            _layerDic[i] = cardLayerData;
        }

        BeDividedExactlyBy3();
        CardAssignment();
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

    // 将数据削减为能被3整除
    private void BeDividedExactlyBy3()
    {
        while (TotalCard() % 3 != 0)
        {
            int key = Random.Range(0, 1000) % _layerCount;
            _layerDic[key].RandomRemove();
        }
    }

    // Card 赋值
    private void CardAssignment()
    {
        List<int> tableList = new List<int>();
        int count = TotalCard();
        count /= 3;
        for (int i = 0; i < count; ++i)
        {
            int tableId = ((char)'A') + i % 8;
            tableList.Add(tableId);
            tableList.Add(tableId);
            tableList.Add(tableId);
        }

        foreach(var kv in _layerDic)
        {
            CreateAssignment(tableList, kv.Value);
        }
    }

    private void CreateAssignment(List<int> tableList, CardLayerData cardLayerData)
    {
        IEnumerable<CardData> ie = cardLayerData.GetData();
        foreach(var card in ie)
        {
            int index = Random.Range(0, 1000) % tableList.Count;
            card.TableId = tableList[index];
            tableList.RemoveAt(index);
        }
    }

    public Dictionary<int, CardLayerData> LayerDic
    {
        get { return _layerDic; }
    }

    public CardLayerData GetLayerData(int layer)
    {
        CardLayerData data = null;
        LayerDic.TryGetValue(layer, out data);
        return data;
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
