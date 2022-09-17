using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardSlotController
{
    private Transform _parent;
    private RectTransform _parentRectTransform;
    private Transform _cardClone;
    private RectTransform _cardRectTransform;
    private List<CardSlotItem> _cardList = new List<CardSlotItem> ();
    private bool _needMove = false;
    private bool _cardIsInTopLayer = false;

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
        int count = 0;
        int leftTableId = -1;
        List<int> removeList = new List<int>();
        for (int i = 0; i < _cardList.Count; ++i)
        {
            CardSlotItem card = _cardList[i];
            int tableId = card._cardItem.CardData.TableId;
            if (leftTableId == tableId)
            {
                count++;
            }
            else
            {
                count = 1;
                leftTableId = tableId;
            }

            if (count == GameConstast.MergeMinCount)
            {
                CalculateRemove(i, leftTableId, removeList);
                count = 0;
                leftTableId = -1;
            }
        }

        _needMove = removeList.Count > 0;
        for (int i = removeList.Count - 1; i >= 0; --i)
        {
            int instanceId = removeList[i];
            CardSlotItem item = _cardList.Find((a) => { return a._cardItem.InstanceId == instanceId; });
            if (null != item)
            {
                item.BeMerge();
                _cardList.Remove(item);
            }
        }
        if (_needMove)
        {
            ReCalculatePosition(0);
        }
    }

    private void CalculateRemove(int index, int tableId, List<int> removeList)
    {
        for (int i = index; i >= 0; --i)
        {
            CardSlotItem card = _cardList[i];
            int id  = card._cardItem.CardData.TableId;
            if (id == tableId)
            {
                removeList.Add(card._cardItem.InstanceId);
            }
            else
            {
                break;
            }
        }
    }

    public void Clear()
    {
        _needMove = false;
        foreach (var card in _cardList)
        {
            card.Release();
        }
        _cardList.Clear();
    }

    public void Release()
    {
        UnRegisterEvent();
        Clear();
    }

    public void SetTr(Transform tr)
    {
        _parent = tr.Find("CardSlot/CardGroup");
        _parentRectTransform = _parent.GetComponent<RectTransform>();
        _cardClone = tr.Find("Card1");
        _cardRectTransform = _cardClone.GetComponent<RectTransform>();
    }

    private void CardClick(CardData cardData, Vector2 screenPoint)
    {
        if (_cardList.Count >= GameConstast.SlotMaxCount)
        {
            return;
        }
        GameNotifycation.GetInstance().Notify<CardData, Action<bool>>(ENUM_MSG_TYPE.MSG_CARD_IS_IN_TOP_LAYER, cardData, CardIsInTopLayer);
        if (!_cardIsInTopLayer)
        {
            return;
        }

        GameNotifycation.GetInstance().Notify<CardData>(ENUM_MSG_TYPE.MSG_CARD_FLY, cardData);

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
        for (int i = 0; i < _cardList.Count; i++)
        {
            CardItem cardItem = _cardList[i]._cardItem;
            if (cardItem.CardData.TableId == cardData.TableId)
            {
                insertIndex = i + 1;
                break;
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

    private void CardIsInTopLayer(bool result)
    {
        _cardIsInTopLayer = result;
    }

    private void RegisterEvent()
    {
        GameNotifycation.GetInstance().AddEventListener<CardData, Vector2>(ENUM_MSG_TYPE.MSG_CARD_ONCLICK, CardClick);
    }

    private void UnRegisterEvent()
    {
        GameNotifycation.GetInstance().RemoveEventListener<CardData, Vector2>(ENUM_MSG_TYPE.MSG_CARD_ONCLICK, CardClick);
    }
}

public class CardSlotItem
{
    private RectTransform _parentRectTransform;
    public CardItem _cardItem;
    private Vector2 _position;
    private Vector2 _moveStartPosition;
    private float _moveTime;
    private int _index;
    private const float MAX_MOVE_TIME = 0.5f;
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
            _moveTime = 0;
            _moveStartPosition = _cardItem.AnchoredPosition;
        }
    }

    public void Move()
    {
        _moveTime += Time.deltaTime;
        _cardItem.AnchoredPosition = Vector2.Lerp(_moveStartPosition, _position, _moveTime / MAX_MOVE_TIME);
    }

    public bool NeedMove()
    {
        Vector2 anchored = _cardItem.AnchoredPosition;
        return Mathf.Abs(_position.x - anchored.x) > 3
            || Mathf.Abs(_position.y - anchored.y) > 3;
    }

    public int Index
    {
        get { return _index; }
        set { _index = value; } 
    }

    public void BeMerge()
    {
        Release();
    }

    public void Release()
    {
        _cardItem.Release();
    }

}