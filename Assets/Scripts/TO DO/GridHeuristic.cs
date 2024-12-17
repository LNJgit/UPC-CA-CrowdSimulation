using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class GridHeuristic : Heuristic<GridCell>
{
    // CONSTRUCTOR: SETS THE GOAL NODE
    public GridHeuristic(GridCell goal) : base(goal)
    {
        goalNode = goal;
    }

    // ESTIMATES COST USING EUCLIDEAN DISTANCE
    public override float estimateCost(GridCell fromNode)
    {
        return Vector3.Distance(fromNode.center, goalNode.center);
    }

    // CHECKS IF THE GOAL NODE IS REACHED
    public override bool goalReached(GridCell node)
    {
        return node.getId() == goalNode.getId();
    }
}
