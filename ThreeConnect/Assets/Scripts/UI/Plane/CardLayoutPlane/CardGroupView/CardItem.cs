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
    private Transform _tr;
    private RectTransform _rectTransform;
    private TMP_Text _text;
    private CardData _cardData;
    private CardType _cardType;
    private bool _cardEnableFly = false;

    public CardItem(Transform itemTr, CardData cardData, CardType cardType)
    {
        _tr = itemTr;
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

        _rectTransform.anchoredPosition = Position();
    }

    private Vector2 Position()
    {
        Vector2 size = _rectTransform.sizeDelta;

        float x = size.x * (_cardData.Col + 0.5f);
        float y = size.y * (_cardData.Row + 0.5f) * -1;

        Vector2 position = new Vector2(x, y);
        return position;
    }

    private void OnClick()
    {
        if (!_cardEnableFly)
        {
            return;
        }
        GameNotifycation.GetInstance().Notify<Action<bool>>(ENUM_MSG_TYPE.MSG_CARD_ENABLE_FLY, CardEnableFly);
        Vector2 screenPoint = PositionConvert.UIPointToScreenPoint(_tr.position);
        GameNotifycation.GetInstance().Notify<CardData, Vector2>(ENUM_MSG_TYPE.MSG_CARD_FLY, _cardData, screenPoint);
    }

    private void CardEnableFly(bool enable)
    {
        _cardEnableFly = enable;
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

    public void Release()
    {
        GameObject.Destroy(_tr.gameObject);
    }

}
