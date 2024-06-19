using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new FSM Transition", menuName = "FSM/Transition/Simple Transition")]

    public class Transition : ScriptableObject
    {
        public BaseCondition condition;
        public BaseState nextState;
    }
   




}