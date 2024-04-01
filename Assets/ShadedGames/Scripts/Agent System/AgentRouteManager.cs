
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using ShadedGames.Scripts.Astar;
using System.IO;
using ShadedGames.Scripts.Grid_System;

namespace ShadedGames.Scripts.AgentSystem
{

    public class AgentRouteManager : MonoBehaviour
    {


        // AgentMovement should only deal with how the Agent will move
        // AgentRouteManager will deal with the Waypoints and nodes33
        [SerializeField] private Vector3 currentGridPosition;
        [SerializeField] private Cell currentCellPosition;
        [SerializeField] private Cell targetCellPosition;
        private Queue<Cell> cellWaypointsQueue = new Queue<Cell>(); // this is Set by path finders or routers
        private Queue<Node> currentNodeWaypointsQueue = new Queue<Node>(); // this is Set by path finders or routers
        [SerializeField] private List<Cell> cellWaypoints = new List<Cell>(); // this is Set by path finders or routers
        [SerializeField] private List<Node> nodeWaypoints = new List<Node>(); // this is Set by path finders or routers
        [SerializeField] private Stack<Node> recentWaypoints = new Stack<Node>(); // Stores recent Route, if route is looped, Pop to nodeWayPoints
        [SerializeField] private List<Node> aStarWaypoints = new List<Node>();

        [SerializeField] private Node currentNodePosition;
        [SerializeField] private Node targetNodePosition;
        // Create Route Class

        [SerializeField] bool routeIsLooped = false; // THIS SHOULD BE CHANGED FROM THE ROUTE GENERATION CLASS, MAKE IT LATER AFTER TESTING
        public UnityEvent onReceiveRoute;
        public UnityEvent OnFinalWaypoint;
        // Debug
        public GameObject pathDebugger;


        public void RequestAStarPath()
        {
            PathRequestManager.Instance.RequestPath(new PathRequest(currentNodePosition, targetNodePosition, OnPathFound)); // TODO:: THIS
        }



        // Request Path With AStar

        public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
        {
            Debug.Log($"Waypoints: {waypoints.Length}");
            if(waypoints !=null)
            {
                for (int i = 0; i < waypoints.Length; i++)
                {
                    Vector3 point = waypoints[i];
                    // Debug.Log(point);
                    Instantiate(pathDebugger,GridSystem.Instance.GetGrid().GetCellMidPoint((int)point.x, (int)point.y),Quaternion.identity);
                    // Convert these points to Nodes
                }
            }
            if (pathSuccessful)
            {


            }
            else
            {
                Debug.Log(this + " No Path");
            }
        }









        // Set Node Waypoints for the Routes to used by the Agent Movements
        #region Route Waypoint Setup
        public void SetNodeWaypoints(List<Node> waypointNodes)
        {
            Debug.Log($"Test List: {waypointNodes.Count}");
            nodeWaypoints.Clear();
            currentNodeWaypointsQueue.Clear();
            currentNodeWaypointsQueue.Enqueue(currentNodePosition);
            foreach (var node in waypointNodes)
            {
                if (currentNodePosition != node)
                {
                    nodeWaypoints.Add(node);
                    currentNodeWaypointsQueue.Enqueue(node);
                }
            }
            int lastIndex = nodeWaypoints.Count - 1;
            if (lastIndex >= 0)
            {
                targetNodePosition = nodeWaypoints[lastIndex];
                // Now you have the last element in the list
            }
            //  isOnDestination = IsOnDestination();

        }

        List<Node> ReloopRoute()
        {
            var tempNodeList = new List<Node>();

            while (recentWaypoints.Count > 0)
            {
                tempNodeList.Add(recentWaypoints.Pop());
            }
            return tempNodeList;
        }

        // Wrapped dequeueing of Node Waypoints



        public bool IsOnFinalWaypoint()
        {
            if (routeIsLooped && currentNodeWaypointsQueue.Count == 0)
            {
                SetNodeWaypoints(ReloopRoute()); // This should only be called once
                return false;
            }
            return currentNodeWaypointsQueue.Count == 0;
        }


        #endregion


        #region Use Waypoints
        /// <summary>
        /// Dequeue a Node to be used
        /// Store it to a stack
        /// </summary>
        /// <returns></returns>
        Node UseNextRouteNodeWaypoint()
        {
            var nextWaypoint = currentNodeWaypointsQueue.Dequeue();
            nodeWaypoints.Remove(nextWaypoint);
            recentWaypoints.Push(nextWaypoint);
            return nextWaypoint;
        }
        /// <summary>
        /// This wraps the function of using Nodes to be used on Movements or Routes. Probably ALso maybe used on Pathfinding
        /// </summary>
        /// <returns></returns>
        public Node GetNextRouteNodeWaypoint()
        {
            Debug.Log($"Current Nodes: {currentNodeWaypointsQueue.Count}");
            if (routeIsLooped && currentNodeWaypointsQueue.Count == 0)
            {
                SetNodeWaypoints(ReloopRoute());
                Debug.Log(" Looped Route");
                return UseNextRouteNodeWaypoint();
            }
            if (currentNodeWaypointsQueue.Count > 0)
            {
                return UseNextRouteNodeWaypoint();

            }
            else
            {
                // OnDestination event can be invoked here

                return null;
            }
        }

        #endregion

        // public void MoveToNodeDebug()
        // {
        //     // Check available Waypoint Nodes, if 0 then check if it is looping
        //     if (nodeWaypointsQueue.Count > 0)
        //     {
        //         var nextWaypoint = nodeWaypointsQueue.Dequeue();
        //         nodeWaypoints.Remove(nextWaypoint);
        //         recentWaypoints.Push(nextWaypoint);
        //         MoveTo(nextWaypoint);
        //     }

        //     // IF Route is looped and nodes are empty THEN check 
        //     if (routeIsLooped && nodeWaypoints.Count == 0)
        //     {
        //         SetNodeWaypoints(ReloopRoute());
        //     }
        //     else
        //     {
        //         Debug.Log("Agent on final Node");
        //         onWhenInFinalDestination?.Invoke();
        //     }

        //     isOnDestination = IsOnDestination();

        // }

    }
}