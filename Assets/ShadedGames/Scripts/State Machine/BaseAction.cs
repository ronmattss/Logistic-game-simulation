using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    public abstract class BaseAction : ScriptableObject, IAction
    {
        public abstract void Execute();
    }



}