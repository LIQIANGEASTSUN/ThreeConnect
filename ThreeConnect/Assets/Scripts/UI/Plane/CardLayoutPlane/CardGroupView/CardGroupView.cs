using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGroupView
{

    public Transform _tr;

    private Transform _groupTr;
    private Transform _group_5_8;
    private Transform _group_5_7;
    private Transform _cardClone;

    private List<CardLayerView> _layerList = new List<CardLayerView>();

    public CardGroupView(Transform tr)
    {
        _tr = tr;

        _groupTr = _tr.Find("Group");
        _group_5_8 = _groupTr.Find("CardGroup5_8");
        _group_5_7 = _groupTr.Find("CardGroup5_7");
        _cardClone = _groupTr.Find("Card");
    }

    public void CreateCard(UICardLayoutModel model)
    {
        foreach(var layer in _layerList)
        {
            layer.Release();
        }
        _layerList.Clear();

        CardLayoutController cardLayoutController = model.CardLayoutController;
        foreach(var kv in cardLayoutController.LayerDic)
        {
            CardLayerData layerData = kv.Value;
            Transform layerTr = CreateLayerTr(layerData.CardLayerType);
            CardLayerView cardLayerView = new CardLayerView(layerTr, cardLayoutController, layerData.Layer);
        }
    }

    private Transform CreateLayerTr(CardLayerType type)
    {
        Transform tr = null;
        if (type == CardLayerType._5_8)
        {
            tr = _group_5_8;
        }
        else if (type == CardLayerType._5_7)
        {
            tr = _group_5_7;
        }

        GameObject go = GameObject.Instantiate(tr.gameObject);
        Transform layerTr = go.transform;
        layerTr.SetParent(_groupTr);
        layerTr.localRotation = Quaternion.identity;
        layerTr.localPosition = Vector3.zero;
        layerTr.localScale = Vector3.one;
        return layerTr;
    }

}
