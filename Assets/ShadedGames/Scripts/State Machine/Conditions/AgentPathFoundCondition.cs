using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    /// <summary>
    /// Condition used when a path is found when requesting a path
    /// </summary>
    /// 

    [CreateAssetMenu(fileName = "new FSM Agent Path Found Condition", menuName = "FSM/Conditions/Agent Path Found Condition")]
    public class AgentPathFoundCondition : BaseCondition
    {
        
        public override bool Evaluate(StateMachineDriver driver)
        {
            var driverRouteManager = driver.GetComponent<AgentRouteManager>();

            return driverRouteManager.GetPathFound();
        }
    }



}