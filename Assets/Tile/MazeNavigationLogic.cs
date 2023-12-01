using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MazeNavigationLogic : MonoBehaviour
{
    const int MapWidth = 28;
    public Tilemap tm;
    public TileBase foodTile;
    public TileBase[] wallTiles;
    public TileBase removedTile;
    public TileBase RTeleporterTile;
    public TileBase LTeleporterTile;
    public GameObject thePacman;
    public Animator theAnim;
    public JoystickDrag joystick;
    // Start is called before the first frame update
    Vector2Int pacmanVelocity = Vector2Int.left;
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

        if (joystick.Output.magnitude > 0.5f)
        {
            var angle = (Mathf.Atan2(joystick.Output.y, joystick.Output.x) + Mathf.PI * 2) % (Mathf.PI * 2);
            if ((1 * Mathf.PI / 4) < angle && angle <= (3 * Mathf.PI / 4))
            {
                wantedVelocity = Vector2Int.up;
            }
            else if ((3 * Mathf.PI / 4) < angle && angle <= (5 * Mathf.PI / 4))
            {
                wantedVelocity = Vector2Int.left;
            }
            else if ((5 * Mathf.PI / 4) < angle && angle <= (7 * Mathf.PI / 4))
            {
                wantedVelocity = Vector2Int.down;
            }
            else
            {
                wantedVelocity = Vector2Int.right;
            }
        }
        else
        {
            wantedVelocity = Vector2Int.zero;
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
            if (!wallTiles.Contains(wantedNextCellType))
            {
                pacmanVelocity = wantedVelocity;
            }
        }
        var nextCell = currentCell + pacmanVelocity;
        //var nextCellCenter = tm.layoutGrid.GetCellCenterWorld((Vector3Int)nextCell);
        var currentCellCenter = tm.layoutGrid.GetCellCenterWorld((Vector3Int)currentCell);
        var displacement = pacmanPos - currentCellCenter;

        if (currentCellType == RTeleporterTile)
        {
            if (pacmanVelocity == Vector2Int.right)
            {
                pacmanPos.x -= displacement.x + 0.48f + MapWidth;
            }
        }
        else if (currentCellType == LTeleporterTile)
        {
            if (pacmanVelocity == Vector2Int.left)
            {
                pacmanPos.x += displacement.x + 0.48f + MapWidth;
            }
        }
        thePacman.transform.rotation = Quaternion.Euler(0, 0, 90 - Mathf.Atan2(pacmanVelocity.x, pacmanVelocity.y) * Mathf.Rad2Deg);

        var nextCellTileType = tm.GetTile((Vector3Int)nextCell);
        // print(displacement);
        if (!wallTiles.Contains(nextCellTileType) || ((pacmanVelocity.x == 0 || (displacement.x != 0 && Mathf.Sign(displacement.x) != Mathf.Sign(pacmanVelocity.x))) && (pacmanVelocity.y == 0 || (displacement.y != 0 && Mathf.Sign(displacement.y) != Mathf.Sign(pacmanVelocity.y)))))
        {
            theAnim.speed = 1;
            pacmanPos += (Vector3)(Vector3Int)pacmanVelocity * Time.deltaTime * 11;
            if (pacmanVelocity.y != 0 && displacement.x != 0)
            {
                var addedVelocity = -new Vector3(Mathf.Sign(displacement.x), 0) * Time.deltaTime * 11;
                if (Mathf.Abs(addedVelocity.x) > Mathf.Abs(displacement.x))
                {
                    addedVelocity.x = -displacement.x;
                }
                pacmanPos += addedVelocity;
            }
            else if (pacmanVelocity.x != 0 && displacement.y != 0)
            {
                var addedVelocity = -new Vector3(0, Mathf.Sign(displacement.y)) * Time.deltaTime * 11;
                if (Mathf.Abs(addedVelocity.y) > Mathf.Abs(displacement.y))
                {
                    addedVelocity.y = -displacement.y;
                }
                pacmanPos += addedVelocity;
            }

            thePacman.transform.position = pacmanPos;
        }
        else
        {
            thePacman.transform.position = currentCellCenter;
            theAnim.speed = 0;
        }
    }
}
