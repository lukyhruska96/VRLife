using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.EventService;
using Assets.Scripts.Core.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VrLifeClient;
using VrLifeClient.Core.Services.RoomService;
using VrLifeShared.Networking.NetworkingModels;

public class PlayerState : MonoBehaviour
{
    public IAvatar Avatar { get; set; }
    private Coroutine _skeletonStateEvent = null;
    // Start is called before the first frame update
    void Start()
    {
        _skeletonStateEvent = StartCoroutine(SkeletonStateEvent());
    }

    // Update is called once per frame
    void Update()
    {

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

    IEnumerator SkeletonStateEvent()
    {
        if (Avatar != null)
        {
            while (true)
            {
                SkeletonState state = Avatar.GetCurrentSkeleton();
                ServiceCallback<byte[]> callback = VrLifeCore.API.Event.SendSkeleton(state);
                yield return callback.WaitCoroutine();
                if(callback.HasException)
                {
                    yield break;
                }
                yield return null;
            }
        }
    }
}
