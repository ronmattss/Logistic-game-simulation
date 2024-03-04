using System.Collections;
using System.Collections.Generic;
using ShadedGames.Scripts.Astar;
using UnityEngine;


/// <summary>
/// A node in a cell, not all cell has a node
/// </summary>
public class Node : MonoBehaviour
{
    public FieldNode node; // Instantiate on Grid Placement 
    private Vector3 worldPosition;
    private Vector3 gridPosition;
    [SerializeField] private Node[] neigborNodes = new Node[4];
    [SerializeField] private bool isWalkable = false;

    public Node[] GetNodeNeighbors() => neigborNodes;
    public void SetNodeNeighbors(Node neighbor, int nodeIndex)
    {
        if(neighbor == null) return;
        if (nodeIndex <= 3 && nodeIndex >= 0)
        {
            neigborNodes[nodeIndex] = neighbor;
        }
    }


    public void InstantiateFieldNode()
    {
        
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
