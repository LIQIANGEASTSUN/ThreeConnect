using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主界面
/// </summary>
public class UIMainView : IUIView
{
    private Transform _tr;
    private IUIController _uiController;
    private UIMainPlane UIMainPlane;

    private Button _restarBtn;
    private Button _startBtn;

    public void Open(Transform tr, IUIController controller)
    {
        _tr = tr;
        _uiController = controller;
        UIMainPlane = controller as UIMainPlane;

        _restarBtn = _tr.Find("RestartBtn").GetComponent<Button>();
        _restarBtn.onClick.RemoveAllListeners();
        _restarBtn.onClick.AddListener(UIMainPlane.RestartOnClick);

        _startBtn = _tr.Find("StartBtn").GetComponent<Button>();
        _startBtn.onClick.RemoveAllListeners();
        _startBtn.onClick.AddListener(UIMainPlane.StartOnClick);
    }

}
