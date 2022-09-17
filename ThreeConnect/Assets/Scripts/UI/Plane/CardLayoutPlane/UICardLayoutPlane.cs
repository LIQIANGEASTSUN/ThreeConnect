using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICardLayoutPlane : UIBasePlane
{
    private UICardLayoutView _view;
    private UICardLayoutModel _cardLayoutModel;
    private CardSlotController _cardSlotController;

    public override void Init(UIPlaneType type)
    {
        base.Init(type);
        View = new UICardLayoutView();
        _view = View as UICardLayoutView;

        Model = new UICardLayoutModel();
        _cardLayoutModel = Model as UICardLayoutModel;
    }

    public override void Open(IUIDataBase data)
    {
        base.Open(data);
        RegisterEvent();
        _view.SetModel(_cardLayoutModel);
        CreateCard();
    }

    public override void Update()
    {
        base.Update();
        _cardSlotController.Update();
    }

    private void CreateCard()
    {
        _view.CreateCard();
    }

    public override void Close()
    {
        base.Close();
        UnRegisterEvent();
        _cardSlotController.Release();
    }

    private void ReBuildCardLayout()
    {
        _cardLayoutModel.Create();
        CreateCard();
    }

    public CardSlotController CardFlyController
    {
        get {
            if (null == _cardSlotController)
            {
                _cardSlotController = new CardSlotController();
            }
            return _cardSlotController;
        }
    }

    private void RegisterEvent()
    {
        GameNotifycation.GetInstance().AddEventListener(ENUM_MSG_TYPE.MSG_REBUILD_CARD_LAYOUT, ReBuildCardLayout);
    }

    private void UnRegisterEvent()
    {
        GameNotifycation.GetInstance().RemoveEventListener(ENUM_MSG_TYPE.MSG_REBUILD_CARD_LAYOUT, ReBuildCardLayout);
    }

}
