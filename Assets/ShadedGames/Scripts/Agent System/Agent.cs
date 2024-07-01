using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using ShadedGames.Scripts.StateMachine;


namespace ShadedGames.Scripts.AgentSystem
{

    /// <summary>
    /// Agent is the base GameObject for things that move in the grid
    /// It will have the properties of a basic Physics object
    /// Extend as you like, it just make sense that this is the entry point if you need things from the Agent
    /// </summary>
    public class Agent : MonoBehaviour
    {
        private Vector3 currentWorldPosition;
        private int traverseSpeed;
        private AgentStateMachine agentBehaviour;

        [SerializeField] AgentRouteManager agentRouteManager;


        [SerializeField] Task task;

        public AgentRouteManager GetAgentRouteManager() => agentRouteManager;

        public bool requestedPath = false;
        


        // Current Task? 

        // When Agent is onDestination Update something here
        // Properties
        // BRAIN OF THE GAME OBJECT?
        // Handles Transition Function for the FSM ( Decides where the FSM will go)






        // Agent States


        // FOR MOVEMENT, should we use nav mesh OR just node paths?
        // for simplicity we gonna just use the most basic one.


        public Vector3 GetCurrentAgentWorldPosition() => currentWorldPosition;
        public void SetCurrentAgentWorldPosition(Vector3 worldPosition) => currentWorldPosition = worldPosition;
        public AgentStateMachine GetAgentBehaviour() => agentBehaviour;

        // Create a Dynamic virtual function that can be overwritten based on how it will move?
        // Start is called before the first frame update
        // AGENT WILL CONTAIN the tasks of the vehicle and it will be passed down to the behaviors and state machine

        void Awake()
        {
            agentBehaviour = GetComponent<AgentStateMachine>();
            agentRouteManager = GetComponent<AgentRouteManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}