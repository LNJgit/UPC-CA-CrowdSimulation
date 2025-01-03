using System.Collections.Generic;
using UnityEngine;
using PathFinding;

public class PathManager : MonoBehaviour
{
    private Dictionary<Agent, List<GridCell>> agentPaths = new Dictionary<Agent, List<GridCell>>();
    private Dictionary<Agent, int> agentWaypoints = new Dictionary<Agent, int>();
    private Dictionary<GridCell, int> cellAgentCount = new Dictionary<GridCell, int>();

    [SerializeField] public float cellSize = 1.0f;

    public Grid grid; // REFERENCE TO THE GRID
    private Grid_A_Star pathfinder;

    public GameObject obstaclePrefab; // ASSIGN IN INSPECTOR

    void Start()
    {
        pathfinder = new Grid_A_Star();

        CrowdGenerator crowdGenerator = FindObjectOfType<CrowdGenerator>();
        if (crowdGenerator != null)
        {
            float spawnRange = crowdGenerator.spawnRange;
            grid = new Grid(-spawnRange / 2, spawnRange / 2, -spawnRange / 2, spawnRange / 2, cellSize, obstaclePrefab);
            Debug.Log("Grid initialized with obstacle visualization.");
        }
        else
        {
            Debug.LogError("CrowdGenerator not found. Cannot initialize grid.");
        }
    }

    public void RecomputePathForAgent(Agent agent)
{
    // Get the agent's current cell
    GridCell currentCell = FindCellFromPosition(agent.transform.position);

    // Retrieve the agent's existing goal
    GridCell goalCell = null;
    if (agentPaths.ContainsKey(agent) && agentPaths[agent].Count > 0)
    {
        goalCell = agentPaths[agent][agentPaths[agent].Count - 1];
    }

    // Debug logs for invalid current or goal cells
    if (currentCell == null)
    {
        Debug.LogError($"Current cell not found for agent: {agent.name} at position {agent.transform.position}");
        return;
    }

    if (goalCell == null)
    {
        Debug.LogError($"Goal cell is null for agent: {agent.name}");
        return;
    }

    // Compute a new path to the same goal
    ComputePathForAgent(agent, currentCell, goalCell);
    Debug.Log($"Path recalculated for {agent.name} to the same goal at {goalCell.center}.");
}

void Update()
{
    Debug.Log($"Processing {agentPaths.Count} agents in Update.");

    // Create a copy of the keys to avoid modifying the collection during enumeration
    var agents = new List<Agent>(agentPaths.Keys);

    foreach (var agent in agents)
    {
        Debug.Log($"Attempting to recompute path for agent: {agent.name}");
        RecomputePathForAgent(agent);
    }
}


    // INITIALIZE GRID SIZE DYNAMICALLY
    private void InitializeGrid(float spawnRange)
    {
        float halfRange = spawnRange / 2f;
        grid = new Grid(-halfRange, halfRange, -halfRange, halfRange, 2.0f); // CELL SIZE = 2
    }

    // REGISTER AN AGENT AND COMPUTE INITIAL PATH
    public void RegisterAgent(Agent agent)
    {
        GridCell startCell = FindCellFromPosition(agent.transform.position);
        GridCell goalCell = FindRandomGoalCell();

        if (startCell != null && goalCell != null)
        {
            ComputePathForAgent(agent, startCell, goalCell);
            UpdateAgentCell(agent, null, startCell); // INITIAL CELL ASSIGNMENT
        }
        else
        {
            Debug.LogError("Failed to register agent: Start or Goal cell is invalid.");
        }
    }

    // COMPUTE PATH FOR AN AGENT
    private void ComputePathForAgent(Agent agent, GridCell startCell, GridCell goalCell)
    {
        GridHeuristic heuristic = new GridHeuristic(goalCell, this);
        int found = -1;

        List<GridCell> path = pathfinder.findpath(grid, startCell, goalCell, heuristic, ref found);

        if (found > 0 && path != null)
        {
            agentPaths[agent] = path;
            agentWaypoints[agent] = 0;
            //Debug.Log($"Path computed for {agent.name} with {path.Count} waypoints.");
        }
        else
        {
            //Debug.LogError("Path not found for agent: " + agent.name);
        }
    }

    // PROVIDE NEXT WAYPOINT FOR AGENT
    public Vector3 GetNextWaypoint(Agent agent)
{
    if (!agentPaths.ContainsKey(agent)) return agent.transform.position;

    List<GridCell> path = agentPaths[agent];
    int waypointIndex = agentWaypoints[agent];

    if (waypointIndex < path.Count)
    {
        return path[waypointIndex].center; 
    }
    

    return GetFinalGoal(agent);
}


public void AdvanceToNextWaypoint(Agent agent)
{
    if (agentPaths.ContainsKey(agent))
    {
        agentWaypoints[agent]++;

        if (agentWaypoints[agent] >= agentPaths[agent].Count)
        {
            Debug.Log($"{agent.name} has reached the final goal.");
            UpdateGoalForAgent(agent); 
        }
    }
}



public void UpdateGoalForAgent(Agent agent)
{
    GridCell startCell = FindCellFromPosition(agent.transform.position);
    GridCell newGoalCell = FindRandomGoalCell();

    if (startCell != null && newGoalCell != null)
    {
        ComputePathForAgent(agent, startCell, newGoalCell);
        Debug.Log($"New goal assigned for {agent.name}.");

        // RESET WAYPOINT INDEX AND UPDATE FINAL GOAL
        agentWaypoints[agent] = 0;
        agent.UpdateFinalGoal(newGoalCell.center);
    }
    else
    {
        Debug.LogError("Failed to update goal for agent: " + agent.name);
    }
}




    // TRACK AGENT CELL DENSITY
    public void UpdateAgentCell(Agent agent, GridCell previousCell, GridCell currentCell)
    {
        if (previousCell != null) DecreaseAgentCount(previousCell);
        if (currentCell != null) IncreaseAgentCount(currentCell);
    }

    private void IncreaseAgentCount(GridCell cell)
    {
        if (!cellAgentCount.ContainsKey(cell))
        {
            cellAgentCount[cell] = 0;
        }
        cellAgentCount[cell]++;
    }

    private void DecreaseAgentCount(GridCell cell)
    {
        if (cellAgentCount.ContainsKey(cell) && cellAgentCount[cell] > 0)
        {
            cellAgentCount[cell]--;
        }
    }

    public int GetAgentDensity(GridCell cell)
    {
        return cellAgentCount.ContainsKey(cell) ? cellAgentCount[cell] : 0;
    }

    private GridCell FindCellFromPosition(Vector3 position)
    {
        foreach (GridCell cell in grid.nodes)
        {
            if (position.x >= cell.xMin && position.x <= cell.xMax &&
                position.z >= cell.zMin && position.z <= cell.zMax)
            {
                return cell;
            }
        }
        return null;
    }

    private GridCell FindRandomGoalCell()
    {
        List<GridCell> validCells = new List<GridCell>();
        foreach (GridCell cell in grid.nodes)
        {
            if (!cell.occupied) validCells.Add(cell);
        }

        return validCells.Count > 0 ? validCells[Random.Range(0, validCells.Count)] : null;
    }

public Vector3 GetFinalGoal(Agent agent)
{
    if (agentPaths.ContainsKey(agent) && agentPaths[agent].Count > 0)
    {
        return agentPaths[agent][agentPaths[agent].Count - 1].center; 
    }
    return agent.transform.position;
}
    

}
