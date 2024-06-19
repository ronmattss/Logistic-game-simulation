using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new FSM Condition", menuName = "FSM/Conditions/Simple Condition")]

    public class SampleCondition : BaseCondition
    {
        public bool sampleSwitchCondition = false;
        public override bool Evaluate(StateMachineDriver driver)
        {
            return sampleSwitchCondition;
        }
    }



}