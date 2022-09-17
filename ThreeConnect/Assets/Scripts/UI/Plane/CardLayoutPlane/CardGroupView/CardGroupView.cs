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

    private Dictionary<int, CardLayerView> _layerDic = new Dictionary<int, CardLayerView>();

    public CardGroupView(Transform tr)
    {
        _tr = tr;

        _groupTr = _tr.Find("Group");
        _group_5_8 = _groupTr.Find("CardGroup5_8");
        _group_5_7 = _groupTr.Find("CardGroup5_7");
        _cardClone = _tr.Find("Card");

        RegisterEvent();
    }

    public void CreateCard(UICardLayoutModel model)
    {
        foreach(var kv in _layerDic)
        {
            kv.Value.Release();
        }
        _layerDic.Clear();

        CardLayoutDataController cardLayoutDataController = model.CardLayoutDataController;
        foreach(var kv in cardLayoutDataController.LayerDic)
        {
            CardLayerData layerData = kv.Value;
            Transform layerTr = CreateLayerTr(layerData.CardLayerType);
            CardLayerView cardLayerView = new CardLayerView(layerTr, _cardClone, cardLayoutDataController, layerData.Layer);
            _layerDic[kv.Key] = cardLayerView;
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
        layerTr.gameObject.SetActive(true);
        return layerTr;
    }

    private void CardFly(CardData data, Vector2 position)
    {
        CardLayerView cardLayerView = null;
        if (!_layerDic.TryGetValue(data._layer, out cardLayerView))
        {
            return;
        }
        cardLayerView.Remove(data._layer, data.Row, data.Col);
    }

    public void Close()
    {
        UnRegisterEvent();
    }

    private void RegisterEvent()
    {
        GameNotifycation.GetInstance().AddEventListener<CardData, Vector2>(ENUM_MSG_TYPE.MSG_CARD_FLY, CardFly);
    }

    private void UnRegisterEvent()
    {
        GameNotifycation.GetInstance().RemoveEventListener<CardData, Vector2>(ENUM_MSG_TYPE.MSG_CARD_FLY, CardFly);
    }

}
