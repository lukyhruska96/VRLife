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
                IServiceCallback<byte[]> callback = VrLifeCore.API.Event.SendSkeleton(state);
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
