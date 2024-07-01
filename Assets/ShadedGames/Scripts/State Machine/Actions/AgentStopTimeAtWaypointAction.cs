using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new Stop Time At Waypoint Action", menuName = "FSM/Actions/Stop Time At Waypoint Action")]
    public class AgentStopTimeAtWaypointAction : BaseAction
    {
        // This will be in entry with slower speed
        public override void Execute(StateMachineDriver driver)
        {

            driver.GetComponent<AgentMovement>().MovementUpdate();

        }
    }







}