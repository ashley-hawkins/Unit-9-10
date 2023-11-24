using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeNavigationLogic : MonoBehaviour
{
    public Tilemap tm;
    public TileBase theTile;
    public GameObject thePacman;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        tm.SetTile(Vector3Int.zero, theTile);
        thePacman.transform.position = tm.layoutGrid.GetCellCenterWorld(new Vector3Int(7, -15));
    }
}
