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
        [SerializeField] private bool isOnDestination = false;

        [Header("Manual Speed Control Related Properties")]
        [SerializeField] private bool isManualControlled = false;
        [SerializeField] private float currentSpeed = 6;
        [SerializeField] private int distanceToNodeCheck = 4;
        [SerializeField] private float accelerationRate = 5f;
        [SerializeField] private float decelerationRate = 5f;
        [SerializeField] private float maxSpeed = 20f;
        [SerializeField] private float tickRate = 0.1f;
        [SerializeField] private float timer = 0f;

        [Header("Automatic Speed Control Related Properties for NPC")]
        [SerializeField] private float variableSpeed;
        [SerializeField] private float variableDistanceToNodeCheck;


        // for simplicity we gonna just use the most basic one.
        public void SetCurrentNodePosition(Node node) => currentNodePosition = node;

        public bool GetIsOnDestination() => isOnDestination;

        public Node GetCurrentNodePosition() => currentNodePosition;


        // DEBUG ZONE;
        [SerializeField] Agent agent;
        [SerializeField] private NavMeshAgent meshAgent;

        [SerializeField] bool routeIsLooped = true;
        private int debugCurrentSpeed = 1;

        // update Movement info every .25f seconds
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
            DebugMoveUpdate();
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
                if(!isManualControlled)
                {
                CheckDirection();
                }


            }

        }

        public void DebugMoveUpdate()
        {
            timer += Time.deltaTime;

            if (timer >= tickRate)
            {
                // Simulate pressing the acceleration and deceleration buttons for debugging
                if (Input.GetKey(KeyCode.W))
                {
                    AccelerateVehicle();
                }
                if (Input.GetKey(KeyCode.S))
                {
                    DecelerateVehicle();
                }

                timer = 0f;
            }
        }

        public void AccelerateVehicle()
        {
            // Increase the current speed by the acceleration rate
            currentSpeed += accelerationRate * Time.deltaTime;
            // Clamp the speed to not exceed the max speed
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

            ChangeSpeed();
        }

        public void DecelerateVehicle()
        {
            // Decrease the current speed by the deceleration rate
            currentSpeed -= decelerationRate * Time.deltaTime;
            // Clamp the speed to not fall below 0
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

            ChangeSpeed();
        }
        public void ChangeSpeed()
        {
            // Apply the current speed to the vehicle
            // This is where you would add code to update the vehicle's velocity or position
            Debug.Log($"Current Speed: {currentSpeed}");
            meshAgent.speed = currentSpeed;
        }


        // Create a Dynamic virtual function that can be overwritten based on how it will move?

    }
}