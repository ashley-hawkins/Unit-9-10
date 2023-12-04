using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GhostMovement : MonoBehaviour
{
    public TileBase[] wallTiles;
    public Tilemap tm;
    // Initialized to a cell that's definitely not in the map.
    Vector2Int previousCell = Vector2Int.one * int.MaxValue;
    Vector2Int velocity;
    Vector2Int nextVelocity = Vector2Int.left;
    public Vector2Int TargetTile { get; set; }
    // Start is called before the first frame update
    public TileBase RTeleporterTile;
    public TileBase LTeleporterTile;

    public GameObject pacman;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var localPreviousCell = previousCell;
        var currentCell = (Vector2Int)tm.WorldToCell(transform.position);
        var currentCellType = tm.GetTile((Vector3Int)currentCell);
        if ((Vector2Int)tm.WorldToCell(pacman.transform.position) == currentCell)
        {
            SceneManager.LoadScene("Scenes/GameOverScene");
        }

        previousCell = currentCell;
        var difference = transform.position - tm.layoutGrid.GetCellCenterWorld((Vector3Int)currentCell);


        if (currentCellType == RTeleporterTile)
        {
            if (velocity == Vector2Int.right)
            {
                transform.position -= new Vector3(MazeNavigationLogic.MapWidth + 2, 0);
            }
        }
        else if (currentCellType == LTeleporterTile)
        {
            if (velocity == Vector2Int.left)
            {
                transform.position += new Vector3(MazeNavigationLogic.MapWidth + 2, 0);
            }
        }
        if (currentCell != localPreviousCell)
        {
            velocity = nextVelocity;
            var nextCell = currentCell + velocity;
            var nextDirections = new List<Vector2Int>{
                Vector2Int.up,
                Vector2Int.left,
                Vector2Int.down,
                Vector2Int.right
            };
            Func<Vector2Int, float> distanceFromTarget = x => (TargetTile - (nextCell + x)).magnitude;

            nextDirections.Remove(-velocity);
            nextDirections = nextDirections.Where(
                x => !wallTiles.Contains(tm.GetTile((Vector3Int)(nextCell + x)))
                ).OrderBy(distanceFromTarget).ToList();
            nextVelocity = nextDirections[0];
        }
        if (velocity.x != 0)
        {
            if (difference.y != 0)
            {
                var diffMagnitude = Mathf.Abs(difference.y);
                var travel = -Mathf.Sign(difference.y) * Mathf.Min(Time.deltaTime * 10f, diffMagnitude);
                transform.position += new Vector3(0, travel);
                return;
            }
        }
        if (velocity.y != 0)
        {
            if (difference.x != 0)
            {
                var diffMagnitude = Mathf.Abs(difference.x);
                var travel = -Mathf.Sign(difference.x) * Mathf.Min(Time.deltaTime * 10f, diffMagnitude);
                transform.position += new Vector3(travel, 0);
                return;
            }
        }
        transform.position += Vector3.ClampMagnitude(((Vector3)(Vector3Int)velocity) * Time.deltaTime * 10f, 1);
    }
}
