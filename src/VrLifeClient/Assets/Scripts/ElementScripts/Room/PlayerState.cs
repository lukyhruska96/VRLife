using Assets.Scripts.Core.Character;
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
    public IAvatar avatar;
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
        StopCoroutine(_skeletonStateEvent);
        _skeletonStateEvent = null;
    }

    IEnumerator SkeletonStateEvent()
    {
        while(true)
        {
            SkeletonState state = avatar.GetCurrentSkeleton();
            VrLifeCore.API.Event.SendSkeleton(state).Wait();
            yield return null;
        }
    }
}
