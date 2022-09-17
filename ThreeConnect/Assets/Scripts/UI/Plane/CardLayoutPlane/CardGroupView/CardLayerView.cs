using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardLayerView
{
    private Transform _tr;
    private Transform _cloneTr;
    private CardLayoutController _cardLayoutController;
    private CardLayerData _layerData;
    public CardLayerView(Transform tr, Transform cloneTr, CardLayoutController cardLayoutController, int layer)
    {
        _tr = tr;
        _cloneTr = cloneTr;
        _cardLayoutController = cardLayoutController;
        _layerData = cardLayoutController.GetLayerData(layer);
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
        }
    }

    public void Release()
    {
        GameObject.Destroy(_tr.gameObject);
    }
}