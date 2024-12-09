using System.Collections;
using UnityEngine;
using System.Collections.Generic;

// A concrete implementation of FiniteGraph specifically for handling GridCell, CellConnection, GridConnections
public class ConcreteFiniteGraph
{
    public List<GridCell> nodes;
    public List<GridConnections> connections;
    public List<GridConnections> nodeConnections = new List<GridConnections>();


    private int width;
    private int height;
    private float spacing;

    public ConcreteFiniteGraph(int width, int height, float spacing)
    {
        this.width = width;
        this.height = height;
        this.spacing = spacing;

        nodes = new List<GridCell>();
        connections = new List<GridConnections>();
    }

    public void AddNode(GridCell node)
    {
        nodes.Add(node);
        connections.Add(new GridConnections()); // Ensure a corresponding GridConnections list is added for each node
    }

    public void AddConnection(GridCell fromNode, GridCell toNode, CellConnection connection)
    {
        int index = nodes.IndexOf(fromNode);
        if (index != -1 && nodes.Contains(toNode))
        {
            connections[index].Add(connection);
        }
    }


    public void AddAllConnections()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            int x = i % width;
            int z = i / width;
            var currentConnections = new GridConnections();
            int[] neighborIndices = { i - 1, i + 1, i - width, i + width };

            foreach (var index in neighborIndices)
            {
                if (index >= 0 && index < nodes.Count)
                {
                    int neighborX = index % width;
                    int neighborZ = index / width;

                    // Ensure the neighbor is actually adjacent (not wrapping around)
                    if ((Mathf.Abs(neighborX - x) + Mathf.Abs(neighborZ - z)) == 1)
                    {
                        GridCell neighbor = nodes[index];
                        CellConnection.Direction direction;

                        if (neighborX > x) {
                            direction = CellConnection.Direction.East; // Assuming positive X is East
                        } else if (neighborX < x) {
                            direction = CellConnection.Direction.West; // Assuming negative X is West
                        } else if (neighborZ > z) {
                            direction = CellConnection.Direction.North; // Assuming positive Z is North
                        } else {
                            direction = CellConnection.Direction.South; // Assuming negative Z is South
                        }

                        CellConnection connection = new CellConnection(nodes[i], neighbor, Vector3.Distance(nodes[i].GetCenter(), neighbor.GetCenter()), direction);
                        currentConnections.AddConnection(connection);
                    }

                }
            }
            connections[i] = currentConnections; // Assigning the new connections to the corresponding list
        }
    }
}
