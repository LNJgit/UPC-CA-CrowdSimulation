using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class Grid : FiniteGraph<GridCell, CellConnection, GridConnections>
{
    // GRID PARAMETERS
    private float sizeOfCell; // SIZE OF EACH GRID CELL
    private int numRows, numColumns; // NUMBER OF ROWS AND COLUMNS

    // GRID CONSTRUCTOR: INITIALIZES GRID WITH CELLS AND CONNECTIONS
    public Grid(float minX, float maxX, float minZ, float maxZ, float cellSize) : base()
    {
        sizeOfCell = cellSize; // STORE CELL SIZE
        numRows = Mathf.FloorToInt((maxZ - minZ) / cellSize); // CALCULATE ROWS
        numColumns = Mathf.FloorToInt((maxX - minX) / cellSize); // CALCULATE COLUMNS

        GenerateGrid(minX, minZ); // CREATE GRID CELLS
        GenerateConnections();   // CREATE CONNECTIONS BETWEEN CELLS
    }

    // METHOD TO GENERATE GRID CELLS
    private void GenerateGrid(float minX, float minZ)
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                // CALCULATE CELL CENTER POSITION
                Vector3 position = new Vector3(
                    minX + col * sizeOfCell + sizeOfCell / 2,
                    0,
                    minZ + row * sizeOfCell + sizeOfCell / 2
                );

                // CREATE NEW GRIDCELL
                GridCell cell = new GridCell(row * numColumns + col, position, sizeOfCell);

                // RANDOMLY MARK SOME CELLS AS OBSTACLES
                if (Random.Range(0.0f, 1.0f) <= 0.2f) // 20% PROBABILITY OF OBSTACLE
                {
                    cell.SetOccupied(true);
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

            // GET NEIGHBORING CELLS
            foreach (GridCell neighbor in GetNeighbors(cell))
            {
                // ONLY CONNECT TO NON-OCCUPIED CELLS
                if (!neighbor.occupied)
                {
                    cellConnections.Add(new CellConnection(cell, neighbor));
                }
            }

            connections.Add(cellConnections); // ADD CONNECTIONS TO GRID
        }
    }

    // METHOD TO GET VALID NEIGHBORS FOR A GIVEN CELL
    private List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new List<GridCell>();

        // DEFINE NEIGHBOR OFFSETS: UP, DOWN, LEFT, RIGHT
        Vector3[] offsets = new Vector3[]
        {
            new Vector3(0, 0, sizeOfCell),    // UP
            new Vector3(0, 0, -sizeOfCell),   // DOWN
            new Vector3(sizeOfCell, 0, 0),    // RIGHT
            new Vector3(-sizeOfCell, 0, 0)    // LEFT
        };

        // CHECK EACH NEIGHBOR OFFSET
        foreach (Vector3 offset in offsets)
        {
            Vector3 neighborPos = cell.center + offset;
            GridCell neighbor = nodes.Find(c => c.center == neighborPos);
            if (neighbor != null) // ADD VALID NEIGHBOR
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}
