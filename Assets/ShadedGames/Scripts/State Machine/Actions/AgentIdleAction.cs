using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new Sample Action", menuName = "FSM/Actions/Sample Action")]
    public class AgentIdleAction : BaseAction
    {

        public string sampleString = "";
        public override void Execute(StateMachineDriver driver)
        {
            Debug.Log($"Sample String Text: {sampleString}");
        }
    }




}