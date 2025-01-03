using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PathFinding{

	public abstract class Heuristic <TNode>
	where TNode : Node
	{	
	
	
		protected TNode goalNode; 

		
		public	Heuristic(TNode goal){
			goalNode = goal; 
		}

		
		public abstract float estimateCost(TNode fromNode);

		
		public abstract bool goalReached (TNode node);

	};

}