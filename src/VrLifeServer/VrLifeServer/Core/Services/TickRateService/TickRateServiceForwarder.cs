using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VrLifeAPI.Common.Core.Services.RoomService;
using VrLifeAPI.Common.Core.Services.TickRateService;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Forwarder.Core.Services.TickRateService;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Core.Services.RoomService;

namespace VrLifeServer.Core.Services.TickRateService
{
    class TickRateServiceForwarder : ITickRateServiceForwarder
    {
        private const long MAX_ACTIVITY_DELTA = 5000;
        private const long TIME_TO_INIT = 10000;
        private IClosedAPI _api;
        private ILogger _log;
        private ConcurrentDictionary<uint, TickRoom> _tickRoom = new ConcurrentDictionary<uint, TickRoom>();
        private Dictionary<(uint, ulong), long> _playerLastActivity = new Dictionary<(uint, ulong), long>();


        public MainMessage HandleMessage(MainMessage msg)
        {
            TickMsg tickMsg = msg.TickMsg;
            if(tickMsg == null || tickMsg.TickMsgTypeCase != TickMsg.TickMsgTypeOneofCase.SnapshotRequest)
            {
                VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Unhandled TickMsg request.");
            }
            SnapshotRequest req = tickMsg.SnapshotRequest;
            // if cached userId is not equal to userId in request, then update value and compare it again
            if(req.UserId != _api.Services.User.GetUserIdByClientId(msg.ClientId, true) &&
                req.UserId != _api.Services.User.GetUserIdByClientId(msg.ClientId))
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Authentication error.");
            }
            uint? roomId = _api.Services.Room.RoomByUserId(req.UserId);
            if(!roomId.HasValue)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Client is not connected to any Room.");
            }
            SnapshotData data = _tickRoom[roomId.Value].GetSnapshotData(req.LastTick, req.LastRTT);
            MainMessage response = new MainMessage();
            response.TickMsg = new TickMsg();
            response.TickMsg.SnapshotData = data;
            return response;
        }

        public void Init(IClosedAPI api)
        {
            _api = api;
            _log = api.OpenAPI.CreateLogger(this.GetType().Name);
            _api.Services.Room.UserConnected += OnUserConnected;
            _api.Services.Room.UserDisconnected += OnUserDisconnected;
            _api.Services.Room.RoomDeleted += OnRoomDeleted;
            _api.Services.Room.RoomCreated += OnRoomCreated;
        }

        public bool IsActive(ulong userId, uint roomId)
        {
            if(!_playerLastActivity.TryGetValue((roomId, userId), out long value))
            {
                return false;
            }
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - value < MAX_ACTIVITY_DELTA;
        }

        public void SetObjectState(uint roomId, ulong objectInstanceId, ObjectState obj)
        {
            // TODO Add ObjectService App
            if(_tickRoom.TryGetValue(roomId, out TickRoom tickRoom))
            {
                tickRoom.CurrentTick.ObjectStates[objectInstanceId] = obj;
            }
        }

        public void SetSkeletonState(uint roomId, ulong userId, SkeletonState skeleton)
        {
            if(_tickRoom.TryGetValue(roomId, out TickRoom tickRoom) && _api.Services.Room.RoomByUserId(userId) == roomId)
            {
                _playerLastActivity[(roomId, userId)] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                tickRoom.CurrentTick.SkeletonStates[userId] = skeleton;
            }
        }

        private void OnUserDisconnected(ulong userId, uint roomId, string reason)
        {
            if (_tickRoom.TryGetValue(roomId, out TickRoom room) && _api.Services.Room.RoomByUserId(userId) == roomId)
            {
                _playerLastActivity.Remove((roomId, userId));
                room.CurrentTick.SkeletonStates.TryRemove(userId, out _);
            }
        }

        private void OnUserConnected(ulong userId, uint roomId)
        {
            _playerLastActivity[(roomId, userId)] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + TIME_TO_INIT;
        }

        protected virtual void OnRoomDeleted(uint roomId)
        {
            TickRoom r = null;
            while (_tickRoom.ContainsKey(roomId) && !_tickRoom.TryRemove(roomId, out r)) ;
            r?.Stop();
        }

        protected virtual void OnRoomCreated(IRoom room)
        {
            TickRoom tickRoom = new TickRoom(room, (int)room.TickRate);
            if (_tickRoom.TryAdd(room.Id, tickRoom))
            {
                tickRoom.Start();
            }
        }
    }
}
