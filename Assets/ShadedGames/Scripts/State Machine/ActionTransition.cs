using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new FSM Action Transition", menuName = "FSM/Action Transition/Simple Action Transition")]
    public class ActionTransition: ScriptableObject
    {
        public BaseCondition condition;
        public BaseAction trueAction;
        public BaseAction falseAction;
    }



}