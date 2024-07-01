using ShadedGames.Scripts.AgentSystem;
using ShadedGames.Scripts.Vehicles;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new FSM Agent Check If Next Waypoint Is Stop Waypoint Condition", menuName = "FSM/Conditions/Is Next Waypoint Stop Node Condition")]
    public class AgentCheckNextWaypointCondition : BaseCondition
    {

        /// <summary>
        ///  Condition to check if Next Node is a Stop Node,
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public override bool Evaluate(StateMachineDriver driver)
        {
            var driverRouteManager = driver.GetComponent<AgentRouteManager>();
            var driverVehicleBehaviour = driver.GetComponent<VehicleBaseBehaviour>();

            // Test Checking
            // var isNextWaypointAStopWayNode = driverVehicleBehaviour.GetPassengerKeyValuePairs().ContainsValue(driverRouteManager.CheckNextNodeWaypoint()); // this should be used
            var isNextWaypointAStopWayNode = driverVehicleBehaviour.passengerStopNodes.Contains(driverRouteManager.CheckNextNodeWaypoint());

            driver.GetComponent<AgentMovement>().SetIsStopping(isNextWaypointAStopWayNode);
            // CHeck if next Waypoint is a Stop Point
            // Get next Stop Node

            Debug.Log($"Transitioning to Stopping State Test: {isNextWaypointAStopWayNode}");
            return isNextWaypointAStopWayNode;
        }
    }
}