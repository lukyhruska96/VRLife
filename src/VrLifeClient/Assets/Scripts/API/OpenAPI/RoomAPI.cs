using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Core.Wrappers;
using VrLifeAPI.Client.Core.Services;
using VrLifeClient.Core.Services.RoomService;

namespace VrLifeClient.API.OpenAPI
{
    class RoomAPI : IRoomAPI

    {
        private IRoomServiceClient _roomService;
        public event Action RoomExited
        {
            add { _roomService.RoomExited += value; }
            remove { _roomService.RoomExited -= value; }
        }
        public event Action RoomEntered
        {
            add { _roomService.RoomEntered += value; }
            remove { _roomService.RoomEntered -= value; }
        }
        public IRoom CurrentRoom { get => _roomService.CurrentRoom; }

        public void OnRoomEnter()
        {
            _roomService.OnRoomEnter();
        }

        public RoomAPI(IRoomServiceClient roomService)
        {
            this._roomService = roomService;
        }

        public IServiceCallback<IRoom> QuickJoin()
        {
            return new ServiceCallback<IRoom>(() =>
            {
                List<IRoom> rooms = _roomService.RoomList().Wait();
                IRoom r;
                if (rooms.Count == 0)
                {
                    r = _roomService.RoomCreate("First Room", 2).Wait();
                    return _roomService.RoomEnter(r.Id, r.Address).Wait();
                }
                foreach(Room room in rooms)
                {
                    if(!room.IsFull())
                    {
                        return _roomService.RoomEnter(room.Id).Wait();
                    }
                }
                r = _roomService.RoomCreate($"Generated No. {new Random().Next()}", 10).Wait();
                return _roomService.RoomEnter(r.Id).Wait();
            });
        }

        public IServiceCallback<bool> RoomExit(uint roomId)
        {
            return _roomService.RoomExit(roomId, _roomService.ForwarderAddress);
        }

        public IRoom GetRoomDetails()
        {
            return _roomService.CurrentRoom;
        }
    }
}
