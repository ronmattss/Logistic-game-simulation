using System.Collections;
using System.Collections.Generic;
using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    public class StateMachine
    {
        // STATES enum  SHOULD BE ON Present Form
        public enum CurrentState
        {
            IDLE, MOVING, STOPPED,LOADING,UNLOADING, BUSY
        }
        // These events are the three events in the behaviour loop
        public enum StateProcess
        {
            ENTER, UPDATE, EXIT
        }

        public CurrentState currentStateName;

        public CurrentState name;
        protected StateProcess stateStatus;
        protected GameObject agentGameObject;
        protected Agent agent;
        protected StateMachine nextState; // What if there are pool of states?
        // STFU WITH DYNAMIC TRANSITIONS AND PLUGGABLE FSM FOR NOW

        public StateMachine(Agent _agent, GameObject _agentGameObject)

        {
            agentGameObject = _agentGameObject;
            agent = _agent;
            stateStatus = StateProcess.ENTER;
        }

        public virtual void Enter()
        {
            Debug.Log("Entered State:" + name);
            stateStatus = StateProcess.UPDATE;
        }
        public virtual void Update()
        {
            stateStatus = StateProcess.UPDATE; // run update while no condition to exit
        }
        public virtual void Exit()
        {
            stateStatus = StateProcess.EXIT;
        }

        public StateMachine Process()
        {
            Debug.Log(LogStateStatus(currentStateName, stateStatus));
            if (stateStatus == StateProcess.ENTER) Enter();
            if (stateStatus == StateProcess.UPDATE) Update();
            if (stateStatus == StateProcess.EXIT)
            {
                Exit();
                return nextState;
            }
            return this;
        }
        public string LogStateStatus(CurrentState curState,
    StateProcess curStateProcess)
        {
            return "Current State: " + curState + " Current State Process: " + curStateProcess;
        }


        // Not sure what properties to add
        // Agent States, 
        // Gameobject ?, AgentMovement, Agent
        // STATE TRANSITIONS CAN BE MANIPULATED BY THE TASKS, 

    }

}