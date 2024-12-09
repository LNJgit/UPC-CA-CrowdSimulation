using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Specialized NodeConnections for GridCells and CellConnections
public class GridConnections : NodeConnections<GridCell, CellConnection>
{
    // List to hold all connections related to a specific node
    public List<CellConnection> connections = new List<CellConnection>();

    public GridConnections() : base()
    {
    }

    // Method to add a connection if it doesn't already exist
    public void Add(CellConnection connection)
    {
        if (!connections.Contains(connection))
        {
            connections.Add(connection);
        }
    }

    // Method to remove a connection
    public void RemoveConnection(CellConnection connection)
    {
        if (connections.Contains(connection))
        {
            connections.Remove(connection);
        }
    }

    // Method to get all connections
    public List<CellConnection> GetAllConnections()
    {
        return connections;
    }
}
