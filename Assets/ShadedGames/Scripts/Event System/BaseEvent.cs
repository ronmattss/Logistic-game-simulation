using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEvent : MonoBehaviour
{

    public static BaseEvent current;
    private void Awake()
    {
        current = this;
    }


    public event Action <int> onTriggerEventEnter;

    // TRIGGER THIS SOMEWHERE
    public void onTriggerEvent(int id)
    {
        if(onTriggerEventEnter != null)
        {
            onTriggerEventEnter?.Invoke(id);
        }
    }

}
