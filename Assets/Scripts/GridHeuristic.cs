using UnityEngine;
using PathFinding;

public class GridHeuristic : Heuristic<GridCell>
{
    private PathManager pathManager; // REFERENCE TO PATHMANAGER

    // CONSTRUCTOR TAKES GOAL NODE AND PATHMANAGER REFERENCE
    public GridHeuristic(GridCell goal, PathManager manager) : base(goal)
    {
        goalNode = goal;
        pathManager = manager;
    }

    // ESTIMATES COST USING EUCLIDEAN DISTANCE AND AGENT DENSITY PENALTY
    public override float estimateCost(GridCell fromNode)
    {
        int densityPenalty = pathManager.GetAgentDensity(fromNode); // DENSITY PENALTY
        return Vector3.Distance(fromNode.center, goalNode.center) + densityPenalty * 10; // ADD PENALTY
    }

    // CHECKS IF THE GOAL NODE IS REACHED
    public override bool goalReached(GridCell node)
    {
        return node.getId() == goalNode.getId();
    }
}
