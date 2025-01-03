using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathTest : MonoBehaviour
{
    public GameObject cellPrefab;  // PREFAB FOR VISUALIZING GRID CELLS
    public GameObject startPrefab; // PREFAB FOR START NODE
    public GameObject goalPrefab;  // PREFAB FOR GOAL NODE
    public GameObject pathPrefab;  // PREFAB FOR PATH NODES

    public float gridMinX = -10f;
    public float gridMaxX = 10f;
    public float gridMinZ = -10f;
    public float gridMaxZ = 10f;
    public float cellSize = 2f;

    private Grid grid;
    private Grid_A_Star pathfinder;

    void Start()
    {
        // INITIALIZE GRID
        grid = new Grid(gridMinX, gridMaxX, gridMinZ, gridMaxZ, cellSize);

        // VISUALIZE GRID CELLS
        VisualizeGrid();

        // RUN A* PATHFINDING
        GridCell startCell = grid.nodes[0];                    // START AT FIRST CELL
        GridCell goalCell = grid.nodes[grid.nodes.Count - 1];  // GOAL AT LAST CELL

        pathfinder = new Grid_A_Star(); 
        GridHeuristic heuristic = new GridHeuristic(goalCell, FindObjectOfType<PathManager>()); // PASS PATHMANAGER
        int found = -1;
        List<GridCell> path = pathfinder.findpath(grid, startCell, goalCell, heuristic, ref found);


        // VISUALIZE PATH
        if (found > 0)
        {
            VisualizePath(path);
        }
        else
        {
            Debug.LogError("Path not found!");
        }
    }

    void VisualizeGrid()
    {
        foreach (GridCell cell in grid.nodes)
        {
            GameObject cellObj = Instantiate(cellPrefab, cell.center, Quaternion.identity);
            cellObj.transform.localScale = new Vector3(cellSize, 0.1f, cellSize);

            Renderer renderer = cellObj.GetComponent<Renderer>();
            if (cell.occupied)
                renderer.material.color = Color.red; // OBSTACLES
            else
                renderer.material.color = Color.white; // FREE CELLS
        }
    }

    void VisualizePath(List<GridCell> path)
    {
        foreach (GridCell cell in path)
        {
            Instantiate(pathPrefab, cell.center, Quaternion.identity);
        }
        Instantiate(startPrefab, path[0].center, Quaternion.identity); // START NODE
        Instantiate(goalPrefab, path[path.Count - 1].center, Quaternion.identity); // GOAL NODE
    }
}
