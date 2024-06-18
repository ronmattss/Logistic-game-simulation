using System.Collections;
using System.Collections.Generic;
using ShadedGames.Scripts.Astar;
using ShadedGames.Scripts.Grid_System;
using UnityEngine;


/// <summary>
/// A node in a cell, not all cell has a node
/// </summary>
public class Node : MonoBehaviour
{
    public FieldNode node; // Instantiate on Grid Placement 
    private Cell parentCell;
    private Vector3 worldPosition;
    private Vector3 gridPosition;
    [SerializeField] private Node[] neigborNodes = new Node[4];
    [SerializeField] private bool isPathWalkable = true;
    [SerializeField] private bool isPlaceable = true;
    [SerializeField] private bool isAWall = true;



    public void SetParentCell(Cell cell)
    {
        parentCell = cell;
    }
    public Cell GetParentCell() {  return parentCell; }
    public Node[] GetNodeNeighbors() => neigborNodes;
    public Vector3 GetWorldPosition() => worldPosition;
    public void SetNodeNeighbors(Node neighbor, int nodeIndex)
    {
        if (neighbor == null) return;
        if (nodeIndex <= 3 && nodeIndex >= 0)
        {
            neigborNodes[nodeIndex] = neighbor;
        }
    }
    public FieldNode GetFieldNode() => node;


    public void InstantiateFieldNode(Vector3 worldPosition, int x, int y, int movementPenalty, bool isPlaceable = true)
    {
        Debug.Log("Init Field Node");
        node = new FieldNode(isPathWalkable, worldPosition, x, y, 0, this);
        this.worldPosition = worldPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(SelfDestruct());
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Block"))
        {
            isPathWalkable = false;
            isAWall = true;
            this.GetComponent<Collider>().enabled = false;
          //  other.GetComponent<Collider>().enabled = false;
            node.SetIsPath(isPathWalkable);
        }
        else if(other.CompareTag("Path"))
        {
           // Debug.Log("Walkable Path");
          //  Debug.Log($"PATH Collission: {other.gameObject.transform.name}");
            isPathWalkable = true;
            this.GetComponent<Collider>().enabled = false;
          //  other.GetComponent<Collider>().enabled = false;
            node.SetIsPath(isPathWalkable);
        }
    }

    public bool RemoveGameObjectIfUnused()
    {
        if (isPathWalkable) return true;
        if (isAWall) return true;
        return false;
    }

    IEnumerator SelfDestruct()
    {
        Debug.Log($"Testing Self Destruct");

        yield return new WaitForSeconds(1);
        if (!RemoveGameObjectIfUnused())
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log($"Cleaning up cause of double Bool: {isPathWalkable} {isAWall}");
            Destroy(this.gameObject);
        }
        else
        {
            GridSystem.Instance.GetCellGrid().Add(parentCell);
            yield return null;
        }
    }
}
