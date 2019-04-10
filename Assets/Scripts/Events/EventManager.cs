using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager
{

    private Dictionary<EventsName, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (eventManager == null)
            {
                eventManager = new EventManager();
                eventManager.Init();
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventsName, UnityEvent>();
        }
    }

    public static void StartListening(EventsName eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(EventsName eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(EventsName eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}