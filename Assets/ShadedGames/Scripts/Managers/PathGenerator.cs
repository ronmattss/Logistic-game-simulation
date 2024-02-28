using System.Collections;
using ProjectAssets.Scripts;
using System.Collections.Generic;
using ProjectAssets.Scripts.Util;
using TMPro;
using UnityEngine;
using ShadedGames.Scripts.AgentSystem;
using UnityEngine.UI;
using ShadedGames.Scripts.Utils;

namespace ShadedGames.Scripts.Managers
{


    /// <summary>
    /// This will be part of the USER UI later, but for now it will live in the editor
    /// What This Do: Create paths for Agents
    /// > Select Node
    /// > Click Paths to be added
    /// > 
    /// > press OK to finish
    /// > THIS WILL BE A UI in Game
    /// </summary>
    public class PathGenerator : Singleton<PathGenerator>
    {

        public Agent selectedAgent;
        public List<GeneratedRoute> routes = new();
        public Button selectNodeButton;
        public Button selectNodeLayerButton;
        public Button confirmButton;
        public Button cancelButton;
        public List<Node> waypointsToAdd;



        // Detects Mouse
        [SerializeField] Vector3 mousePosition;
        [SerializeField] bool pathGenerationMode = false;
        [SerializeField] Cell currentlySelectedCell;
        [SerializeField] Node currentlySelectedNode;
        [SerializeField] List<Node> currentlySelectedNodes = new List<Node>();
        [SerializeField]






        // BUtton Function

        public void SetAgentRoute()
        {
            selectedAgent.GetAgentBehaviour().agentMovement.SetNodeWaypoints(routes[0].nodeWaypoint);
        }
        // BUtton Function
        public void SelectWaypoint()
        {
            if (waypointsToAdd.Count > 0)
            {
                var newRoute = new GeneratedRoute("Example Test Route", new List<Node>(waypointsToAdd));
                routes.Add(newRoute);

                ClearAddedWaypoints();
            }

        }

        void ClearAddedWaypoints()
        {
            waypointsToAdd.Clear();
            currentlySelectedNodes.Clear();
            waypointsToAdd.Capacity = 0;
            currentlySelectedNodes.Capacity = 0;
        }
        void ClearCurrentlySelectedNodes()
        {
            currentlySelectedCell = null;
            currentlySelectedNode = null;
            currentlySelectedNodes.Clear();
            currentlySelectedNodes.Capacity = 0;


        }




        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            GetCellNodeOnGrid();
        }


        void GetCellNodeOnGrid()
        {
            if (!pathGenerationMode) return;

            if (!Mouse3D.Instance.GetMouseState()) return;

            mousePosition = Mouse3D.GetMouseWorldPosition();
            // Get the Cell Node in the current Cell
            // NOTE: There are TWO WAYS TO DO THIS, CHECK VIA THE POSITION, 
            // OR JUST CHECK THE COLLIDER THE MOUSE IS RETURNING
            var checkCell = Mouse3D.GetCellOnMouseWorldPosition();
            switch (currentlySelectedCell)
            {
                case null:
                    currentlySelectedCell = checkCell;
                    currentlySelectedNode = currentlySelectedCell.GetComponent<Node>();
                    break;
                default:
                    if (currentlySelectedCell == checkCell) return;
                    currentlySelectedCell = checkCell;
                    break;
            }
            SelectNodesForPath();
        }


        // WE NOW CREATE A BETTER METHOD TO SELECT THE CURRENT NODES FOR A WAYPOINT
        void SelectNodesForPath()
        {

            // Note THIS WILL NOT DUPLICATE THE NODE IN THE NODE LIST
            // DUPLICATED NODE IS USEFUL if the direction is looped 

            if (!currentlySelectedNodes.Contains(currentlySelectedCell.GetComponent<Node>()))
            {
                if (!CheckIfNeighboringNode()) return;
                currentlySelectedNode = currentlySelectedCell.GetComponent<Node>();
                currentlySelectedNodes.Add(currentlySelectedNode); // This is for Debugging
                waypointsToAdd.Add(currentlySelectedNode);
            }
        }

        // voi SelectNodesForPathWithLimit

        bool CheckIfNeighboringNode()
        {
            if (currentlySelectedNodes.Count == 0) return true;
            for (int i = 0; i < currentlySelectedNode.GetNodeNeighbors().Length; i++)
            {
                if (currentlySelectedNode.GetNodeNeighbors()[i] == currentlySelectedCell.GetComponent<Node>())
                {
                    return true;
                }
            }
            Debug.Log("Current Node is not connected to the current Node");
            return false;
        }


        void DebugGenerateRoute()
        {
            
        }

        // The challenge: When mouse is down, Get the cell on the grid with a Node, then add it to the list
        // only add a new Node if it is adjacent to the previous Node. 
        // DO NOT DUPLICATE NODES on the LIST (To be Decided)
        // Path Optimization (Basic Idea, only include nodes with turns)
    }

}