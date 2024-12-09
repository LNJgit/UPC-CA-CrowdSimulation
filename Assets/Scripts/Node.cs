using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Node{
    public Node(int i){ id = i; }
    public Node(Node n){ id = n.id; }
    public int getId(){ return id; }
    public int id;
};