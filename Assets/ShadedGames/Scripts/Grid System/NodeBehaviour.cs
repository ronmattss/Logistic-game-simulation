using ShadedGames.Scripts.AgentSystem;
using System.Collections.Generic;
using UnityEngine;

namespace ShadedGames.Scripts.Grid_System
{
    public class NodeBehaviour : MonoBehaviour
    {
        // List of Passengers Waiting close to the Node
        // Current Vehicle On Node

        // Neighboring GameObjects (not nodes) that detects how much NPCs are there 
      public  List<PassengerBehaviour> passengerList = new List<PassengerBehaviour>();
      public  Agent currentVehicle;



        void AddAgent(Agent agent)
        {
            currentVehicle = agent;
        }
        public void RemoveCurrentAgent() 
        {
            currentVehicle = null;
        }

        public void AddPassenger(PassengerBehaviour currentPassenger)
        {
            passengerList.Add(currentPassenger);
        }
        public void RemovePassenger(PassengerBehaviour currentPassenger)
        {
            passengerList.Remove(currentPassenger);
            passengerList.TrimExcess();
        }
    }
}