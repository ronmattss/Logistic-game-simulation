using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "Idle Action Exit Disable Path Found", menuName = "FSM/Actions/Idle Exit Action Disable Path Found")]
    public class AgentSetPathFound : BaseAction
    {
        public bool setPathFound = false;
        public override void Execute(StateMachineDriver driver)
        {
            driver.GetComponent<AgentRouteManager>().SetPathFound(setPathFound);

        }
    }
}