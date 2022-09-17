using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICardLayoutModel : IUIModel
{
    private IUIDataBase _data;
    private CardLayoutDataController _controller;

    public UICardLayoutModel()
    {
        _controller = new CardLayoutDataController();
    }

    public void Open(IUIDataBase data)
    {
        _data = data;

        Create();
    }

    public void Create()
    {
        _controller.ReLayout();
    }

    public CardLayoutDataController CardLayoutDataController
    {
        get { return _controller; }
    }


}
