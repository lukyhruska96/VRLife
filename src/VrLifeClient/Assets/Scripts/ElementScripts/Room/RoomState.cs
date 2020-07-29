using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Client.Core.Wrappers;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient;
using VrLifeClient.API;
using System;

public class RoomState : MonoBehaviour
{
    private AppInfo _info = new AppInfo(ulong.MaxValue, "RoomState", null, 
        new AppVersion(new int[] { 1, 0, 0 }),  AppType.APP_GLOBAL);
    private Coroutine _roomStateCoroutine;
    private IAvatar _playerAvatar;
    private ControlHUD _hud;
    private bool _ready = false;
    private IClosedAPI _api;

    private void Awake()
    {
        VrLifeCore.API.Room.OnRoomEnter();
    }

    void Start()
    {
        if(VrLifeCore.API == null)
        {
            return;
        }
        VrLifeCore.API.Room.RoomExited += OnRoomExit;
        _api = VrLifeCore.GetClosedAPI(_info);
        InitilizePlayer();
        _roomStateCoroutine = StartCoroutine(RoomStateCoroutine());
        _ready = true;
    }

    private void OnEnable()
    {
        if(_roomStateCoroutine == null && _ready)
        {
            _roomStateCoroutine = StartCoroutine(RoomStateCoroutine());
        }
    }

    private void OnDisable()
    {
        if (_roomStateCoroutine != null)
        {
            StopCoroutine(_roomStateCoroutine);
            _roomStateCoroutine = null;
        }
    }

    IEnumerator RoomStateCoroutine()
    {
        ulong lastTick = 0;
        while(VrLifeCore.API == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        while(true)
        {
            IServiceCallback<SnapshotData> callback = null;
            SnapshotData data;
            try
            {
                callback = VrLifeCore.API.TickRate.GetSnapshot();
            }
            catch(Exception e)
            {
                UILogger.current?.Error(e);
            }
            yield return callback.WaitCoroutine();
            if (callback.HasException)
            {
                UILogger.current?.Error(callback.Exception);
                yield return null;
                continue;
            }
            List<SkeletonState> skeletons;
            Dictionary<ulong, IAvatar> _avatarsCopy;
            try
            {
                data = callback.Result;
                if (data.TickNum == lastTick)
                {
                    continue;
                }
                skeletons = data.Skeletons
                    .Where(x => x.UserId != _api.Services.User.UserId)
                    .Select(x => new SkeletonState(x)).ToList();
                _avatarsCopy = _api.GlobalAPI.Players.GetAvatars().ToDictionary(x => x.Key, x => x.Value);
            }
            catch(Exception e)
            {
                UILogger.current.Error(e);
                continue;
            }
            yield return null;
            try
            {
                foreach (SkeletonState state in skeletons)
                {
                    if (state.UserId == _playerAvatar.GetUserId())
                    {
                        continue;
                    }
                    if (!_avatarsCopy.TryGetValue(state.UserId, out IAvatar avatar))
                    {
                        IAvatar tmp = new DefaultAvatar(state.UserId, state.UserId.ToString(),
                            state.BodyLocation.ToUnity(), Quaternion.identity);
                        _api.GlobalAPI.Players.AddAvatar(state.UserId, tmp);
                        avatar = tmp;
                    }
                    _avatarsCopy.Remove(state.UserId);
                    avatar.SetSkeleton(state);
                }
            }
            catch(Exception e)
            {
                UILogger.current.Error(e);
                continue;
            }
            yield return null;
            try
            {
                foreach (IAvatar avatar in _avatarsCopy.Values)
                {
                    _api.GlobalAPI.Players.DeleteAvatar(avatar.GetUserId());
                }
            }
            catch(Exception e)
            {
                UILogger.current.Error(e);
                continue;
            }
            yield return null;
            try
            {
                foreach (var state in data.Objects)
                {
                    if (_api.Services.App.ObjectAppInstances.TryGetValue(state.AppId, out var list) &&
                        list.Count > (int)state.AppInstanceId && list[(int)state.AppInstanceId] != null)
                    {
                        continue;
                    }
                    _api.Services.App.CreateObjectAppInstance(state.AppId, state.AppInstanceId, state.Center.ToVector());
                }
                lastTick = data.TickNum;
            }
            catch(Exception e)
            {
                UILogger.current.Error(e);
            }
            yield return null;
        }
    }

    private void OnRoomExit()
    {
        SceneController.current.ToMainMenu();
    }

    private void InitilizePlayer()
    {
        ulong userId = VrLifeCore.API.User.UserId.Value;
        _playerAvatar = new DefaultAvatar(userId, "Player", Vector3.zero, Quaternion.identity);
        _api.GlobalAPI.Players.AddMainAvatar(_playerAvatar);
    }
}
