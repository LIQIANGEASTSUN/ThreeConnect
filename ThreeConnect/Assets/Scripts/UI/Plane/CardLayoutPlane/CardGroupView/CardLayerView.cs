using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardLayerView
{
    private Transform _tr;
    private Transform _cloneTr;
    private CardLayoutDataController _cardLayoutDataController;
    private CardLayerData _layerData;
    private Dictionary<int, CardItem> _cardDic = new Dictionary<int, CardItem>();

    public CardLayerView(Transform tr, Transform cloneTr, CardLayoutDataController cardLayoutDataController, int layer)
    {
        _tr = tr;
        _cloneTr = cloneTr;
        _cardLayoutDataController = cardLayoutDataController;
        _layerData = cardLayoutDataController.GetLayerData(layer);
        Create();
    }

    private void Create()
    {
        IEnumerable<CardData> ie = _layerData.GetData();
        foreach (var data in ie)
        {
            GameObject go = GameObject.Instantiate(_cloneTr.gameObject);
            Transform itemTr = go.transform;
            itemTr.SetParent(_tr);
            itemTr.localScale = Vector3.one;
            itemTr.localRotation = Quaternion.identity;
            itemTr.localPosition = Vector3.zero;

            CardItem item = new CardItem(itemTr, data, CardType.CardLayout);
            int index = RowColToIndex(data.Row, data.Col);
            _cardDic.Add(index, item);
        }
    }

    private int RowColToIndex(int row, int col)
    {
        int index = row * _layerData.Col + col;
        return index;
    }

    public void Remove(int layer, int row, int col)
    {
        int index = RowColToIndex(row, col);
        CardItem cardItem = null;
        if (!_cardDic.TryGetValue(index, out cardItem))
        {
            return;
        }
        cardItem.Release();
        _cardDic.Remove(index);
        _cardLayoutDataController.Remove(layer, row, col);
    }

    public CardItem GetCardItem(int row, int col)
    {
        int index = RowColToIndex(row, col);
        CardItem cardItem = null;
        _cardDic.TryGetValue(index, out cardItem);
        return cardItem;
    }

    public void Release()
    {
        GameObject.Destroy(_tr.gameObject);
    }
}