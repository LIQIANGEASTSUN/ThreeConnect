﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotController
{
    private Transform _parent;
    private RectTransform _parentRectTransform;
    private Transform _cardClone;
    private RectTransform _cardRectTransform;
    private List<CardSlotItem> _cardList = new List<CardSlotItem> ();
    private bool _needMove = false;

    public CardSlotController()
    {
        RegisterEvent();
    }

    public void Update()
    {
        MoveItem();
    }

    private void MoveItem()
    {
        if (!_needMove)
        {
            return;
        }

        bool hasNeedMove = false;
        foreach (var card in _cardList)
        {
            if (card.NeedMove())
            {
                card.Move();
                hasNeedMove = true;
            }
        }

        if (!hasNeedMove)
        {
            _needMove = false;
            CheckMerge();
        }
    }

    private void CheckMerge()
    {
        Debug.LogError("CheckMerge");
    }

    public void Release()
    {
        UnRegisterEvent();

        _needMove = false;
        foreach(var card in _cardList)
        {
            card.Release();
        }
        _cardList.Clear();
    }

    public void SetTr(Transform tr)
    {
        _parent = tr.Find("CardSlot/CardGroup");
        _parentRectTransform = _parent.GetComponent<RectTransform>();
        _cardClone = tr.Find("Card1");
        _cardRectTransform = _cardClone.GetComponent<RectTransform>();
    }

    private void CardFly(CardData cardData, Vector2 screenPoint)
    {
        _needMove = true;

        GameObject go = GameObject.Instantiate(_cardClone.gameObject);
        Transform cardTr = go.transform;
        cardTr.SetParent(_parent);
        cardTr.localScale = Vector3.one;
        cardTr.localPosition = Vector3.zero;
        cardTr.localRotation = Quaternion.identity;
        CardItem cardItem = new CardItem(cardTr, cardData, CardType.Slot);
        CardSlotItem cardSlotItem = new CardSlotItem(_parentRectTransform, cardItem, screenPoint);

        int insertIndex = SearchInsertIndex(cardData);
        _cardList.Insert(insertIndex, cardSlotItem);

        ReCalculatePosition(insertIndex);
    }

    private int SearchInsertIndex(CardData cardData)
    {
        int insertIndex = _cardList.Count;
        int count = 0;
        for (int i = 0; i < _cardList.Count; i++)
        {
            CardItem cardItem = _cardList[i]._cardItem;
            if (cardItem.CardData.TableId == cardData.TableId)
            {
                ++count;
            }
            else
            {
                count = 1;
            }
            if (count >= 2)
            {
                insertIndex = i + 1;
            }
        }
        insertIndex = Mathf.Clamp(insertIndex, 0, _cardList.Count);
        return insertIndex;
    }

    private void ReCalculatePosition(int fromIndex)
    {
        for (int i = fromIndex; i < _cardList.Count; ++i)
        {
            Vector2 position = Position(i);
            _cardList[i].SetPosition(position);
        }
    }

    private Vector2 Position(int index)
    {
        float x = (index + 0.5f) * _cardRectTransform.sizeDelta.x - _parentRectTransform.sizeDelta.x * 0.5f;
        //float y = -0.5f * _cardRectTransform.sizeDelta.y;
        return new Vector2(x, 0);
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

public class CardSlotItem
{
    private RectTransform _parentRectTransform;
    public CardItem _cardItem;
    private Vector2 _position;
    private Vector2 _moveDir;
    private float _speed = 300;

    public CardSlotItem(RectTransform parentRectTransform, CardItem cardItem, Vector2 screenPoint)
    {
        _parentRectTransform = parentRectTransform;
        _cardItem = cardItem;
        Vector2 localPoint = PositionConvert.ScreenPointToUILocalPoint(_parentRectTransform, screenPoint);
        _cardItem.AnchoredPosition = localPoint;
    }

    public void SetPosition(Vector2 position)
    {
        _position = position;
        if (NeedMove())
        {
            _moveDir = (_position - _cardItem.AnchoredPosition).normalized;
        }
    }

    public void Move()
    {
        Debug.LogError("Move:" + _moveDir);
        _cardItem.AnchoredPosition += _moveDir * _speed * Time.deltaTime;
    }

    public bool NeedMove()
    {
        Vector2 anchored = _cardItem.AnchoredPosition;
        return Mathf.Abs(_position.x - anchored.x) > 3
            || Mathf.Abs(_position.y - anchored.y) > 3;
    }

    public void Release()
    {
        _cardItem.Release();
    }

}