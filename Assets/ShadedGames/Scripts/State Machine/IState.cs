namespace ShadedGames.Scripts.StateMachine
{
    public interface IState
    {
        void Enter();
        void Process(StateMachineDriver driver);
        void Exit();
    }



}