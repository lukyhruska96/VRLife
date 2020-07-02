using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Core.Services.UserService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.RoomService
{
    class RoomServiceForwarder : IRoomServiceForwarder
    {
        private const int ROOM_INFO_INTERVAL = 10000;
        private const int ACTIVITY_WATCH_INTERVAL = 5000;

        private List<Room> _roomList = new List<Room>();
        private Dictionary<uint, uint?> _client2Room = new Dictionary<uint, uint?>();
        private ClosedAPI _api = null;
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
                            return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid Operation.");
                    }
                case RoomMsg.MessageTypeOneofCase.RoomDetail:
                    if (msg.SenderIdCase != MainMessage.SenderIdOneofCase.ServerId)
                    {
                        return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Client cannot ask for room migration.");
                    }
                    return RoomMigration(msg.MsgId, msg.ServerId, roomMsg.RoomDetail);
                case RoomMsg.MessageTypeOneofCase.RoomList:
                    if (msg.SenderIdCase != MainMessage.SenderIdOneofCase.ServerId)
                    {
                        return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Client cannot ask for bulk room migration.");
                    }
                    return RoomBulkMigration(msg.MsgId, msg.ServerId, roomMsg.RoomList);
                default:
                    return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid Operation.");

            }
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
            InitRoomListInfo();
            InitActivityWatch();
        }

        private MainMessage RoomDetail(uint roomId)
        {
            _log.Debug("In RoomDetail method.");
            MainMessage msg = PrepareMsg();
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
            lock(_roomList)
            {
                room.Id = (uint) _roomList.Count;
                _roomList.Add(room);
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
                return ISystemService.CreateErrorMessage(msgId, 0, 0, "There is no such room with this ID.");
            }
            lock (_roomList[roomId])
            {
                Room room = _roomList[roomId];
                if(room.Capacity <= room.Players.Count)
                {
                    return ISystemService.CreateErrorMessage(msgId, 0, 0, "Room is full.");
                }
                ulong? userId = _api.Services.User.GetUserIdByClientId(clientId);
                if(!userId.HasValue)
                {
                    return ISystemService.CreateErrorMessage(msgId, 0, 0, "Unable to match client ID to any user.");
                }
                room.Players.Add(userId.Value);
                _log.Info($"User with id '{userId.Value}' entered room '{room.Name}'.");
            }
            return ISystemService.CreateOkMessage(msgId);
        }

        private MainMessage RoomExit(ulong msgId, ulong clientId, RoomExit roomExit)
        {
            _log.Debug("In RoomExit method.");
            int roomId = (int) roomExit.RoomId;
            ulong? userId = _api.Services.User.GetUserIdByClientId(clientId);
            if(!userId.HasValue || roomId >= _roomList.Count || _roomList[roomId] == null)
            {
                return ISystemService.CreateErrorMessage(msgId, 0, 0, "Unable to match client ID to any user.");
            }
            lock(_roomList[roomId])
            {
                int idx = _roomList[roomId].Players.FindIndex(x => x == userId.Value);
                if (idx < 0)
                {
                    return ISystemService.CreateErrorMessage(msgId, 0, 0, "Your client is not connected to this room.");
                }
                Room room = _roomList[roomId];
                room.Players.RemoveAt(idx);
                _log.Info($"User with id '{userId.Value}' exited room '{room.Name}'.");
            }
            return ISystemService.CreateOkMessage(msgId);
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
                                if(_api.Services.TickRate.IsActive(userId, room.Id))
                                {
                                    room.Players.Add(userId);
                                }
                            }
                        }
                    }
                    Thread.Sleep(ACTIVITY_WATCH_INTERVAL);
                }
            }, TaskCreationOptions.LongRunning);
            activityWatch.Start();
        }

        // TODO Features

        private MainMessage RoomBulkMigration(ulong msgId, uint serverId, RoomList roomList)
        {
            _log.Debug("In RoomBulkMigration method.");
            return ISystemService.CreateErrorMessage(msgId, 0, 0, "Not supported yet.");
        }

        private MainMessage RoomMigration(ulong msgId, uint serverId, RoomDetail roomDetail)
        {
            _log.Debug("In RoomMigration method.");
            return ISystemService.CreateErrorMessage(msgId, 0, 0, "Not supported yet.");
        }

        public uint? RoomByClientId(uint clientId)
        {
            if(!_client2Room.TryGetValue(clientId, out uint? roomId))
            {
                return null;
            }
            return roomId;
        }
    }
}
