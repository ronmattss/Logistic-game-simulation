using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace ShadedGames.Scripts.EventSystem
{
    /// <summary>
    /// What is an Event?
    /// An event is anything happening in the Game.
    /// We can classify Events in to three categories
    /// Game Events, Global Events, and UI Events
    /// Maybe only two
    /// 
    /// </summary>
    public class Event
    {
        // What is the event?

        public static UnityEvent gameEvent = new UnityEvent();
        public virtual void InvokeEvent()
        {
            gameEvent?.Invoke();
        }
        public virtual void AddListener(UnityAction gameFunction)
        {
            gameEvent.AddListener(gameFunction);            
        }
        public virtual void RemoveListener(UnityAction gameFunction) 
        {
            gameEvent.RemoveListener(gameFunction);
        }
    }

}