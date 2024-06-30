using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new FSM Agent Check If Next Waypoint Is Stop Waypoint Condition", menuName = "FSM/Conditions/Is Next Waypoint Stop Node Condition")]
    public class AgentStopAtNextWaypointCondition : BaseCondition
    {

        /// <summary>
        ///  NOT FINISH
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public override bool Evaluate(StateMachineDriver driver)
        {
            var driverRouteManager = driver.GetComponent<AgentRouteManager>();

            return driverRouteManager.GetPathFound();
        }
    }





}