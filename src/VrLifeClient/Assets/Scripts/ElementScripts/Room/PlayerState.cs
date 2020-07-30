using Assets.Scripts.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Client.Core.Wrappers;
using VrLifeAPI.Client.Core.Services;
using VrLifeClient;
using VrLifeClient.Core.Services.RoomService;

public class PlayerState : MonoBehaviour
{
    private const int TIMEOUT_MS = 2000;
    public IAvatar Avatar { get; set; }
    private Coroutine _skeletonStateEvent = null;
    private long _lastUpdate = 0;

    void Start()
    {
        _skeletonStateEvent = StartCoroutine(SkeletonStateEvent());
    }

    void Update()
    {
        if(_lastUpdate == 0)
        {
            return;
        }

        if(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _lastUpdate > TIMEOUT_MS)
        {
            _skeletonStateEvent = StartCoroutine(SkeletonStateEvent());
        }
    }

    private void OnEnable()
    {
        if(_skeletonStateEvent == null)
        {
            _skeletonStateEvent = StartCoroutine(SkeletonStateEvent());
        }
    }

    private void OnDisable()
    {
        if (_skeletonStateEvent != null)
        {
            StopCoroutine(_skeletonStateEvent);
            _skeletonStateEvent = null;
        }
    }

    private void OnDestroy()
    {
        if (_skeletonStateEvent != null)
        {
            StopCoroutine(_skeletonStateEvent);
            _skeletonStateEvent = null;
        }
    }

    IEnumerator SkeletonStateEvent()
    {
        if (Avatar != null)
        {
            while (VrLifeCore.API == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
            while (true)
            {
                SkeletonState state;
                IServiceCallback<byte[]> callback = null;
                try
                {
                    state = Avatar.GetCurrentSkeleton();
                    callback = VrLifeCore.API.Event.SendSkeleton(state);
                }
                catch(Exception e)
                {
                    UILogger.current?.Error(e);
                }
                yield return callback.WaitCoroutine();
                if(callback.HasException)
                {
                    UILogger.current?.Error(callback.Exception);
                    yield return null;
                }
                yield return null;
            }
        }
    }
}
