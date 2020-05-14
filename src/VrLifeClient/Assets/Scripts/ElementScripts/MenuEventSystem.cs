using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuEventSystem : MonoBehaviour
{

    public static MenuEventSystem current;

    private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

    public void Awake()
    {
        current = this;
    }

    public void AddListener(string key, UnityAction action)
    {
        if(!eventDictionary.ContainsKey(key))
        {
            eventDictionary[key] = new UnityEvent();
        }
        eventDictionary[key].AddListener(action);
    }

    public void InvokeEvent(string key)
    {
        eventDictionary[key]?.Invoke();
    }


}
