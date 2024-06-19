namespace ShadedGames.Scripts.StateMachine
{
    public interface ICondition
    {
        bool Evaluate(StateMachineDriver driver);
    }



}