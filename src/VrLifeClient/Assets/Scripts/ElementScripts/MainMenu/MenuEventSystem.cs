using Assets.Scripts.Core.Services;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VrLifeClient.Core.Services.RoomService;

public class MenuEventSystem : MonoBehaviour
{

    public static MenuEventSystem current;

    private ConcurrentQueue<UnityAction> _mainThreadCallbacks = new ConcurrentQueue<UnityAction>();

    public UnityEvent<bool> SignedIn { get; } = new ServiceEvent<bool>();
    public UnityEvent<bool> SignedUp { get; } = new ServiceEvent<bool>();
    public UnityEvent<Room> RoomFound { get; } = new ServiceEvent<Room>();
    public UnityEvent Login { get; } = new UnityEvent();
    public UnityEvent Register { get; } = new UnityEvent();

    void Awake()
    {
        current = this;
        StartCoroutine(HandleMainThreadCallbacks());
    }

    public void AddMainThreadCallback(UnityAction action)
    {
        _mainThreadCallbacks.Enqueue(action);
    }

    IEnumerator HandleMainThreadCallbacks()
    {
        while(true)
        {
            while(!_mainThreadCallbacks.IsEmpty)
            {
                UnityAction action;
                while (!_mainThreadCallbacks.TryDequeue(out action)) ;
                action.Invoke();
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
