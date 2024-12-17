using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathFinding;

public class Bidirectional_A_Star<TNode, TConnection, TNodeConnection, TGraph, THeuristic> 
    : PathFinder<TNode, TConnection, TNodeConnection, TGraph, THeuristic>
    where TNode : Node
    where TConnection : Connection<TNode>
    where TNodeConnection : NodeConnections<TNode, TConnection>
    where TGraph : Graph<TNode, TConnection, TNodeConnection>
    where THeuristic : Heuristic<TNode>
{
    protected class NodeRecord
    {
        public TNode node;
        public NodeRecord parent;
        public float costSoFar;
        public float estimatedTotalCost;
    }

    public override List<TNode> findpath(
        TGraph graph, TNode start, TNode goal, THeuristic heuristic, ref int found)
    {
        // OPEN AND CLOSED SETS FOR BOTH DIRECTIONS
        List<NodeRecord> openStart = new List<NodeRecord>();
        HashSet<TNode> closedStart = new HashSet<TNode>();

        List<NodeRecord> openGoal = new List<NodeRecord>();
        HashSet<TNode> closedGoal = new HashSet<TNode>();

        // START AND GOAL INITIAL NODES
        NodeRecord startRecord = new NodeRecord { node = start, costSoFar = 0, estimatedTotalCost = heuristic.estimateCost(start) };
        NodeRecord goalRecord = new NodeRecord { node = goal, costSoFar = 0, estimatedTotalCost = heuristic.estimateCost(goal) };

        openStart.Add(startRecord);
        openGoal.Add(goalRecord);

        // MAIN LOOP
        while (openStart.Count > 0 && openGoal.Count > 0)
        {
            // EXPAND FROM START SIDE
            NodeRecord currentStart = ExpandNode(openStart, closedStart, graph, heuristic, goal, "Start");
            if (currentStart != null && closedGoal.Contains(currentStart.node))
            {
                found = 1;
                return ReconstructPath(currentStart, openGoal.Find(n => n.node == currentStart.node));
            }

            // EXPAND FROM GOAL SIDE
            NodeRecord currentGoal = ExpandNode(openGoal, closedGoal, graph, heuristic, start, "Goal");
            if (currentGoal != null && closedStart.Contains(currentGoal.node))
            {
                found = 1;
                return ReconstructPath(openStart.Find(n => n.node == currentGoal.node), currentGoal);
            }
        }

        found = -1;
        return new List<TNode>(); // PATH NOT FOUND
    }

    private NodeRecord ExpandNode(
        List<NodeRecord> open, HashSet<TNode> closed, TGraph graph, THeuristic heuristic, TNode target, string direction)
    {
        // GET THE NODE WITH LOWEST f(n)
        open = open.OrderBy(n => n.estimatedTotalCost).ToList();
        NodeRecord current = open[0];
        open.RemoveAt(0);

        // MOVE TO CLOSED SET
        closed.Add(current.node);

        // EXPAND NEIGHBORS
        foreach (var connection in graph.getConnections(current.node).connections)
        {
            TNode neighbor = connection.getToNode();
            if (closed.Contains(neighbor)) continue;

            float gCost = current.costSoFar + connection.getCost();
            NodeRecord neighborRecord = open.FirstOrDefault(n => n.node == neighbor);

            if (neighborRecord == null)
            {
                neighborRecord = new NodeRecord
                {
                    node = neighbor,
                    parent = current,
                    costSoFar = gCost,
                    estimatedTotalCost = gCost + heuristic.estimateCost(neighbor)
                };
                open.Add(neighborRecord);
            }
            else if (gCost < neighborRecord.costSoFar)
            {
                neighborRecord.costSoFar = gCost;
                neighborRecord.estimatedTotalCost = gCost + heuristic.estimateCost(neighbor);
                neighborRecord.parent = current;
            }
        }

        return current;
    }

    private List<TNode> ReconstructPath(NodeRecord startNode, NodeRecord goalNode)
    {
        List<TNode> path = new List<TNode>();

        // RECONSTRUCT PATH FROM START TO MEETING POINT
        NodeRecord current = startNode;
        while (current != null)
        {
            path.Add(current.node);
            current = current.parent;
        }
        path.Reverse();

        // RECONSTRUCT PATH FROM MEETING POINT TO GOAL
        current = goalNode.parent; // SKIP DUPLICATE MEETING POINT
        while (current != null)
        {
            path.Add(current.node);
            current = current.parent;
        }

        return path;
    }
}
