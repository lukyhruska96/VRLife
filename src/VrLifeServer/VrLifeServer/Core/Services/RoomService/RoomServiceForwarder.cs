using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Forwarder.Core.Services.RoomService;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.API.Forwarder;

namespace VrLifeServer.Core.Services.RoomService
{
    class RoomServiceForwarder : IRoomServiceForwarder
    {
        private const int ROOM_INFO_INTERVAL = 10000;
        private const int ACTIVITY_WATCH_INTERVAL = 5000;

        public event UserDisconnectEventHandler UserDisconnected;
        public event UserConnectEventHandler UserConnected;
        public event RoomDeleteEventHandler RoomDeleted;
        public event RoomCreateEventHandler RoomCreated;

        private List<Room> _roomList = new List<Room>();
        private Dictionary<ulong, uint?> _user2Room = new Dictionary<ulong, uint?>();
        private IClosedAPI _api = null;
        private ILogger _log;

        public MainMessage HandleMessage(MainMessage msg)
        {
            _log.Debug("In HandleMessage method.");
            RoomMsg roomMsg = msg.RoomMsg;
            switch(roomMsg.MessageTypeCase)
            {
                case RoomMsg.MessageTypeOneofCase.RoomCreate:
                    return RoomCreate(msg.MsgId, roomMsg.RoomCreate);
                case RoomMsg.MessageTypeOneofCase.RoomEnter:
                    return RoomEnter(msg.MsgId, msg.ClientId, roomMsg.RoomEnter);
                case RoomMsg.MessageTypeOneofCase.RoomExit:
                    return RoomExit(msg.MsgId, msg.ClientId, roomMsg.RoomExit);
                case RoomMsg.MessageTypeOneofCase.RoomQuery:
                    switch(roomMsg.RoomQuery.RoomQueryCase)
                    {
                        case RoomQuery.RoomQueryOneofCase.RoomDetailId:
                            return RoomDetail(roomMsg.RoomQuery.RoomDetailId);
                        case RoomQuery.RoomQueryOneofCase.RoomListQuery:
                            return RoomList(roomMsg.RoomQuery.RoomListQuery);
                        default:
                            return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid Operation.");
                    }
                default:
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid Operation.");

            }
        }

        public void Init(IClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
            InitRoomListInfo();
            InitActivityWatch();
            InitRoomGC();
        }

        private MainMessage RoomDetail(uint roomId)
        {
            _log.Debug("In RoomDetail method.");
            MainMessage msg = PrepareMsg();
            msg.RoomMsg = new RoomMsg();
            msg.RoomMsg.RoomDetail = _roomList[(int)roomId].ToNetworkModel();
            return msg;
        }

        private MainMessage RoomList(RoomListQuery listQuery)
        {
            _log.Debug("In RoomList method.");
            RoomList roomList = new RoomList();
            IEnumerable<RoomDetail> roomEnum = _roomList.Select(x => x.ToNetworkModel());
            if(listQuery.NotEmpty)
            {
                roomEnum = roomEnum.Where(x => x.Players.Count != 0);
            }
            if(listQuery.NotFull)
            {
                roomEnum = roomEnum.Where(x => x.Players.Count != x.Capacity);
            }
            roomList.RoomList_.Add(roomEnum);
            MainMessage msg = PrepareMsg();
            msg.RoomMsg.RoomList = roomList;
            return msg;
        }

        private MainMessage RoomCreate(ulong msgId, RoomCreate create)
        {
            _log.Debug("In RoomCreate method.");
            Room room = new Room();
            room.Name = create.Name;
            room.Capacity = create.Capacity;
            room.Address = new IPEndPoint(_api.OpenAPI.Config.ServerAddress, (int)_api.OpenAPI.Config.UdpPort);
            room.LastActivity = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            lock (_roomList)
            {
                room.Id = (uint) _roomList.Count;
                _roomList.Add(room);
                OnRoomCreated(room);
                _log.Info($"Created new Room with ID {room.Id}.");
                return RoomDetail(room.Id);
            }
        }

