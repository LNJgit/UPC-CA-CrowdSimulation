using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class GridCell : Node 
{
    public float xMin, xMax, zMin, zMax; // GRID CELL BOUNDARIES
    public Vector3 center;              // CENTER POSITION OF THE CELL
    public bool occupied;               // FLAG TO INDICATE OBSTACLE PRESENCE

    // MAIN CONSTRUCTOR: INITIALIZES CELL PROPERTIES
    public GridCell(int id, Vector3 position, float size) : base(id) 
    {
        xMin = position.x - size / 2;  // SET LEFT BOUNDARY
        xMax = position.x + size / 2;  // SET RIGHT BOUNDARY
        zMin = position.z - size / 2;  // SET LOWER BOUNDARY
        zMax = position.z + size / 2;  // SET UPPER BOUNDARY
        center = position;             // SET CENTER POSITION
        occupied = false;              // DEFAULT: NOT OCCUPIED
    }

    // COPY CONSTRUCTOR: COPIES ALL PROPERTIES FROM ANOTHER GRIDCELL
    public GridCell(GridCell other) : base(other) 
    {
        xMin = other.xMin;     // COPY XMIN
        xMax = other.xMax;     // COPY XMAX
        zMin = other.zMin;     // COPY ZMIN
        zMax = other.zMax;     // COPY ZMAX
        center = other.center; // COPY CENTER POSITION
        occupied = other.occupied; // COPY OCCUPANCY STATUS
    }

    // METHOD TO SET THE CELL AS OCCUPIED OR EMPTY
    public void SetOccupied(bool isOccupied)
    {
        occupied = isOccupied; // UPDATE OCCUPIED STATUS
    }
}
