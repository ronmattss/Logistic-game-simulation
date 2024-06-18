using System.Collections.Generic;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    public abstract class BaseState : ScriptableObject, IState
    {
        public List<BaseAction> entryActions;
        public List<BaseAction> updateActions;
        public List<BaseAction> exitActions;
        public List<Transition> transitions;

        public virtual void Enter()
        {
            foreach (var action in entryActions)
            {
                action.Execute();
            }
        }

        public virtual void Process(StateMachineDriver driver)
        {
            foreach (var action in updateActions)
            {
                action.Execute();
            }

            foreach (var transition in transitions)
            {
                if (transition.condition.Evaluate())
                {
                    driver.ChangeState(transition.nextState);
                    break;
                }
            }
        }

        public virtual void Exit()
        {
            foreach (var action in exitActions)
            {
                action.Execute();
            }
        }
    }



}