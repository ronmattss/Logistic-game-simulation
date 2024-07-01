using ShadedGames.Scripts.AgentSystem;
using ShadedGames.Scripts.Vehicles;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new Stop At Next Waypoint Action", menuName = "FSM/Actions/Stop at Next Waypoint Action")]
    public class AgentStopAtNextWaypointAction : BaseAction
    {
        public bool isStopping;
        public override void Execute(StateMachineDriver driver)
        {
            var vehicle = driver.GetComponent<VehicleBaseBehaviour>();
            var agent = driver.GetComponent<AgentRouteManager>();
            // Set passenger Load and unload time 
            var nextWaypoint = agent.CheckNextNodeWaypoint(); // now check for passengers to disembark
            vehicle.useTickRate = isStopping;
            

        }
    }






}