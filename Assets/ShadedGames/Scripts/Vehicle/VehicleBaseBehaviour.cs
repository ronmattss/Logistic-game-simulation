using ShadedGames.Scripts.AgentSystem;
using ShadedGames.Scripts.Grid_System;
using ShadedGames.Scripts.Vehicles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadedGames.Scripts.Vehicles
{
    public class VehicleBaseBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update

        // Embark Disembark Passengers
        public VehicleProperties vehicleProperties;
       [SerializeField] List<PassengerBehaviour> passengerGameObjects = new List<PassengerBehaviour>();
        [SerializeField] List<Node> passengerStopNodes = new List<Node>();

        [SerializeField] int numberOfPassengersAllowed;
        [SerializeField] int currentNumberOfPassengers = 0;
        [SerializeField] AgentRouteManager agentRouteManager;
        [SerializeField] Dictionary<PassengerBehaviour, Node> passengerKeyValuePairs = new Dictionary<PassengerBehaviour, Node>();
        [SerializeField] Node currentNode;
        [SerializeField] Node nextNode;

        void Awake()
        {
            agentRouteManager = GetComponent<AgentRouteManager>();
            numberOfPassengersAllowed = vehicleProperties.GetPassengerCapacity();
            passengerGameObjects.Capacity = numberOfPassengersAllowed;
        }
        public void SetCurrentNodeLocation(Node currentNodeDestination)
        {
            currentNode = currentNodeDestination;
        }
        public void SetNextNodeLocation(Node nextNodeDestination)
        {
            nextNode = nextNodeDestination;
        }

        // Update is called once per frame
        void Update() 
        {

        }
        // This will go with a new State
        // thjis will be like a request to the passenger
        void EmbarkPassenger(PassengerBehaviour passengerGameObject)
        {
            if(currentNumberOfPassengers <= numberOfPassengersAllowed) 
            {
                passengerKeyValuePairs.Add(passengerGameObject, passengerGameObject.passengerNodeDestination);
                passengerGameObjects.Add(passengerGameObject);
                // add node to stop or Get the Waypoints of route if the node is there
                // if node is in route add passenger
                // add node to the passenger stop nodes
            }
        }

        // and this
        void DisembarkPassenger(PassengerBehaviour passengerGameObject)
        {
            passengerGameObjects.Remove(passengerGameObject);
            passengerKeyValuePairs.Remove(passengerGameObject);
            passengerGameObjects.TrimExcess();
            passengerKeyValuePairs.TrimExcess();
            currentNumberOfPassengers = passengerGameObjects.Count;
        }

        // Passenger Checks if the current Vehicle goes to their Destination
       public bool CheckIfPassengerNodeDestinationExistOnWaypoints(Node nodeToCheck)
        {
            var result = agentRouteManager.GetWaypointList().Find(x => x == nodeToCheck);
            return result;
        }
    }
}