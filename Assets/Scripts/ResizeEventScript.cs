using UnityEngine;
using UnityEngine.UI;
public class ResizeEventScript : MonoBehaviour
{
    public float lastScreenWidth = 0f;

    float ratio = 1.43f;
    GridLayoutGroup group;
    // Start is called before the first frame update
    void Start()
    {
        lastScreenWidth = Screen.width;
        group = GetComponent<GridLayoutGroup>();
    }

    void Update()
    {
        if (lastScreenWidth != Screen.width)
        {
            lastScreenWidth = Screen.width;
            ResizeEvent();
            //StartCoroutine("AdjustScale");
        }
    }
    public void ResizeEvent()
    {
        int width = Screen.width;
        int height = (int)(width * ratio);
        Vector2 cellSize = new Vector2(width, height);
        group.cellSize = cellSize;
        Screen.SetResolution(width, height, false);

    }

}
