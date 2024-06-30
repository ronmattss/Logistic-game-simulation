using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadedGames.Scripts.Vehicles
{
    /// <summary>
    /// Properties of a Vehicle
    /// </summary>

    [CreateAssetMenu(fileName = "new Vehicle Properties", menuName = "Vehicles/Simple Vehicle Properties")]
    [Serializable]
    public class VehicleProperties : ScriptableObject
    {
      [SerializeField] private  float speed;
      [SerializeField] private  float acceleration;
      [SerializeField] private  float brakingForce;
      [SerializeField] private  float fuelCapacity;
      [SerializeField] private  int passengerCapacity;

        public float GetSpeed() => speed;
        public float GetAcceleration() => acceleration;
        public float GetBrakingForce() => brakingForce;
        public float GetFuelCapacity() => fuelCapacity;
        public int GetPassengerCapacity() => passengerCapacity;


    }
}
