using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : Node
{
    // Bounds of the cell
    protected float xMin;
    protected float xMax;
    protected float zMin;
    protected float zMax;

    // Center position of the cell for easy access
    protected Vector3 center;

    // Occupied status to indicate whether the cell has an obstacle
    protected bool occupied;

    // Constructor
    public GridCell(int id, float minX, float maxX, float minZ, float maxZ, bool isOccupied) : base(id)
    {
        xMin = minX;
        xMax = maxX;
        zMin = minZ;
        zMax = maxZ;
        occupied = isOccupied;

        // Calculate center of the cell
        center = new Vector3((xMin + xMax) / 2.0f, 0, (zMin + zMax) / 2.0f);
    }

    // Accessors for properties
    public bool IsOccupied()
    {
        return occupied;
    }

    public Vector3 GetCenter()
    {
        return center;
    }

    // Method to set cell occupancy
    public void SetOccupied(bool status)
    {
        occupied = status;
    }
}
