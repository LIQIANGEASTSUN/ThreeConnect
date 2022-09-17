using UnityEngine;
using UnityEngine.UI;

public class TestLoop : MonoBehaviour
{
    private LoopScrollView _loopScrollView;
    public int goToIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _loopScrollView = GetComponent<LoopScrollView>();
        _loopScrollView.Init(RefreshItem);
        _loopScrollView.ItemCount(130);

        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _loopScrollView.GoToIndex(goToIndex);
        }
    }

    private void RefreshItem(Transform itemTr, int index)
    {
        Text text = itemTr.Find("Text").GetComponent<Text>();
        text.text = index.ToString();

        CellShowType cellShowType = _loopScrollView.GetCellShowType(50);
        Debug.LogError(cellShowType);
    }
}
