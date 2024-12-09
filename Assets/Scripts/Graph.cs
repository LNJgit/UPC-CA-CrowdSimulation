using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Graph<TNode, TConnection, TNodeConnections>
where TNode : Node
where TConnection : Connection<TNode>
where TNodeConnections : NodeConnections<TNode,TConnection>
{
    // Abstract class that represents a graph (infinite or not), that is at least
    // a function that returns a list of connections for any node
    public Graph(){ }
    public abstract TNodeConnections getConnections (TNode fromNode);
};