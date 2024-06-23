using System.Collections;
using System.Collections.Generic;
using ShadedGames.Scripts.AgentSystem;
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
        [SerializeField] private string currentStateName;
        public void SetStateName(string currentState) => currentStateName = currentState;
        protected virtual void Awake()
        {
            currentState = initialState;
            currentState.Enter(this);
        }

        protected virtual void Update()
        {
            currentState.Process(this);
        }

        public void ChangeState(BaseState newState)
        {
            currentState.Exit(this);
            currentState = newState;
            currentState.Enter(this);
        }
        public void DisplayCurrentState()
        {
            SetStateName(currentState.name);
            Debug.Log($"Current State: {currentStateName}");
        }
    }

}