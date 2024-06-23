using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new Move To First Waypoint Action", menuName = "FSM/Actions/Move To First Waypoint Action")]
    public class AgentMoveToFirstWaypointAction : BaseAction
    {        
        public override void Execute(StateMachineDriver driver)
        {
           driver.GetComponent<AgentMovement>().MoveToFirstWaypoint();
        }
    }

    


}