using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Core.Utils;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.RoomService
{
    class RoomServiceProvider : IRoomServiceProvider
    {
        private Dictionary<int, List<Room>> _roomsPerServer = new Dictionary<int, List<Room>>();
        private Dictionary<uint, uint?> _user2Room = new Dictionary<uint, uint?>();
        private ClosedAPI _api;

        public MainMessage HandleMessage(MainMessage msg)
        {
            RoomMsg roomMsg = msg.RoomMsg;
            int serverId = 0;
            switch (roomMsg.MessageTypeCase)
            {
                case RoomMsg.MessageTypeOneofCase.RoomCreate:
                    return RoomCreate(msg.MsgId, msg.ClientId, msg);
                case RoomMsg.MessageTypeOneofCase.RoomDetail:
                    return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid message.");
                case RoomMsg.MessageTypeOneofCase.RoomEnter:
                    serverId = _roomsPerServer.Where(x => x.Value.Find(y => y.Id == msg.RoomMsg.RoomEnter.RoomId) != null).FirstOrDefault().Key;
                    if(serverId == 0)
                    {
                        return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Room with this ID could not be found.");
                    }
                    return ISystemService.CreateRedirectMessage(msg, _api.Services.System.GetAddressById((uint)serverId));
                case RoomMsg.MessageTypeOneofCase.RoomExit:
                    serverId = _roomsPerServer.Where(x => x.Value.Find(y => y.Id == msg.RoomMsg.RoomExit.RoomId) != null).FirstOrDefault().Key;
                    if (serverId == 0)
                    {
                        return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Room with this ID could not be found.");
                    }
                    return ISystemService.CreateRedirectMessage(msg, _api.Services.System.GetAddressById((uint)serverId));
                case RoomMsg.MessageTypeOneofCase.RoomList:
                    if(msg.SenderIdCase == MainMessage.SenderIdOneofCase.ClientId)
                    {
                        return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid message.");
                    }
                    return RoomListServer(msg.MsgId, (int)msg.ServerId, msg.RoomMsg.RoomList);
                case RoomMsg.MessageTypeOneofCase.RoomQuery:
                    switch(msg.RoomMsg.RoomQuery.RoomQueryCase)
                    {
                        case RoomQuery.RoomQueryOneofCase.RoomDetailId:
                            return RoomDetail(msg.MsgId, msg.RoomMsg.RoomQuery.RoomDetailId);
                        case RoomQuery.RoomQueryOneofCase.RoomListQuery:
                            return RoomListClient(msg.RoomMsg.RoomQuery.RoomListQuery);
                        default:
                            return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid message.");
                    }
                default:
                    return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Invalid message.");
            }
        }

        public void Init(ClosedAPI api)
        {
            _api = api;
        }

        private MainMessage RoomDetail(ulong msgId, uint roomId)
        {
            Room room = null;
            foreach (List<Room> rooms in _roomsPerServer.Values)
            {
                room = rooms.Find(x => x.Id == roomId);
                if(room != null)
                {
                    break;
                }
            }
            if(room == null)
            {
                return ISystemService.CreateErrorMessage(msgId, 0, 0, "Room with this ID could not be found.");
            }
            MainMessage msg = new MainMessage();
            msg.RoomMsg = new RoomMsg();
            msg.RoomMsg.RoomDetail = room.ToNetworkModel();
            return msg;
        }

        private MainMessage RoomListServer(ulong msgId, int serverId, RoomList roomList)
        {
            if(!_roomsPerServer.ContainsKey(serverId))
            {
                _roomsPerServer[serverId] = new List<Room>();
            }
            lock(_roomsPerServer[serverId])
            {
                _roomsPerServer[serverId].Clear();
                _roomsPerServer[serverId].AddRange(roomList.RoomList_.Select(x => new Room(x)));
            }
            roomList.RoomList_.AsParallel().ForAll(x => x.Players.AsParallel().ForAll(y => _user2Room[(uint)y] = x.RoomId));
            return ISystemService.CreateOkMessage(msgId);
        }

        private MainMessage RoomListClient(RoomListQuery roomListQuery)
        {
            MainMessage msg = new MainMessage();
            msg.RoomMsg = new RoomMsg();
            msg.RoomMsg.RoomList = new RoomList();
            // flatten
            IEnumerable<Room> rooms = _roomsPerServer.Values.SelectMany(x => x);
            // search filter
            rooms = rooms.Where(x => x.Name.Contains(roomListQuery.Search));
            // not empty filter
            if (roomListQuery.NotEmpty)
            {
                rooms = rooms.Where(x => x.Players.Count != 0);
            }
            // not full filter
            if(roomListQuery.NotFull)
            {
                rooms = rooms.Where(x => x.Players.Count != x.Capacity);
            }
            msg.RoomMsg.RoomList.RoomList_.AddRange(rooms.Select(x => x.ToNetworkModel()));
            return msg;
        }

        private MainMessage RoomCreate(ulong msgId, ulong clientId, MainMessage msg)
        {
            if(_roomsPerServer.Count == 0)
            {
                return ISystemService.CreateErrorMessage(msgId, 0, 0, "There is currently no computing server.");
            }
            int serverId = 1;
            foreach(KeyValuePair<int, List<Room>> pair in _roomsPerServer)
            {
                lock(_roomsPerServer[pair.Key])
                {
                    if (pair.Value.Count < _roomsPerServer[serverId].Count)
                    {
                        serverId = pair.Key;
                    }
                }
            }

            IPEndPoint redirect = _api.Services.System.GetAddressById((uint)serverId);
            return ISystemService.CreateRedirectMessage(msg, redirect);
        }
    }
}
