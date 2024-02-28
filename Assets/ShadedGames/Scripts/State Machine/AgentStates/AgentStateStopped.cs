

using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine.AgentStates
{
    public class AgentStateStopped : StateMachine
    {
        public AgentStateStopped(Agent _agent, GameObject _agentGameObject) : base(_agent, _agentGameObject)
        {
            currentStateName = CurrentState.IDLE;
        }
        public override void Enter()
        {
            base.Enter();
            // Move to 
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

}