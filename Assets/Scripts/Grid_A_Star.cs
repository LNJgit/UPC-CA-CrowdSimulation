using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class Grid_A_Star : A_Star<GridCell, CellConnection, GridConnections, Grid, GridHeuristic>
{
    // CLASS THAT IMPLEMENTS A* PATHFINDING OVER A GRID GRAPH
    // CONSTRUCTOR CALLS BASE CLASS WITHOUT PARAMETERS

    public Grid_A_Star() : base()
    {
    }
}
