using UnityEngine;
using System.Collections;

namespace PathFinding{

	[System.Serializable]	
	public abstract class Node{
	

		public Node(int i){ id = i; }
		public Node(Node n){ id = n.id; }

		public int getId(){ return id; }
		
		public int id;
	};

}

