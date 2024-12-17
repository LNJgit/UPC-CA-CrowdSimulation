using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class Grid_Bidirectional_A_Star 
    : Bidirectional_A_Star<GridCell, CellConnection, GridConnections, Grid, GridHeuristic>
{
    // CLASS THAT IMPLEMENTS BIDIRECTIONAL A* PATHFINDING OVER A GRID GRAPH
    // CONSTRUCTOR CALLS BASE CLASS WITHOUT PARAMETERS

    public Grid_Bidirectional_A_Star() : base()
    {
    }
}