        private MainMessage RoomEnter(ulong msgId, ulong clientId, RoomEnter roomEnter)
        {
            _log.Debug("In RoomEnter method.");
            int roomId = (int)roomEnter.RoomId;
            if (roomId >= _roomList.Count || _roomList[roomId] == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msgId, 0, 0, "There is no such room with this ID.");
            }
            lock (_roomList[roomId])
            {
                Room room = _roomList[roomId];
                if(room.Capacity <= room.Players.Count)
                {
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msgId, 0, 0, "Room is full.");
                }
                ulong? userId = _api.Services.User.GetUserIdByClientId(clientId);
                if(!userId.HasValue)
                {
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msgId, 0, 0, "Unable to match client ID to any user.");
                }
                room.Players.Add(userId.Value);
                room.LastActivity = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _user2Room[userId.Value] = (uint?)roomId;
                OnUserConnected(userId.Value, room.Id);
                _log.Info($"User with id '{userId.Value}' entered room '{room.Name}'.");
                MainMessage response = new MainMessage();
                response.RoomMsg = new RoomMsg();
                response.RoomMsg.RoomDetail = room.ToNetworkModel();
                return response;
            }
        }

        private MainMessage RoomExit(ulong msgId, ulong clientId, RoomExit roomExit)
        {
            _log.Debug("In RoomExit method.");
            int roomId = (int) roomExit.RoomId;
            ulong? userId = _api.Services.User.GetUserIdByClientId(clientId);
            if(!userId.HasValue || roomId >= _roomList.Count || _roomList[roomId] == null)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msgId, 0, 0, "Unable to match client ID to any user.");
            }
            lock(_roomList[roomId])
            {
                int idx = _roomList[roomId].Players.FindIndex(x => x == userId.Value);
                if (idx < 0)
                {
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msgId, 0, 0, "Your client is not connected to this room.");
                }
                Room room = _roomList[roomId];
                room.Players.RemoveAt(idx);
                room.LastActivity = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                OnUserDisconnected(userId.Value, room.Id, "Disconnected");
                _user2Room[userId.Value] = null;
                _log.Info($"User with id '{userId.Value}' exited room '{room.Name}'.");
            }
            return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateOkMessage(msgId);
        }

        private static MainMessage PrepareMsg()
        {
            MainMessage msg = new MainMessage();
            msg.RoomMsg = new RoomMsg();
            return msg;
        }

        private void InitRoomListInfo()
        {
            Task roomListInfo = new Task(() =>
            {
                while(true)
                {
                    RoomList roomList = new RoomList();
                    lock(_roomList)
                    {
                        roomList.RoomList_.AddRange(_roomList.Select(x => x.ToNetworkModel()));
                    }
                    MainMessage msg = new MainMessage();
                    msg.RoomMsg = new RoomMsg();
                    msg.RoomMsg.RoomList = roomList;
                    _api.OpenAPI.Networking.SendAsync(msg, _api.OpenAPI.Config.MainServer, (response) =>
                    {
                        if(response.MessageTypeCase != MainMessage.MessageTypeOneofCase.SystemMsg)
                        {
                            _log.Error("Unexpected response from Provider Server.");
                        }
                        else if(response.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg)
                        {
                            _log.Error(response.SystemMsg.ErrorMsg.ErrorMsg_);
                        }
                    }, (ex) => _log.Error(ex));
                    Thread.Sleep(ROOM_INFO_INTERVAL);
                }
            }, TaskCreationOptions.LongRunning);
            roomListInfo.Start();
        }

        private void InitActivityWatch()
        {
            Task activityWatch = new Task(() => {
                while(true)
                {
                    foreach(Room room in _roomList)
                    {
                        List<ulong> tempList = new List<ulong>();
                        tempList.AddRange(room.Players);
                        lock(room)
                        {
                            room.Players.Clear();
                            foreach(ulong userId in tempList)
                            {
                                if (_api.Services.TickRate.IsActive(userId, room.Id))
                                {
                                    room.Players.Add(userId);
                                }
                                else
                                {
                                    OnUserDisconnected(userId, room.Id, "Inactive");
                                    _user2Room[userId] = null;
                                }
                            }
                        }
                    }
                    Thread.Sleep(ACTIVITY_WATCH_INTERVAL);
                }
            }, TaskCreationOptions.LongRunning);
            activityWatch.Start();
        }

        private void InitRoomGC()
        {
            Task roomGC = new Task(() =>
            {
                for(int i = 0; i < _roomList.Count; ++i)
                {
                    Room r = _roomList[i];
                    if(r.Players.Count == 0 && 
                        DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - 
                        r.LastActivity > ACTIVITY_WATCH_INTERVAL * 3)
                    {
                        _roomList[(int)r.Id] = null;
                        OnRoomDeleted(r.Id);
                    }
                }
                Thread.Sleep(ACTIVITY_WATCH_INTERVAL);
            }, TaskCreationOptions.LongRunning);
            roomGC.Start();
        }

        public uint? RoomByUserId(ulong userId)
        {
            if (!_user2Room.TryGetValue(userId, out uint? roomId))
            {
                return null;
            }
            return roomId;
        }

        public ulong[] GetConnectedUsers(uint roomId)
        {
            if(roomId >= _roomList.Count)
            {
                return null;
            }
            return _roomList[(int)roomId].Players.ToArray();
        }

        protected virtual void OnUserConnected(ulong userId, uint roomId)
        {
            UserConnected?.Invoke(userId, roomId);
        }

        protected virtual void OnUserDisconnected(ulong userId, uint roomId, string reason)
        {
            UserDisconnected?.Invoke(userId, roomId, reason);
        }

        protected virtual void OnRoomDeleted(uint roomId)
        {
            RoomDeleted?.Invoke(roomId);
        }

        protected virtual void OnRoomCreated(Room room)
        {
            RoomCreated?.Invoke(room);
        }
    }
}
