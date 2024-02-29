

using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine.AgentStates
{
    public class AgentStateMoving : StateMachine
    {
        AgentMovement agentMovement;
        public AgentStateMoving(Agent _agent, GameObject _agentGameObject) : base(_agent, _agentGameObject)
        {
            currentStateName = CurrentState.MOVING;
            agentMovement = agent.GetAgentBehaviour().GetAgentMovement();
        }

        public override void Enter()
        {
            base.Enter();
            // Move to 
        }

        public override void Update()
        {
            base.Update();
            if (agentMovement.GetIsOnDestination())
            {
                nextState = new AgentStateIdle(this.agent, this.agentGameObject);
                stateStatus = StateProcess.EXIT;
                return;
            }
            else
            {
                agentMovement.DebugMoveUpdate(); // THIS WILL BE CHANGED
                return;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

}