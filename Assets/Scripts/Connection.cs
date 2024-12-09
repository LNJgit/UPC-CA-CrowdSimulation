using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Connection<TNode> where TNode : Node
{
    public TNode fromNode;  // Reference to the starting node
    public TNode toNode;    // Reference to the destination node
    public int fromNodeId; // id of the origin node
    public int toNodeId; // id of the destination node
    public float cost; // the cost of using that connection in a path
    public Connection(TNode from, TNode to)
    {
        fromNode = from;
        toNode = to;
        fromNodeId = fromNode.id;
        toNodeId = toNode.id;
    }

    public TNode getFromNode()
    {
    return fromNode;
    }
    public TNode getToNode() 
    {
    return toNode;
    }
    public float getCost() 
    {
    return cost;
    }
    public void setCost(float c)
    {
    cost = c;
    }

    // You might want to include methods to update the cost if your application needs dynamic pathfinding updates
}
