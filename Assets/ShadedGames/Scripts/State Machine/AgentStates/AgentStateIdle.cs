

using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine.AgentStates
{
    /// <summary>
    /// NOTE: Agents are the entities moving in the main Node, in other words, these are vehicles.
    /// </summary>
    public class AgentStateIdle : StateMachine
    {
        private AgentMovement agentMovement;
        public AgentStateIdle(Agent _agent, GameObject _agentGameObject) : base(_agent, _agentGameObject)
        {
            currentStateName = CurrentState.IDLE;
            agentMovement = _agent.GetAgentBehaviour().GetAgentMovement();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            // DO THINGS while on IDLE State.
            // LIKE WAITING FOR A TASK



            if (!agentMovement.GetIsOnDestination())
            {
                nextState = new AgentStateMoving(this.agent, this.agentGameObject);
                stateStatus = StateProcess.EXIT;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

}