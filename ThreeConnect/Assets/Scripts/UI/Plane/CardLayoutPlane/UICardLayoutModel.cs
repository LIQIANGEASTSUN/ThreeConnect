using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICardLayoutModel : IUIModel
{
    private IUIDataBase _data;
    private CardLayoutController _controller;

    public UICardLayoutModel()
    {
        _controller = new CardLayoutController();
    }

    public void Open(IUIDataBase data)
    {
        _data = data;
    }

    public void Create()
    {
        _controller.ReLayout();
    }

}
