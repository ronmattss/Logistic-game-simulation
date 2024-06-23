using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShadedGames.Scripts.StateMachine
{
    public abstract class BaseState : ScriptableObject, IState
    {
        public List<BaseAction> entryActions;   // actions performed when entering a state
        public List<BaseAction> updateActions;  // actions performed when in a state
        public List<BaseAction> exitActions;    // actions performed when exiting a state
        public List<ActionTransition> actionTransitions; // actions performed depending on the condition, prioritized
        public List<Transition> transitions;             // these are conditions to go to the next state.
        
        public virtual void Enter(StateMachineDriver driver)
        {
            driver.DisplayCurrentState();
            if (entryActions != null && entryActions.Count != 0)
            {
                foreach (var action in entryActions)
                {
                    action.Execute(driver);
                }
            }
        }

        public virtual void Process(StateMachineDriver driver)
        {
            if (actionTransitions != null && actionTransitions.Count != 0)
            {
                foreach (var actionCondition in actionTransitions)
                {
                    if (actionCondition.condition.Evaluate(driver))
                    {
                        actionCondition.trueAction.Execute(driver);
                    }
                    else
                    {
                        actionCondition.falseAction.Execute(driver);
                    }
                }
            }

            if (updateActions != null && updateActions.Count != 0)
            {
                foreach (var action in updateActions)
                {
                    action.Execute(driver);
                }
            }
            if (transitions != null && transitions.Count != 0)
            {
                foreach (var transition in transitions)
                {
                    if (transition.condition.Evaluate(driver))
                    {
                        driver.ChangeState(transition.nextState);
                        break;
                    }
                }
            }
        }

        public virtual void Exit(StateMachineDriver driver)
        {
            if (exitActions != null && exitActions.Count != 0)
            {
                foreach (var action in exitActions)
                {
                    action.Execute(driver);
                }
            }
        }
    }



}