using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.GetInstance().Open(UIPlaneType.Main, null);
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.GetInstance().Update();

        if (Input.GetKeyDown(KeyCode.A))
        {
            UIManager.GetInstance().OpenOrRefresh(UIPlaneType.Main, null);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //UIManager.GetInstance().Close(UIPlaneType.Shop);
        }
    }
}
