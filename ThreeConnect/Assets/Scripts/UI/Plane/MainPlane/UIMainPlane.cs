using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainPlane : UIBasePlane
{
    private UIMainView _mainView;
    private UIMainModel _mainModel;

    public override void Init(UIPlaneType type)
    {
        base.Init(type);
        View = new UIMainView();
        _mainView = View as UIMainView;

        Model = new UIMainModel();
        _mainModel = Model as UIMainModel;
    }

    public override void Open(IUIDataBase data)
    {
        base.Open(data);
    }

    public void RestartOnClick()
    {
        //_mainView.ShowRestart(false);
        GameNotifycation.GetInstance().Notify(ENUM_MSG_TYPE.MSG_REBUILD_CARD_LAYOUT);
    }

    public void StartOnClick()
    {
        _mainView.ShowStart(false);
        UIManager.GetInstance().Open(UIPlaneType.CardLayout, null);
    }

}
