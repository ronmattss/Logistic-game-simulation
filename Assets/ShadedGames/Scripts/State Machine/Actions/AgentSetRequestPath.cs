using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "Movement Action Exit Set Request Path", menuName = "FSM/Actions/Exit Set Request Path")]
    public class AgentSetRequestPath : BaseAction
    {
        public bool setRequestPath = false;
        public override void Execute(StateMachineDriver driver)
        {
            driver.GetComponent<Agent>().requestedPath = setRequestPath;

        }
    }
}