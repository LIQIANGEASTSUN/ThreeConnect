using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardLayoutView : IUIView
{
    private Transform _tr;
    private IUIController _uiController;
    private UICardLayoutPlane _cardLayoutPlane;
    private UICardLayoutModel _cardLayoutModel;

    private CardGroupView _cardGroupView;

    public void Open(Transform tr, IUIController controller)
    {
        _tr = tr;
        _uiController = controller;
        _cardLayoutPlane = controller as UICardLayoutPlane;

        _cardGroupView = new CardGroupView(_tr);
        _cardLayoutPlane.CardFlyController.SetTr(_tr);
    }

    public void SetModel(UICardLayoutModel model)
    {
        _cardLayoutModel = model;
    }

    public void CreateCard()
    {
        _cardGroupView.CreateCard(_cardLayoutModel);
    }

    public void Close()
    {
        _cardGroupView.Close();
    }

    public CardGroupView CardGroupView
    {
        get { return _cardGroupView; }
    }


}
