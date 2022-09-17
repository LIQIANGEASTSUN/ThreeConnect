using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIOnClik : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool _enter = false;
    private bool _down = false;

    private Action _onClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        _down = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_down && _enter)
        {
            _onClick?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _enter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _enter = false;
    }

    public void AddClick(Action clickCallBack)
    {
        _onClick = clickCallBack; 
    }

}
