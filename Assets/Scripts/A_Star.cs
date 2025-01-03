using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PathFinding{

	public class A_Star<TNode, TConnection, TNodeConnection, TGraph, THeuristic> 
    : PathFinder<TNode, TConnection, TNodeConnection, TGraph, THeuristic>
    where TNode : Node
    where TConnection : Connection<TNode>
    where TNodeConnection : NodeConnections<TNode, TConnection>
    where TGraph : Graph<TNode, TConnection, TNodeConnection>
    where THeuristic : Heuristic<TNode>
{
    // STRUCTURE TO KEEP NODE INFORMATION
    protected class NodeRecord
    {
        public TNode node;               // CURRENT NODE
        public NodeRecord connection;    // PARENT CONNECTION
        public float costSoFar;          // ACCUMULATED COST g(n)
        public float estimatedTotalCost; // TOTAL COST f(n)
    }

    public override List<TNode> findpath(
        TGraph graph, TNode start, TNode end, THeuristic heuristic, ref int found)
    {
        // PRIORITY QUEUE FOR OPEN SET
        List<NodeRecord> open = new List<NodeRecord>();
        HashSet<TNode> closed = new HashSet<TNode>(); // CLOSED SET

        // ADD START NODE TO OPEN SET
        NodeRecord startRecord = new NodeRecord
        {
            node = start,
            costSoFar = 0,
            estimatedTotalCost = heuristic.estimateCost(start)
        };
        open.Add(startRecord);

        // PATH CALCULATION LOOP
        while (open.Count > 0)
        {
            // SORT OPEN SET BY LOWEST ESTIMATED COST
            open = open.OrderBy(n => n.estimatedTotalCost).ToList();
            NodeRecord current = open[0];
            open.RemoveAt(0);

            // CHECK IF GOAL IS REACHED
            if (heuristic.goalReached(current.node))
            {
                found = 1; // PATH FOUND
                return ReconstructPath(current);
            }

            // MOVE CURRENT NODE TO CLOSED SET
            closed.Add(current.node);

            // GET CONNECTIONS FOR CURRENT NODE
            var connections = graph.getConnections(current.node);
            foreach (var connection in connections.connections)
            {
                TNode neighbor = connection.getToNode();
                if (closed.Contains(neighbor)) continue; // SKIP IF ALREADY VISITED

                float gCost = current.costSoFar + connection.getCost();

                // CHECK IF NEIGHBOR IS IN OPEN SET
                var neighborRecord = open.FirstOrDefault(n => n.node == neighbor);
                if (neighborRecord == null) // NOT IN OPEN SET
                {
                    neighborRecord = new NodeRecord
                    {
                        node = neighbor,
                        connection = current,
                        costSoFar = gCost,
                        estimatedTotalCost = gCost + heuristic.estimateCost(neighbor)
                    };
                    open.Add(neighborRecord);
                }
                else if (gCost < neighborRecord.costSoFar) // BETTER PATH FOUND
                {
                    neighborRecord.costSoFar = gCost;
                    neighborRecord.estimatedTotalCost = gCost + heuristic.estimateCost(neighbor);
                    neighborRecord.connection = current;
                }
            }
        }

        found = -1; // PATH NOT FOUND
        return new List<TNode>();
    }

    // METHOD TO RECONSTRUCT PATH FROM GOAL TO START
    private List<TNode> ReconstructPath(NodeRecord endRecord)
    {
        List<TNode> path = new List<TNode>();
        NodeRecord current = endRecord;

        while (current != null)
        {
            path.Add(current.node);
            current = current.connection;
        }

        path.Reverse(); // REVERSE TO GET START -> GOAL
        return path;
    }
}
}