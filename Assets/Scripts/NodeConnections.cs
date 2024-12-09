using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnections<TNode, TConnection>
    where TNode : Node
    where TConnection : Connection<TNode>
{
    // List to store connections
    public List<TConnection> connections;

    public NodeConnections()
    {
        connections = new List<TConnection>();
    }

    // Method to add a connection
    public void AddConnection(TConnection connection)
    {
        if (!connections.Contains(connection))
        {
            connections.Add(connection);
        }
    }

    // Method to remove a connection
    public void RemoveConnection(TConnection connection)
    {
        if (connections.Contains(connection))
        {
            connections.Remove(connection);
        }
    }

    // Get all connections for this node
    public List<TConnection> GetAllConnections()
    {
        return connections;
    }
}
