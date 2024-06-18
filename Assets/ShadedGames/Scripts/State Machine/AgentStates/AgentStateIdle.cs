

using ShadedGames.Scripts.AgentSystem;
using ShadedGames.Scripts.Grid_System;
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
            Debug.Log($"Agent Node Position: {agent.gameObject.transform.position} {Mathf.Floor(agent.gameObject.transform.position.x / 10) } {Mathf.Floor(agent.gameObject.transform.position.z / 10)}");
            var currentNode = GridSystem.Instance.GetCellOnGridWithRawCoordinates((int)Mathf.Floor(agent.gameObject.transform.position.x / 10), (int)Mathf.Floor(agent.gameObject.transform.position.z / 10));
            Debug.Log($"is current Node null: {currentNode == null}");
            if(currentNode != null)
            {
                agentMovement.SetCurrentNodePosition(currentNode.GetNode());
                agent.GetAgentRouteManager().SetCurrentNodePosition(currentNode.GetNode());
            }
        }

        public override void Update()
        {
            base.Update();
            // DO THINGS while on IDLE State.
            // LIKE WAITING FOR A TASK



            if (!agentMovement.GetIsOnDestination()) // CHECK IF THERE ARE AVAILABLE NODES, REFACTOR THIS
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