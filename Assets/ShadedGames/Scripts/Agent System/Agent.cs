using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace ShadedGames.Scripts.AgentSystem
{

    /// <summary>
    /// Agent is the base GameObject for things that move in the grid
    /// It will have the properties of a basic Physics object
    /// Extend as you like
    /// </summary>
    public class Agent : MonoBehaviour
    {
        private Vector3 currentWorldPosition;
        private int traverseSpeed;

        private Rigidbody rigidbody;
        private AgentBehaviour agentBehaviour;


        // Agent States


        // FOR MOVEMENT, should we use nav mesh OR just node paths?
        // for simplicity we gonna just use the most basic one.


        public Vector3 GetCurrentAgentWorldPosition() => currentWorldPosition;
        public void SetCurrentAgentWorldPosition(Vector3 worldPosition) => currentWorldPosition = worldPosition;
        public AgentBehaviour GetAgentBehaviour() => agentBehaviour;

        // Create a Dynamic virtual function that can be overwritten based on how it will move?



        // Start is called before the first frame update
        void Awake()
        {
            agentBehaviour = GetComponent<AgentBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}