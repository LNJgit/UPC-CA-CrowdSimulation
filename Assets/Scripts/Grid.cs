using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public float spacing;
    public GameObject nodePrefab;
    public float obstacleProbability = 0.3f;  // Probability that a node will be occupied by an obstacle

    private ConcreteFiniteGraph graph;

    void Start()
    {
        graph = new ConcreteFiniteGraph(gridWidth, gridHeight, spacing);
        BuildRandomGraph();
    }

    private void BuildRandomGraph()
{
    // Calculate offset to center the grid
    Vector3 offset = new Vector3(-gridWidth * spacing / 2, 0, -gridHeight * spacing / 2);

    for (int x = 0; x < gridWidth; x++)
    {
        for (int z = 0; z < gridHeight; z++)
        {
            // Adjust position by offset to center the grid
            Vector3 position = new Vector3(x * spacing, 0, z * spacing) + offset;
            bool isBoundary = x == 0 || z == 0 || x == gridWidth - 1 || z == gridHeight - 1;
            bool isOccupied = isBoundary || Random.value < obstacleProbability; // Ensure boundary nodes are always occupied

            GameObject nodeObj = Instantiate(nodePrefab, position, Quaternion.identity, transform);
            int nodeId = nodeObj.GetInstanceID(); // Unique identifier for the node
            GridCell cell = new GridCell(nodeId, position.x, position.x, position.z, position.z, isOccupied);
            graph.AddNode(cell);

            if (isOccupied)
            {
                nodeObj.GetComponent<Renderer>().material.color = Color.red; // Color the node red to indicate it is occupied
            }
            else
            {
                Destroy(nodeObj); // Destroy prefab if not an obstacle
            }
        }
    }

    // Connect all adjacent nodes (4-way connectivity)
    for (int i = 0; i < graph.nodes.Count; i++)
    {
        GridCell current = graph.nodes[i];
        int x = i % gridWidth;
        int z = i / gridWidth;

        // Connect to right and up neighbors if within bounds
        List<int> potentialNeighbors = new List<int>();
        if (x < gridWidth - 1) potentialNeighbors.Add(i + 1); // Right neighbor
        if (z < gridHeight - 1) potentialNeighbors.Add(i + gridWidth); // Up neighbor

        foreach (var index in potentialNeighbors)
        {
            GridCell neighbor = graph.nodes[index];
            if (current.IsOccupied() && neighbor.IsOccupied()) // Create connection only if both nodes are obstacles
            {
                CellConnection connection = new CellConnection(current, neighbor, Vector3.Distance(current.GetCenter(), neighbor.GetCenter()), CellConnection.Direction.North);
                graph.AddConnection(current, neighbor, connection);
            }
        }
    }
}

}
