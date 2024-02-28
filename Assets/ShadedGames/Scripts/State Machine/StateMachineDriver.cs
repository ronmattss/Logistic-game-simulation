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
    public class StateMachineDriver : MonoBehaviour
    {
        // Start is called before the first frame update

        private StateMachine baseState; 
        void Start()
        {
        }

        private void Awake()
        {
            // What we can do is create State classes for each agents
            baseState = new AgentStateIdle(this.gameObject.GetComponent<Agent>(), this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            baseState = baseState.Process();
        }
    }

}