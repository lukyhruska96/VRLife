using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Services.TickRateService;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Core.Wrappers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VrLifeClient;
using VrLifeShared.Networking.NetworkingModels;

public class RoomState : MonoBehaviour
{
    private Dictionary<ulong, IAvatar> _avatars = new Dictionary<ulong, IAvatar>();
    private Coroutine _roomStateCoroutine;
    private IAvatar _playerAvatar;
    private ControlHUD _hud;
    private bool _ready = false;

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
            try
            {
                data = VrLifeCore.API.TickRate.GetSnapshot().Wait();
            }
            catch(TickRateServiceException)
            {
                yield break;
            }
            if(data.TickNum == lastTick)
            {
                continue;
            }
            List<SkeletonState> skeletons = data.Skeletons.Select(x => new SkeletonState(x)).ToList();
            Dictionary<ulong, IAvatar> _avatarsCopy = _avatars.ToDictionary(x => x.Key, x => x.Value);
            _avatarsCopy.Remove(_playerAvatar.GetUserId());
            foreach(SkeletonState state in skeletons)
            {
                if(state.UserId == _playerAvatar.GetUserId())
                {
                    continue;
                }
                if(!_avatars.TryGetValue(state.UserId, out IAvatar avatar))
                {
                    _avatars[state.UserId] = new DefaultAvatar(state.UserId, state.UserId.ToString(), 
                        state.BodyLocation.ToUnity(), Quaternion.identity);
                    avatar = _avatars[state.UserId];
                }
                _avatarsCopy.Remove(state.UserId);
                avatar.SetSkeleton(state);
            }
            foreach(IAvatar avatar in _avatarsCopy.Values)
            {
                _avatars.Remove(avatar.GetUserId());
                avatar.Destroy();
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
        _playerAvatar.SetControls(true);
        _hud = new ControlHUD(_playerAvatar.GetHead());
    }
}
