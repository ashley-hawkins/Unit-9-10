using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GhostTargeting : MonoBehaviour
{
    public enum TargetingType
    {
        Blinky,
        Pinky,
        Inky,
        Clyde
    }
    public TargetingType targetingType;
    public Tilemap tm;
    public GameObject PacMan;
    GhostMovement movement;
    public MazeNavigationLogic mnl;
    public GameObject blinky;
    // Start is called before the first frame update

    readonly Vector2Int blinkyHome = new Vector2Int(13, 15);
    readonly Vector2Int pinkyHome = new Vector2Int(-14, 15);
    readonly Vector2Int inkyHome = new Vector2Int(13, -17);
    readonly Vector2Int clydeHome = new Vector2Int(-14, -17);
    void Start()
    {
        movement = GetComponent<GhostMovement>();
    }

    Vector2Int BlinkyTarget()
    {
        if (mnl.ScatterMode) return blinkyHome;
        return (Vector2Int)tm.WorldToCell(PacMan.transform.position);
    }
    Vector2Int PinkyTarget()
    {
        if (mnl.ScatterMode) return pinkyHome;
        var targetTile = (Vector2Int)tm.WorldToCell(PacMan.transform.position) + mnl.pacmanVelocity * 4;
        if (mnl.pacmanVelocity == Vector2Int.up)
        {
            targetTile += Vector2Int.left * 4;
        }
        return targetTile;
    }
    Vector2Int InkyTarget()
    {
        if (mnl.ScatterMode) return inkyHome;
        var pinkyTarget = PinkyTarget();
        var blinkyCell = (Vector2Int)tm.WorldToCell(blinky.transform.position);
        return pinkyTarget + pinkyTarget - blinkyCell;
    }
    Vector2Int ClydeTarget()
    {
        if (mnl.ScatterMode) return clydeHome;
        if ((PacMan.transform.position - transform.position).magnitude > 8)
        {
            return BlinkyTarget();
        }
        else
        {
            return clydeHome;
        }
    }

    void DrawLine(Vector3 centre, Color color)
    {
        Debug.DrawLine(centre - new Vector3(0.5f, 0.5f), centre + new Vector3(0.5f, 0.5f), color);
        Debug.DrawLine(centre - new Vector3(0.5f, -0.5f), centre + new Vector3(0.5f, -0.5f), color);
    }
    // Update is called once per frame
    void Update()
    {
        Vector2Int newTarget = Vector2Int.zero;
        var clr = GetComponent<SpriteRenderer>().color;
        if (targetingType == TargetingType.Blinky)
        {
            newTarget = BlinkyTarget();
        }
        else if (targetingType == TargetingType.Pinky)
        {
            newTarget = PinkyTarget();
        }
        else if (targetingType == TargetingType.Inky)
        {
            newTarget = InkyTarget();
        }
        else if (targetingType == TargetingType.Clyde)
        {
            newTarget = ClydeTarget();
        }
        var centre = tm.layoutGrid.GetCellCenterWorld((Vector3Int)newTarget);
        DrawLine(centre, clr);   
        movement.TargetTile = newTarget;
    }
}
