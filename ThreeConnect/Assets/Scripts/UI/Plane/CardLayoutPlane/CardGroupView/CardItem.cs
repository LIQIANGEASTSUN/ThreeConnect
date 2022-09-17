using UnityEngine;
using TMPro;
using System;
public enum CardType
{
    /// <summary>
    /// 卡牌布局的卡
    /// </summary>
    CardLayout, 
    /// <summary>
    /// 卡槽里的卡
    /// </summary>
    Slot,
}

public class CardItem
{
    private RectTransform _parentRectTransform;
    private Transform _tr;
    private RectTransform _rectTransform;
    private TMP_Text _text;
    private CardData _cardData;
    private CardType _cardType;
    private int _instanceId;
    private AABB2D _aabb2D;

    private static int _NewInstanceId = 0;

    public CardItem(Transform itemTr, CardData cardData, CardType cardType)
    {
        _instanceId = ++_NewInstanceId;
        _tr = itemTr;
        _parentRectTransform = _tr.parent.GetComponent<RectTransform>();
        _cardData = cardData;
        _cardType = cardType;

        _rectTransform = itemTr.GetComponent<RectTransform>();
        _text = itemTr.Find("Text").GetComponent<TMP_Text>();
        _tr.gameObject.SetActive(true);
        _tr.name = string.Format("{0}_{1}", _cardData.Row, _cardData.Col);

        if (_cardType == CardType.CardLayout)
        {
            UIOnClik uIOnClik = _tr.Find("Bg").GetComponent<UIOnClik>();
            uIOnClik.AddClick(OnClick);
        }

        Refresh();
    }

    private void Refresh()
    {
        string str = string.Format("{0}", (char)_cardData.TableId);
        _text.text = str;

        if (_cardType == CardType.CardLayout)
        {
            _rectTransform.anchoredPosition = Position();
            CreateRect();
        }
    }

    private Vector2 Position()
    {
        //Vector2 size = _rectTransform.sizeDelta;

        //float x = size.x * (_cardData.Col + 0.5f);
        //float y = size.y * (_cardData.Row + 0.5f) * -1;

        //Vector2 position = new Vector2(x, y);
        //return position;

        Vector2 size = _rectTransform.sizeDelta;

        float x = size.x * (_cardData.Col + 0.5f) - _parentRectTransform.sizeDelta.x * 0.5f;
        float y = size.y * (_cardData.Row + 0.5f) - _parentRectTransform.sizeDelta.y * 0.5f;

        Vector2 position = new Vector2(x, y);
        return position;
    }

    private void OnClick()
    {
        Vector2 screenPoint = PositionConvert.UIPointToScreenPoint(_tr.position);
        GameNotifycation.GetInstance().Notify<CardData, Vector2>(ENUM_MSG_TYPE.MSG_CARD_ONCLICK, _cardData, screenPoint);
    }

    private void CreateRect()
    {
        Vector2 screenPoint = PositionConvert.UIPointToScreenPoint(_tr.position);
        Vector2 localPosition = PositionConvert.ScreenPointToUILocalPoint(_parentRectTransform, screenPoint);

        Vector2 min = localPosition - _rectTransform.sizeDelta * 0.5f;
        Vector2 max = localPosition + _rectTransform.sizeDelta * 0.5f;
        _aabb2D = new AABB2D(min, max);
    }

    public CardData CardData
    {
        get { return _cardData; }
    }

    public Transform Tr
    {
        get { return _tr; }
    }

    public RectTransform RectTransform
    {
        get { return _rectTransform; }
    }

    public Vector2 AnchoredPosition
    {
        get { return _rectTransform.anchoredPosition; }
        set { _rectTransform.anchoredPosition = value; }    
    }

    public int InstanceId
    {
        get { return _instanceId; }
    }

    public AABB2D AABB2D
    {
        get { return _aabb2D; }
    }

    public void Release()
    {
        GameObject.Destroy(_tr.gameObject);
    }

}
