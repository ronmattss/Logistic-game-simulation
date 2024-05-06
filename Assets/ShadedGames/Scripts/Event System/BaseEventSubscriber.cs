using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEventSubscriber : MonoBehaviour
{
    // Start is called before the first frame update
    public int eventID;
    void Start()
    {
        // Subscribe to an event
        BaseEvent.current.onTriggerEventEnter += SubscribedMethod;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SubscribedMethod(int id)
    {
       if(id == eventID)
        {
            // Do  Something

        }
    }
}
