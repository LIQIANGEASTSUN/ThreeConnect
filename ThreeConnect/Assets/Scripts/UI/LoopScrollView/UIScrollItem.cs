using UnityEngine;

public class UIScrollItem
{

    public int _index;
    public Transform _itemTr;
    public RectTransform _rect;

    public UIScrollItem()
    {

    }

    public void SetItemTr(Transform itemTr)
    {
        _itemTr = itemTr;
        _rect = itemTr.GetComponent<RectTransform>();
    }

    public void SetPosition(Vector2 position)
    {
        _rect.anchoredPosition = position;
    }

}
