using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    public abstract class ConditionalAction : ScriptableObject, IConditionalAction
    {
        public abstract bool Evaluate(StateMachineDriver driver);
        public abstract void Execute(StateMachineDriver driver);
    }



}