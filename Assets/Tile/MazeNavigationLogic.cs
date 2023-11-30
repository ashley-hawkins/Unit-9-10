using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeNavigationLogic : MonoBehaviour
{
    public Tilemap tm;
    public TileBase theTile;
    public TileBase wallTile;
    public GameObject thePacman;
    public Animator theAnim;
    // Start is called before the first frame update
    Vector2Int pacmanVelocity = Vector2Int.left;
    void Start()
    {
        Application.targetFrameRate = 60;
        tm.SetTile(Vector3Int.zero, theTile);
        thePacman.transform.position = tm.layoutGrid.GetCellCenterWorld(new Vector3Int(-1, -9)) + new Vector3(0.5f, 0);
        theAnim = thePacman.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var pacmanPos = thePacman.transform.position;
        var currentCell = (Vector2Int)tm.layoutGrid.WorldToCell(pacmanPos);
        var nextCell = currentCell + pacmanVelocity;
        //var nextCellCenter = tm.layoutGrid.GetCellCenterWorld((Vector3Int)nextCell);
        var currentCellCenter = tm.layoutGrid.GetCellCenterWorld((Vector3Int)currentCell);
        var displacement = pacmanPos - currentCellCenter;
        if (tm.GetTile((Vector3Int)nextCell) != wallTile || ((pacmanVelocity.x == 0 || Mathf.Sign(displacement.x) != Mathf.Sign(pacmanVelocity.x)) && (pacmanVelocity.y != 0 || Mathf.Sign(displacement.y) == Mathf.Sign(pacmanVelocity.y))))
        {
            theAnim.speed = 1;
            pacmanPos += (Vector3)(Vector3Int)pacmanVelocity * Time.deltaTime;
            thePacman.transform.position = pacmanPos;
        }
        else
        {
            theAnim.speed = 0;
        }
    }
}
