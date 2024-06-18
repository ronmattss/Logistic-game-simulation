using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public class Transition : ScriptableObject
    {
        public BaseCondition condition;
        public BaseState nextState;
    }



}