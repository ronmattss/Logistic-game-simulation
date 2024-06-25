using ShadedGames.Scripts.AgentSystem;
using ShadedGames.Scripts.EventSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    /// <summary>
    /// Condition used to transition to Idle State
    /// </summary>
    /// 
    [CreateAssetMenu(fileName = "new FSM is On Destination Condition", menuName = "FSM/Conditions/Agent is On Destination Condition")]
    public class AgentIsOnDestinationCondition : BaseCondition
    {

        public override bool Evaluate(StateMachineDriver driver)
        {
            var driverAgentMovement = driver.GetComponent<AgentMovement>();
           // driver.GetComponent<Agent>().GetAgentRouteManager().ClearAStarNodeWaypoints();
            return driverAgentMovement.GetIsOnDestination();
        }
    }



}