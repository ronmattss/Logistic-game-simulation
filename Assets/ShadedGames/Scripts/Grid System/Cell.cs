using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using ShadedGames.Scripts.Grid_System;
using UnityEngine;



/// <summary>
/// The most basic Cell, it only has basic info, worldPosition, neighbors, and Connection Type
/// 
/// </summary>
/// 
/// 
/// 
/// It makes more sense to call the node as cell cause you know it's inside a grid system.
/*[ExecuteAlways]
*/
public class Cell : MonoBehaviour
{
    private Vector3 worldPosition;
    [SerializeField] private Node node;

    [SerializeField] private Cell[] neighborObjects = new Cell[4];
    public Node GetNode() => node;





    // Set the neighboring cells
    public void SetNeighbor()
    {
        int x, y;
        GridSystem.Instance.GetGrid().GetXZ(worldPosition, out x, out y);
        neighborObjects[0] = GridSystem.Instance.GetCellOnGrid(x, y + 1); // North
        neighborObjects[1] = GridSystem.Instance.GetCellOnGrid(x + 1, y); // East
        neighborObjects[2] = GridSystem.Instance.GetCellOnGrid(x, y - 1); // South
        neighborObjects[3] = GridSystem.Instance.GetCellOnGrid(x - 1, y); // West
        SetNodeNeighbor(); // Set the node Neighbors if it is not null
        // now Set NodeNeighbors
    }
    public void EditorModeSetNeighbor()
    {
        int x, y;
        GridSystem.Instance.GetGrid().GetXZ(worldPosition, out x, out y);
        neighborObjects[0] = EditorGridSystem.Instance.GetCellOnGrid(x, y + 1); // North
        neighborObjects[1] = EditorGridSystem.Instance.GetCellOnGrid(x + 1, y); // East
        neighborObjects[2] = EditorGridSystem.Instance.GetCellOnGrid(x, y - 1); // South
        neighborObjects[3] = EditorGridSystem.Instance.GetCellOnGrid(x - 1, y); // West
        SetNodeNeighbor(); // Set the node Neighbors if it is not null
        // now Set NodeNeighbors
    }
    public void SetNodeNeighbor()
    {
        if (this.node != null)
        {
            for (int i = 0; i < neighborObjects.Length; i++)
            {
                if (neighborObjects[i] != null)
                {
                    node.SetNodeNeighbors(neighborObjects[i].GetNode(), i);

                }
            }
        }
    }

    public void SetWorldPosition(Vector3 worldPos)
    {
        worldPosition = worldPos;
    }

    public Vector3 GetWorldPosition() => worldPosition;



    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
