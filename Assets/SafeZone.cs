using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public Canvas c;
    public RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        print(Screen.safeArea);
        Vector2 anchorMin = Screen.safeArea.position / c.pixelRect.size;
        Vector2 anchorMax = (Screen.safeArea.position + Screen.safeArea.size) / c.pixelRect.size;
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }
}
