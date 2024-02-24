using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A node in a cell, not all cell has a node
/// </summary>
public class Node : MonoBehaviour
{
    private Vector3 worldPosition;
    private Vector3 gridPosition;
    [SerializeField] private Node[] neigborNodes = new Node[4];

    public Node[] GetNodeNeighbors() => neigborNodes;
    public void SetNodeNeighbors(Node neighbor, int nodeIndex)
    {
        if(neighbor == null) return;
        if (nodeIndex <= 3 && nodeIndex >= 0)
        {
            neigborNodes[nodeIndex] = neighbor;
        }
    }




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
