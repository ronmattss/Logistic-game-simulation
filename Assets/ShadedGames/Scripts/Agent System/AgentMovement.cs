using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        [SerializeField] private Node previousNodePosition;

        [SerializeField] private Cell targetCellPosition;
        [SerializeField] private int currentSpeed = 6;
        [SerializeField] private int distanceToNodeCheck = 4;
        [SerializeField] private int variableSpeed;
        [SerializeField] private int variableDistanceToNodeCheck;
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
            variableSpeed = currentSpeed;
            variableDistanceToNodeCheck = distanceToNodeCheck;

           // SetGridPosition();
        }

        /* public bool GetIsOnDestination() => agent.GetAgentRouteManager().IsOnFinalWaypoint();*/


        public Node GetAgentCurrentNodePosition() => currentNodePosition;


       public  void SetGridPosition()
        {
            if (currentNodePosition != null) return;
            var point = gameObject.transform.position;
            int x = Mathf.FloorToInt(point.x / 10);
            int z = Mathf.FloorToInt(point.z / 10);
            string gameObjectNodeName = $"{x}{z}";
            Debug.Log($"Game Object Name: {gameObjectNodeName} RAW Coords: {point.x} {point.z}");
            currentCellPosition = GridSystem.Instance.GetCellViaNameOnGridList(gameObjectNodeName);
            currentNodePosition = currentCellPosition.GetNode();
        }

        public enum Direction
        {
            North,
            East,
            South,
            West,
            None
        }

        void CheckDirection()
        {
            if (agent.GetAgentRouteManager().GetWaypointList().Count == 0) return;
            var currentNode = currentNodePosition;
            var lastNode = previousNodePosition;
           // Debug.Log($"AgentName: {transform.name} Agent current Node Waypoints: {agent.GetAgentRouteManager().GetWaypointList().Count}");
            var nextNode = currentNode == agent.GetAgentRouteManager().GetWaypointList()[0] ? agent.GetAgentRouteManager().GetWaypointList()[1] : agent.GetAgentRouteManager().GetWaypointList()[0];

            var currentDirection = GetAdjacentNodeDirection(currentNode, lastNode); // Get direction of last node relative to current node
            var nextDirection = GetAdjacentNodeDirection(currentNode, nextNode);    // Get direction of next node relative to current node
            Debug.Log($"Current Direction: {currentNode} {lastNode} = {currentDirection} | Next Direction: {currentNode} {nextNode} = {nextDirection} ");
            if (!AreDirectionsAligned(currentDirection, nextDirection))
            {
                // Do Something else when directions are not aligned
                meshAgent.speed =(float) variableSpeed/2;
                variableDistanceToNodeCheck = distanceToNodeCheck + 2;
                Debug.Log($"Changing Direction {currentDirection} {nextDirection}");
            }
            else
            {
                Debug.Log($"Retaining Direction {currentDirection} {nextDirection}");

                meshAgent.speed = currentSpeed;
                variableDistanceToNodeCheck = distanceToNodeCheck;
                // retain Speed
            }
        }

        Direction GetAdjacentNodeDirection(Node currentNode, Node nodeToCheck)
        {
            var neighbors = currentNode.GetNodeNeighbors();
            if (nodeToCheck == null) return Direction.None;
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] == nodeToCheck)
                {
                    return (Direction)i;
                }
            }
            return Direction.None;
        }

        bool AreDirectionsAligned(Direction currentDirection, Direction nextDirection)
        {
            // Check if both nodes are either vertical (North or South) or horizontal (East or West)
            if ((currentDirection == Direction.North || currentDirection == Direction.South) &&
                (nextDirection == Direction.North || nextDirection == Direction.South))
            {
                return true;
            }
            if ((currentDirection == Direction.East || currentDirection == Direction.West) &&
                (nextDirection == Direction.East || nextDirection == Direction.West))
            {
                return true;
            }
            return false;
        }


        // Check Next Node if it is current Direction


        // This directs the Agent on where to go
        // The brain of the script basically
        // Waypoints should be editable, in case you need to go somewhere







        // Using Navmesh Agent Move To Waypoint
        // If tehre are multiple waypoints, Check if agent is near the waypoint
        // if it is change the waypoint to the Next one.








        private void FixedUpdate()
        {
            //DebugMoveUpdate();
        }




        public void MoveToFirstWaypoint()
        {
            meshAgent.speed = variableSpeed;
            if (agent.GetAgentRouteManager().CheckIfThereAreWaypointsAvailable())
            {
                SetDestinationToMeshAgent();
               /* previousNodePosition = currentNodePosition;*/
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

            // NOT SURE CHeckDirection is here or inside SetDestination

            //  Debug.Log($"is current Node Position null: {currentNodePosition == null} Distance: {Vector3.Distance(currentNodePosition.transform.position, this.transform.position)}");
            if (currentNodePosition != null && Vector3.Distance(currentNodePosition.transform.position, this.transform.position) <= variableDistanceToNodeCheck) // Now this should trigger when changing Waypoints
            {
                SetDestinationToMeshAgent();

            }


        }

         void SetDestinationToMeshAgent()
        {
           var nextNodePosition = agent.GetAgentRouteManager().GetNextRouteNodeWaypoint(); // changed to var so that currentNodePosition will not be null, and the last value will be the last Nodes Position
            if(nextNodePosition == null)
            {
                meshAgent.isStopped = true;
                isOnDestination = true;
            }
            else
            {
                // This gives an error if Current  node position is null lmao
                meshAgent.SetDestination(nextNodePosition.transform.position);

                currentCellPosition = nextNodePosition.GetParentCell();
                previousNodePosition = currentNodePosition;
                currentNodePosition = nextNodePosition;
                agent.GetAgentRouteManager().SetCurrentNodePosition(currentNodePosition);

                isOnDestination = false;
                meshAgent.isStopped = false;
                CheckDirection();


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