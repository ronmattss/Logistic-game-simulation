using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        [SerializeField] private int currentSpeed = 6;
        [SerializeField] private int distanceToNodeCheck = 4;
        [SerializeField] private bool isOnDestination = false;


        // for simplicity we gonna just use the most basic one.
        public void SetCurrentNodePosition(Node node) => currentNodePosition = node;

        public bool GetIsOnDestination() => isOnDestination;

        public Node GetCurrentNodePosition() => currentNodePosition;


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

           // SetGridPosition();
        }

        /* public bool GetIsOnDestination() => agent.GetAgentRouteManager().IsOnFinalWaypoint();*/


        public Vector3 GetCurrentGridPosition() => currentGridPosition;
        public void SetCurrentAgentGridPosition(Vector3 gridPosition) => currentGridPosition = gridPosition;
        public Node GetAgentCurrentNodePosition() => currentNodePosition;


       public  void SetGridPosition()
        {
            var point = this.transform.position;
            int x = Mathf.FloorToInt(point.x / 10);
            int z = Mathf.FloorToInt(point.z / 10);
            string gameObjectNodeName = $"{x}{z}";
            Debug.Log($"Game Object Name: {gameObjectNodeName} RAW Coords: {point.x} {point.z}");
            currentCellPosition = GridSystem.Instance.GetCellViaNameOnGridList(gameObjectNodeName);
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
            if (waypoint != null)
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
            if (agent.GetAgentRouteManager().CheckIfThereAreWaypointsAvailable())
            {
                SetDestinationToMeshAgent();

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
                SetDestinationToMeshAgent();
            }


        }

         void SetDestinationToMeshAgent()
        {            
            currentNodePosition = agent.GetAgentRouteManager().GetNextRouteNodeWaypoint();
            if(currentNodePosition == null)
            {
                meshAgent.isStopped = true;
                isOnDestination = true;
            }
            else
            {
                // This gives an error if Current  node position is null lmao
                meshAgent.SetDestination(currentNodePosition.transform.position);
                isOnDestination = false;
                meshAgent.isStopped = false;

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