using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.RoomService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Core.Wrappers;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.API;
using VrLifeClient.Core.Services.SystemService;

namespace VrLifeClient.Core.Services.RoomService
{
    class RoomServiceClient : IRoomServiceClient
    {
        private IClosedAPI _api;

        private IPEndPoint _forwarderAddress = null;
        public IPEndPoint ForwarderAddress { get => _forwarderAddress; }

        public IRoom CurrentRoom { get => _currentRoom; }
        private Room _currentRoom = null;

        public event Action RoomExited;

        public event Action RoomEntered;

        public void HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(IClosedAPI api)
        {
            this._api = api;
            this._api.Services.System.ForwarderLost += Reset;
            this._api.Services.User.UserLoggedOut += Reset;
        }

        public IServiceCallback<IRoom> RoomDetail(uint roomId)
        {
            return new ServiceCallback<IRoom>(() =>
            {
                RoomQuery roomQuery = new RoomQuery();
                roomQuery.RoomDetailId = roomId;
                MainMessage msg = new MainMessage();
                msg.RoomMsg = new RoomMsg();
                msg.RoomMsg.RoomQuery = roomQuery;
                MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
                ErrorMsgCheck(response);
                return new Room(response.RoomMsg.RoomDetail);
            });
        }

        public IServiceCallback<List<IRoom>> RoomList(string contains = "", bool notEmpty = false, bool notFull = false)
        {
            return new ServiceCallback<List<IRoom>>(() =>
            {
                RoomListQuery query = new RoomListQuery();
                query.Search = contains;
                query.NotEmpty = notEmpty;
                query.NotFull = notFull;
                MainMessage msg = new MainMessage();
                msg.RoomMsg = new RoomMsg();
                msg.RoomMsg.RoomQuery = new RoomQuery();
                msg.RoomMsg.RoomQuery.RoomListQuery = query;
                MainMessage response;
                try
                {
                    response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
                }
                catch(SocketException)
                {
                    _api.Services.System.OnProviderLost();
                    throw;
                }
                ErrorMsgCheck(response);
                RoomList list = response.RoomMsg.RoomList;
                return list == null ? new List<IRoom>() : list.RoomList_.Select(x => new Room(x)).ToList<IRoom>();
            });
        }

        public IServiceCallback<IRoom> RoomCreate(string name, uint capacity)
        {
            return new ServiceCallback<IRoom>(() =>
            {
                RoomCreate roomCreate = new RoomCreate();
                roomCreate.Name = name;
                roomCreate.Capacity = capacity;
                MainMessage msg = new MainMessage();
                msg.RoomMsg = new RoomMsg();
                msg.RoomMsg.RoomCreate = roomCreate;
                MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
                ErrorMsgCheck(response);
                RoomDetail roomDetail = response.RoomMsg.RoomDetail;
                return new Room(roomDetail);
            });
        }

        public IServiceCallback<IRoom> RoomEnter(uint roomId)
        {
            return RoomEnter(roomId, _api.OpenAPI.Config.MainServer);
        }

        public IServiceCallback<IRoom> RoomEnter(uint roomId, IPEndPoint address)
        {
            return new ServiceCallback<IRoom>(() =>
            {
                RoomEnter roomEnter = new RoomEnter();
                roomEnter.RoomId = roomId;
                MainMessage msg = new MainMessage();
                msg.RoomMsg = new RoomMsg();
                msg.RoomMsg.RoomEnter = roomEnter;
                MainMessage response = _api.OpenAPI.Networking.Send(msg, address);
                ErrorMsgCheck(response);
                RoomDetail roomDetail = response.RoomMsg.RoomDetail;
                if (roomDetail == null)
                {
                    return null;
                }
                Room room = new Room(roomDetail);
                _forwarderAddress = room.Address;
                _currentRoom = room;
                return room;
            });
        }

        public IServiceCallback<bool> RoomExit(uint roomId)
        {
            return RoomExit(roomId, _api.OpenAPI.Config.MainServer);
        }

        public IServiceCallback<bool> RoomExit(uint roomId, IPEndPoint address)
        {
            return new ServiceCallback<bool>(() =>
            {
                if (address == null)
                {
                    address = _api.OpenAPI.Config.MainServer;
                }
                RoomExit roomExit = new RoomExit();
                roomExit.RoomId = roomId;
                MainMessage msg = new MainMessage();
                msg.RoomMsg = new RoomMsg();
                msg.RoomMsg.RoomExit = roomExit;
                MainMessage response = _api.OpenAPI.Networking.Send(msg, address);
                ErrorMsgCheck(response);
                Reset();
                return true;
            });
        }

        private static void ErrorMsgCheck(MainMessage msg)
        {
            if (SystemServiceClient.IsErrorMsg(msg))
            {
                ErrorMsg err = msg.SystemMsg.ErrorMsg;
                throw new RoomServiceException(err.ErrorMsg_);
            }
        }

        private void Reset()
        {
            _currentRoom = null;
            _forwarderAddress = null;
            OnRoomExit();
        }

        public void OnRoomExit()
        {
            RoomExited?.Invoke();
        }

        public void OnRoomEnter()
        {
            RoomEntered?.Invoke();
        }
    }
}
