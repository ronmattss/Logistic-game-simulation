namespace ShadedGames.Scripts.StateMachine
{
    public interface IState
    {
        void Enter(StateMachineDriver driver);
        void Process(StateMachineDriver driver);
        void Exit(StateMachineDriver driver);
    }



}