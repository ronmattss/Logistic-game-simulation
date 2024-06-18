using System.Collections;
using System.Collections.Generic;
using ShadedGames.Scripts.AgentSystem;
using ShadedGames.Scripts.StateMachine.AgentStates;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{

    /// <summary>
    /// Nothing much to see here
    /// </summary>
    public abstract class StateMachineDriver : MonoBehaviour
    {
        public BaseState initialState;
        private BaseState currentState;

        protected virtual void Awake()
        {
            currentState = initialState;
            currentState.Enter();
        }

        protected virtual void Update()
        {
            currentState.Process(this);
        }

        public void ChangeState(BaseState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

}