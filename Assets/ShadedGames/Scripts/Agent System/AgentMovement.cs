using System.Collections;
using System.Collections.Generic;
using ShadedGames.Scripts.Grid_System;
using UnityEngine;
using UnityEngine.AI;
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
        [SerializeField] private int currentSpeed = 6;
        [SerializeField] private int distanceToNodeCheck = 4;
        [SerializeField] private bool isOnDestination = false;


        // for simplicity we gonna just use the most basic one.
        public void SetCurrentNodePosition(Node node) => currentNodePosition = node;
        public void SetTargetNodePosition(Node node) => targetNodePosition = node;

        public bool GetIsOnDestination() =>  isOnDestination;



        // DEBUG ZONE;
        [SerializeField] Agent agent;
        [SerializeField] private NavMeshAgent meshAgent;

        [SerializeField] bool routeIsLooped = true;
        private int debugCurrentSpeed = 1;
        private float tickRate = 1;
        private float timer = 0.25f; // update Movement info every .25f seconds
        // FOR MOVEMENT, should we use nav mesh OR just node paths?



        void Awake()
        {
            agent = GetComponent<Agent>();
            tickRate = debugCurrentSpeed;

            SetGridPosition();
        }

       /* public bool GetIsOnDestination() => agent.GetAgentRouteManager().IsOnFinalWaypoint();*/


        public Vector3 GetCurrentGridPosition() => currentGridPosition;
        public void SetCurrentAgentGridPosition(Vector3 gridPosition) => currentGridPosition = gridPosition;


        void SetGridPosition()
        {
            currentCellPosition = GridSystem.Instance.GetCellOnGrid(this.transform.position);
            currentNodePosition = currentCellPosition.GetNode();
        }



        // This directs the Agent on where to go
        // The brain of the script basically
        // Waypoints should be editable, in case you need to go somewhere







        // Using Navmesh Agent Move To Waypoint
        // If tehre are multiple waypoints, Check if agent is near the waypoint
        // if it is change the waypoint to the Next one.
        public void MoveToWaypoint() 
        {
            var waypoint = agent.GetAgentRouteManager().GetNextRouteNodeWaypoint();
            if ( waypoint != null)
            {
                meshAgent.Move(waypoint.transform.position);
                isOnDestination = false;
            }
        }







        private void FixedUpdate()
        {
            //DebugMoveUpdate();
        }




        public void MoveToFirstWaypoint()
        {
            meshAgent.speed = currentSpeed;
            if(agent.GetAgentRouteManager().CheckIfThereAreWaypointsAvailable())
            {
                currentNodePosition = agent.GetAgentRouteManager().GetNextRouteNodeWaypoint();
                Debug.Log($"Moving to First Node: {currentNodePosition.transform.position}");
                agent.GetAgentRouteManager().SetPathFound(false);
                isOnDestination = false;
                meshAgent.isStopped = false;
                meshAgent.SetDestination(currentNodePosition.transform.position);

            }
            else
            {
                Debug.Log("First Waypoint is Last Waypoint");
                isOnDestination = true;
            }
        }

        // Will be Called on AgentStateMovement
        public void MovementUpdate()
        {            
           

              //  Debug.Log($"is current Node Position null: {currentNodePosition == null} Distance: {Vector3.Distance(currentNodePosition.transform.position, this.transform.position)}");
                if (currentNodePosition != null && Vector3.Distance(currentNodePosition.transform.position, this.transform.position) <= distanceToNodeCheck) // Now this should trigger when changing Waypoints
                {
                    if(agent.GetAgentRouteManager().GetNextRouteNodeWaypoint() != null)
                    {
                        currentNodePosition = agent.GetAgentRouteManager().GetNextRouteNodeWaypoint();
                        // This gives an error if Current  node position is null lmao
                        meshAgent.SetDestination(currentNodePosition.transform.position);
                        Debug.Log($"Moving to Node: {currentNodePosition.transform.position}");
                        isOnDestination = false;
                    }
                    else
                    {
                        meshAgent.isStopped = true;
                        isOnDestination = true;
                    }
                }
  

        }

        public void DebugMoveUpdate()
        {
            timer += Time.fixedDeltaTime;

            if (timer >= tickRate)
            {
                Debug.Log("1 second Tick");

                MovementUpdate();
                timer = 0;
            }
        }


        // Create a Dynamic virtual function that can be overwritten based on how it will move?

    }
}