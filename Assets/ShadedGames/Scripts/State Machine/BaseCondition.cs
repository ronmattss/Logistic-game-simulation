using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    public abstract class BaseCondition : ScriptableObject, ICondition
    {
        public abstract bool Evaluate();
    }



}