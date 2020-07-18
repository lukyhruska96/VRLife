using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.TickRateService;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Core.Wrappers;
using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VrLifeClient;
using VrLifeClient.API;
using VrLifeShared.Core.Applications;
using VrLifeShared.Networking.NetworkingModels;

public class RoomState : MonoBehaviour
{
    private AppInfo _info = new AppInfo(ulong.MaxValue, "RoomState", null, AppType.APP_GLOBAL);
    private Coroutine _roomStateCoroutine;
    private IAvatar _playerAvatar;
    private ControlHUD _hud;
    private bool _ready = false;
    private ClosedAPI _api;

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
        while(true)
        {
            SnapshotData data;
            ServiceCallback<SnapshotData> callback = VrLifeCore.API.TickRate.GetSnapshot();
            yield return callback.WaitCoroutine();
            if(callback.HasException)
            {
                yield break;
            }
            data = callback.Result;
            if(data.TickNum == lastTick)
            {
                continue;
            }
            List<SkeletonState> skeletons = data.Skeletons.Select(x => new SkeletonState(x)).ToList();
            Dictionary<ulong, IAvatar> _avatarsCopy = _api.GlobalAPI.Players.GetAvatars().ToDictionary(x => x.Key, x => x.Value);
            foreach(SkeletonState state in skeletons)
            {
                if(state.UserId == _playerAvatar.GetUserId())
                {
                    continue;
                }
                if(!_avatarsCopy.TryGetValue(state.UserId, out IAvatar avatar))
                {
                    IAvatar tmp = new DefaultAvatar(state.UserId, state.UserId.ToString(),
                        state.BodyLocation.ToUnity(), Quaternion.identity);
                    _api.GlobalAPI.Players.AddAvatar(state.UserId, tmp);
                    avatar = tmp;
                }
                _avatarsCopy.Remove(state.UserId);
                avatar.SetSkeleton(state);
            }
            foreach(IAvatar avatar in _avatarsCopy.Values)
            {
                _api.GlobalAPI.Players.DeleteAvatar(avatar.GetUserId());
            }
            lastTick = data.TickNum;
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
