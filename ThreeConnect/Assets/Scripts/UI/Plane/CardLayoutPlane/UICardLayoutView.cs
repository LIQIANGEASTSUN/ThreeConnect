using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardLayoutView : IUIView
{
    private Transform _tr;
    private IUIController _uiController;
    private UICardLayoutPlane _cardLayoutPlane;

    private Button _restarBtn;
    private Button _startBtn;

    public void Open(Transform tr, IUIController controller)
    {
        _tr = tr;
        _uiController = controller;
        _cardLayoutPlane = controller as UICardLayoutPlane;

        _restarBtn = _tr.Find("RestartBtn").GetComponent<Button>();
        _restarBtn.onClick.RemoveAllListeners();
        _restarBtn.onClick.AddListener(_cardLayoutPlane.RestartOnClick);

        _startBtn = _tr.Find("StartBtn").GetComponent<Button>();
        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(_cardLayoutPlane.StartOnClick);
    }

}
