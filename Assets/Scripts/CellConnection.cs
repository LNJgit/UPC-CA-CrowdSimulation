using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellConnection : Connection<GridCell>
{
    // Example: Adding a direction property to indicate the direction of the connection
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public Direction direction;

    // Constructor inheriting from the base Connection class
    public CellConnection(GridCell from, GridCell to, float initialCost, Direction dir) : base(from, to)
    {
        direction = dir;
    }

    // Any additional methods or properties specific to grid cell connections can be added here
}
