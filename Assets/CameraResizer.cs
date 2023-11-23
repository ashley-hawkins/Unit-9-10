using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    void Start()
    {
    }
    public void DoUpdate(Vector2 res)
    {
        const int DesiredWidth = 28;
        cam.orthographicSize = DesiredWidth / 2 * (float)res.y / res.x;
    }
    // Update is called once per frame
    public void Update()
    {
        DoUpdate(new Vector2(Screen.width, Screen.height));
    }
}
