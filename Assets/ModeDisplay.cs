using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeDisplay : MonoBehaviour
{
    public MazeNavigationLogic mnl;
    TMPro.TextMeshProUGUI tmpro;
    // Start is called before the first frame update
    void Start()
    {
        tmpro = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmpro.text = mnl.ScatterMode ? "Mode: Scatter" : "Mode: Chase";
    }
}
