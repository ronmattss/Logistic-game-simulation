using System.Collections;
using System.Collections.Generic;
using ShadedGames.Scripts.Grid_System;
using UnityEngine;
using UnityEngine.Events;


namespace ShadedGames.Scripts.AgentSystem
{

    /// <summary>
    /// Agent movement
    /// Agents will not move via navmesh BUT MOVE in the created GRID SYSTEM. this means the GRID should be static.
    /// Stores start, waypoints, end cells. 
    /// HANDLES MOVEMENT LOGIC, BUT NOT THE MOVEMENT BEHAVIOR ITSELF, THIS WILL BE IN THE AGENT BEHAVIOR
    /// </summary>
    public class AgentMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 currentGridPosition;
        [SerializeField] private Cell currentCellPosition;
        [SerializeField] private Node currentNodePosition;
        [SerializeField] private Cell targetCellPosition;
        [SerializeField] private Node targetNodePosition;
        private Queue<Cell> cellWaypointsQueue = new Queue<Cell>(); // this is Set by path finders or routers
        private Queue<Node> nodeWaypointsQueue = new Queue<Node>(); // this is Set by path finders or routers
        [SerializeField] private List<Cell> cellWaypoints = new List<Cell>(); // this is Set by path finders or routers
        [SerializeField] private List<Node> nodeWaypoints = new List<Node>(); // this is Set by path finders or routers
        [SerializeField] private Stack<Node> recentWaypoints = new Stack<Node>(); // Stores recent Route, if route is looped, Pop to nodeWayPoints
        private int currentSpeed = 1;
        private bool isOnDestination = false;







        // DEBUG ZONE;
        [SerializeField] Agent agent;
        [SerializeField] bool routeIsLooped = true;
        private int debugCurrentSpeed = 1;
        private float tickRate = 1;
        private float timer = 0f;
        // FOR MOVEMENT, should we use nav mesh OR just node paths?
        // for simplicity we gonna just use the most basic one.


        void Awake()
        {
            agent = GetComponent<Agent>();
            tickRate = debugCurrentSpeed;

            SetGridPosition();
        }

        public bool GetIsOnDestination() => agent.GetAgentRouteManager().IsOnFinalWaypoint();


        public Vector3 GetCurrentGridPosition() => currentGridPosition;
        public void SetCurrentAgentGridPosition(Vector3 gridPosition) => currentGridPosition = gridPosition;

        void SetCurrentCellPosition()
        {
            currentGridPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

            currentCellPosition = GridSystem.Instance.GetCellOnGrid(currentGridPosition);
            //    currentNodePosition = GridSystem.Instance.GetCellOnGrid(currentGridPosition).GetNode();
        }

        void SetGridPosition()
        {
            currentCellPosition = GridSystem.Instance.GetCellOnGrid(this.transform.position);
            currentNodePosition = currentCellPosition.GetNode();
        }


        public void MoveToCellDebug()
        {

            if (cellWaypointsQueue.Count > 0)
            {
                var nextWaypoint = cellWaypointsQueue.Dequeue();
                cellWaypoints.Remove(nextWaypoint);
                MoveTo(nextWaypoint);
            }
            else
            {
                Debug.Log("Agent on final Cell");
            }
        }

        // This directs the Agent on where to go
        // The brain of the script basically
        // Waypoints should be editable, in case you need to go somewhere

        // DO NOT CALL THIS IF YOU ARE NOT IN MOVING STATE
        public void MoveToNextWaypointDebug()
        {
            var waypoint = agent.GetAgentRouteManager().GetNextRouteNodeWaypoint();

            if (waypoint != null)
            {
                MoveTo(waypoint);
                isOnDestination = false;
            }
            else
            {
                Debug.Log("Agent on final Node");
                isOnDestination = true;
                agent.GetAgentRouteManager().OnFinalWaypoint.Invoke(); //  This still does not make any sense to invoke this here

            }

        }




        // This is debug Move Cell moving
        // THIS IS WHERE THE MOVING HAPPENS
        public void MoveTo(Cell cell)
        {
            currentCellPosition = cell;
            this.transform.position = new Vector3(currentCellPosition.transform.position.x, 1, currentCellPosition.transform.position.z);
        }
        public void MoveTo(Node node)
        {
            currentNodePosition = node;
            this.transform.position = new Vector3(currentNodePosition.transform.position.x, 1, currentNodePosition.transform.position.z);
        }





        // Get the current Cell position from the GridSystem
        public void SetGridCellPosition()
        {

        }


        private void FixedUpdate()
        {
            //DebugMoveUpdate();
        }

        public void DebugMoveUpdate()
        {
            timer += Time.fixedDeltaTime;

            if (timer >= tickRate)
            {
                Debug.Log("1 second Tick");
                // MoveToCellDebug();
                // MoveToNodeDebug();
                MoveToNextWaypointDebug();
                timer = 0;
            }
        }


        // Create a Dynamic virtual function that can be overwritten based on how it will move?

    }
}