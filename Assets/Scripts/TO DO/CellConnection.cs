using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class CellConnection : Connection<GridCell>
{
    // CLASS THAT REPRESENTS THE CONNECTION BETWEEN 2 GRIDCELLS

    public CellConnection(GridCell from, GridCell to) : base(from, to)
    {
        // SET COST BASED ON EUCLIDEAN DISTANCE
        float dx = from.center.x - to.center.x;
        float dz = from.center.z - to.center.z;
        setCost(Mathf.Sqrt(dx * dx + dz * dz));
    }
}
