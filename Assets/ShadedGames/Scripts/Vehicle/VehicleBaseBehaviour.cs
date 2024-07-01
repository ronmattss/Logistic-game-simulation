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
       [SerializeField] public List<PassengerBehaviour> passengerGameObjects = new List<PassengerBehaviour>();
        [SerializeField] public List<Node> passengerStopNodes = new List<Node>();

        [SerializeField] int numberOfPassengersAllowed;
        [SerializeField] int currentNumberOfPassengers = 0;
        [SerializeField] AgentRouteManager agentRouteManager;
        [SerializeField] Dictionary<PassengerBehaviour, Node> passengerKeyValuePairs = new Dictionary<PassengerBehaviour, Node>();
        [SerializeField] Node currentNode;
        [SerializeField] Node nextNode;

        public bool useTickRate = false;
        public float countdownTime = 10f; // Time to countdown from something (e.g Loading / unloading)
        public float tickRate = 1f; // Tick rate in seconds
        private float timer = 0f;

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
        public Dictionary<PassengerBehaviour, Node> GetPassengerKeyValuePairs() => passengerKeyValuePairs;

        // Update is called once per frame

        // This will go with a new State
        // thjis will be like a request to the passenger
        public void SetCountDownTimer(float timerCountDown)
        {
            countdownTime = timerCountDown;
        }
        void EmbarkPassenger(PassengerBehaviour passengerGameObject)
        {
            if(currentNumberOfPassengers <= numberOfPassengersAllowed) 
            {
                passengerGameObjects.Add(passengerGameObject);
                
                passengerKeyValuePairs.Add(passengerGameObject, passengerGameObject.passengerNodeDestination);
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

        void Update()
        {
            if (!useTickRate) return;
            timer += Time.deltaTime;

            if (timer >= tickRate)  // Call function every one second
            {
                Countdown();
                timer = 0f;
            }
        }



        void Countdown()
        {
            if (countdownTime > 0)
            {
                countdownTime -= tickRate;
                Debug.Log("Countdown: " + countdownTime);

                if (countdownTime <= 0)
                {
                    countdownTime = 0;
                    OnCountdownEnd();
                }
            }
        }

        void OnCountdownEnd()
        {
            Debug.Log("Countdown finished!");
            // Add additional logic here for when the countdown finishes
        }


    }
}