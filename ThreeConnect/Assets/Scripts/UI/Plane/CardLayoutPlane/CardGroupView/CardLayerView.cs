using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayerView
{
    private Transform _tr;
    private CardLayoutController _cardLayoutController;
    private CardLayerData _layerData;
    public CardLayerView(Transform tr, CardLayoutController cardLayoutController, int layer)
    {
        _tr = tr;
        _cardLayoutController = cardLayoutController;
        _layerData = cardLayoutController.GetLayerData(layer);
        Create();
    }

    private void Create()
    {

    }

    public void Release()
    {
        GameObject.Destroy(_tr.gameObject);
    }

}
