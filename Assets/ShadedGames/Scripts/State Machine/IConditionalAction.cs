namespace ShadedGames.Scripts.StateMachine
{
    public interface IConditionalAction : ICondition, IAction
    {
        public void Execute(StateMachineDriver driver);
    }



}