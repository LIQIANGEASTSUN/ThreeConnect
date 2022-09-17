using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICardLayoutPlane : UIBasePlane
{
    private UICardLayoutView _view;
    private UICardLayoutModel _cardLayoutModel;

    public override void Init(UIPlaneType type)
    {
        base.Init(type);
        View = new UIMainView();
        _view = View as UICardLayoutView;

        Model = new UICardLayoutModel();
        _cardLayoutModel = Model as UICardLayoutModel;
    }

    public override void Open(IUIDataBase data)
    {
        base.Open(data);
        _cardLayoutModel.Open(data);
    }

    public void RestartOnClick()
    {

    }

    public void StartOnClick()
    {
        //UIManager.GetInstance().Open(UIPlaneType.Shop, null);
    }
}
