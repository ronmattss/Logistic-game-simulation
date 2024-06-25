using ShadedGames.Scripts.AgentSystem;
using ShadedGames.Scripts.Managers;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new Request Random Path Action", menuName = "FSM/Actions/Random Path Action")]
    public class AgentRequestRandomPathAction : BaseAction
    {

       
        public override void Execute(StateMachineDriver driver)
        {
            PathGenerator.Instance.StartRequestRandomRouteCoroutine(driver.GetComponent<Agent>());
        }
    }




}