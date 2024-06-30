using ShadedGames.Scripts.AgentSystem;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    [CreateAssetMenu(fileName = "new At Next Waypoint Action", menuName = "FSM/Actions/Stop At Next Waypoint Action")]
    public class AgentStopAtNextWaypointAction :BaseAction
    {
        public override void Execute(StateMachineDriver driver)
        {
            driver.GetComponent<AgentMovement>().MovementUpdate();

        }
    }






}