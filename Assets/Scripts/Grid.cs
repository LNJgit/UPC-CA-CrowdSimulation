using UnityEngine;
using System.Collections.Generic;
using PathFinding;

public class Grid : FiniteGraph<GridCell, CellConnection, GridConnections>
{
    private float sizeOfCell; // SIZE OF EACH GRID CELL
    private int numRows, numColumns; // NUMBER OF ROWS AND COLUMNS
    private GameObject obstaclePrefab; // OBSTACLE PREFAB TO VISUALIZE OCCUPIED CELLS

    // CONSTRUCTOR: INITIALIZES GRID WITH OBSTACLES
    public Grid(float minX, float maxX, float minZ, float maxZ, float cellSize, GameObject obstaclePrefab = null) : base()
    {
        sizeOfCell = cellSize;
        numRows = Mathf.FloorToInt((maxZ - minZ) / cellSize);
        numColumns = Mathf.FloorToInt((maxX - minX) / cellSize);
        this.obstaclePrefab = obstaclePrefab;

        GenerateGrid(minX, minZ);
        GenerateConnections();
    }

    // METHOD TO GENERATE GRID CELLS
    private void GenerateGrid(float minX, float minZ)
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                Vector3 position = new Vector3(
                    minX + col * sizeOfCell + sizeOfCell / 2,
                    0,
                    minZ + row * sizeOfCell + sizeOfCell / 2
                );

                // CREATE NEW GRIDCELL
                GridCell cell = new GridCell(row * numColumns + col, position, sizeOfCell);

                // RANDOMLY MARK SOME CELLS AS OBSTACLES
                if (Random.Range(0.0f, 1.0f) <= 0.0f) // 20% CHANCE TO BE AN OBSTACLE
                {
                    cell.SetOccupied(true);

                    // INSTANTIATE OBSTACLE PREFAB IF PROVIDED
                    if (obstaclePrefab != null)
                    {
                        GameObject obstacle = GameObject.Instantiate(obstaclePrefab, position, Quaternion.identity);
                        obstacle.transform.localScale = new Vector3(sizeOfCell, sizeOfCell, sizeOfCell);
                        obstacle.name = $"Obstacle_{row}_{col}";
                    }
                }

                nodes.Add(cell); // ADD CELL TO NODE LIST
            }
        }
    }

    // METHOD TO GENERATE CONNECTIONS BETWEEN CELLS
    private void GenerateConnections()
    {
        foreach (GridCell cell in nodes)
        {
            GridConnections cellConnections = new GridConnections();

            foreach (GridCell neighbor in GetNeighbors(cell))
            {
                if (!neighbor.occupied)
                {
                    cellConnections.Add(new CellConnection(cell, neighbor));
                }
            }

            connections.Add(cellConnections);
        }
    }

    private List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new List<GridCell>();
        Vector3[] offsets = new Vector3[]
        {
            new Vector3(0, 0, sizeOfCell),
            new Vector3(0, 0, -sizeOfCell),
            new Vector3(sizeOfCell, 0, 0),
            new Vector3(-sizeOfCell, 0, 0)
        };

        foreach (Vector3 offset in offsets)
        {
            Vector3 neighborPos = cell.center + offset;
            GridCell neighbor = nodes.Find(c => c.center == neighborPos);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
