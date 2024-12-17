using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public float gridMinX = -10f; // MINIMUM X POSITION
    public float gridMaxX = 10f;  // MAXIMUM X POSITION
    public float gridMinZ = -10f; // MINIMUM Z POSITION
    public float gridMaxZ = 10f;  // MAXIMUM Z POSITION
    public float cellSize = 2f;   // SIZE OF EACH CELL

    public GameObject cellPrefab; // PREFAB FOR VISUALIZATION (A SIMPLE CUBE)

    public Grid grid { get; private set; }

    void Start()
    {
        // INITIALIZE THE GRID
        grid = new Grid(gridMinX, gridMaxX, gridMinZ, gridMaxZ, cellSize);

        // VISUALIZE THE GRID CELLS
        VisualizeGrid();
    }

    void VisualizeGrid()
    {
        foreach (GridCell cell in grid.nodes)
        {
            // INSTANTIATE A VISUAL REPRESENTATION FOR EACH CELL
            GameObject cellObject = Instantiate(cellPrefab, cell.center, Quaternion.identity);
            cellObject.transform.localScale = new Vector3(cellSize, 0.1f, cellSize); // SCALE TO MATCH CELL SIZE

            // COLOR CELLS BASED ON WHETHER THEY'RE OCCUPIED OR EMPTY
            Renderer renderer = cellObject.GetComponent<Renderer>();
            if (cell.occupied)
            {
                renderer.material.color = Color.red; // RED FOR OBSTACLES
            }
            else
            {
                renderer.material.color = Color.green; // GREEN FOR FREE CELLS
            }
        }
    }
}
