using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new Move To Next Waypoint Action", menuName = "FSM/Actions/Move To Next Waypoint Action")]
    public class AgentMoveToNextWaypointAction : BaseAction
    {
        public override void Execute(StateMachineDriver driver)
        {
            driver.GetComponent<AgentMovement>().MovementUpdate();

        }
    }






}