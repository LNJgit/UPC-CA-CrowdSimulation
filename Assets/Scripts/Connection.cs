using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathFinding{

	[System.Serializable]
	public abstract class Connection<TNode>
	where TNode : Node
	{
	
		
		[System.NonSerialized]
		public TNode fromNode; 
		[System.NonSerialized]
		public TNode toNode; 
		public int fromNodeId; 
		public int toNodeId; 

		public float cost;  

		public Connection(TNode from, TNode to){
			fromNode = from; 
			toNode = to; 
			fromNodeId = fromNode.id;
			toNodeId = toNode.id;
		}

		public TNode getFromNode(){
			return fromNode;
		}

		public TNode getToNode() {
			return toNode;
		}

		public float getCost() {
			return cost;
		}

		public void setCost(float c){
			cost = c;
		}
	};

}