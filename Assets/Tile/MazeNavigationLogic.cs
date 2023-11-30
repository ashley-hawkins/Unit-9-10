using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeNavigationLogic : MonoBehaviour
{
    public Tilemap tm;
    public TileBase foodTile;
    public TileBase wallTile;
    public TileBase removedTile;
    public GameObject thePacman;
    public Animator theAnim;
    // Start is called before the first frame update
    Vector2Int pacmanVelocity = Vector2Int.up;
    void Start()
    {
        Application.targetFrameRate = 60;
        // tm.SetTile(Vector3Int.zero, theTile);
        thePacman.transform.position = tm.layoutGrid.GetCellCenterWorld(new Vector3Int(-1, -9)) + new Vector3(0.5f, 0);
        theAnim = thePacman.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int wantedVelocity = Vector2Int.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            wantedVelocity = Vector2Int.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            wantedVelocity = Vector2Int.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            wantedVelocity = Vector2Int.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            wantedVelocity = Vector2Int.right;
        }
        var pacmanPos = thePacman.transform.position;
        var currentCell = (Vector2Int)tm.layoutGrid.WorldToCell(pacmanPos);
        var currentCellType = tm.GetTile((Vector3Int)currentCell);
        if (currentCellType == foodTile)
        {
            tm.SetTile((Vector3Int)currentCell, removedTile);
        }
        if (wantedVelocity != Vector2Int.zero)
        {
            var wantedNextCell = currentCell + wantedVelocity;
            var wantedNextCellType = tm.GetTile((Vector3Int)wantedNextCell);
            if (wantedNextCellType != wallTile)
            {
                pacmanVelocity = wantedVelocity;
            }
        }
        var nextCell = currentCell + pacmanVelocity;
        //var nextCellCenter = tm.layoutGrid.GetCellCenterWorld((Vector3Int)nextCell);
        var currentCellCenter = tm.layoutGrid.GetCellCenterWorld((Vector3Int)currentCell);
        var displacement = pacmanPos - currentCellCenter;

        thePacman.transform.rotation = Quaternion.Euler(0, 0, 90 - Mathf.Atan2(pacmanVelocity.x, pacmanVelocity.y) * Mathf.Rad2Deg);

        var nextCellTileType = tm.GetTile((Vector3Int)nextCell);
        print(displacement);
        if (nextCellTileType != wallTile || ((pacmanVelocity.x == 0 || (displacement.x != 0 && Mathf.Sign(displacement.x) != Mathf.Sign(pacmanVelocity.x))) && (pacmanVelocity.y == 0 || (displacement.y != 0 && Mathf.Sign(displacement.y) != Mathf.Sign(pacmanVelocity.y)))))
        {
            theAnim.speed = 1;
            pacmanPos += (Vector3)(Vector3Int)pacmanVelocity * Time.deltaTime * 11;
            //print(pacmanPos);
            if (pacmanVelocity.y != 0 && displacement.x != 0)
            {
                var addedVelocity = -new Vector3(Mathf.Sign(displacement.x), 0) * Time.deltaTime * 11;
                if (Mathf.Abs(addedVelocity.x) > Mathf.Abs(displacement.x))
                {
                    addedVelocity.x = -displacement.x;
                }
                //print("adding velocity on x");
                pacmanPos += addedVelocity;
            }
            else if (pacmanVelocity.x != 0 && displacement.y != 0)
            {
                var addedVelocity = -new Vector3(0, Mathf.Sign(displacement.y)) * Time.deltaTime * 11;
                if (Mathf.Abs(addedVelocity.y) > Mathf.Abs(displacement.y))
                {
                    addedVelocity.y = -displacement.y;
                }
                //print("adding velocity on y");
                pacmanPos += addedVelocity;
            }

            //print("Pos pos: " + pacmanPos);
            thePacman.transform.position = pacmanPos;
            //print("Final pos: " + thePacman.transform.position);
        }
        else
        {
            thePacman.transform.position = currentCellCenter;
            theAnim.speed = 0;
        }
    }
}
