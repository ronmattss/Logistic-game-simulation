using System.Collections;
using System.Collections.Generic;
using ShadedGames.Scripts.AgentSystem;
using ShadedGames.Scripts.StateMachine;
using ShadedGames.Scripts.StateMachine.AgentStates;
using UnityEngine;


namespace ShadedGames.Scripts.AgentSystem
{

  // NOW THIS MAKES SENSE :>
  // AGENT BEHAVIOR INHERITS STATE MACHINE DRIVER
  public class AgentBehaviour : StateMachineDriver
  {

    public bool loopCurrentRoute = false;
    private AgentMovement agentMovement;
    public AgentMovement GetAgentMovement() => agentMovement;


    public override void Awake()
    {
      agentMovement = GetComponent<AgentMovement>();
      base.Awake();
      baseState = new AgentStateIdle(this.gameObject.GetComponent<Agent>(), this.gameObject);

    }
    public override void Update()
    {
      base.Update();
    }


  }

  // Behaviours Will

}