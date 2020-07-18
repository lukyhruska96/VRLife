using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.Core.Services.RoomService;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.API.OpenAPI
{
    class RoomAPI
    {
        private RoomServiceClient _roomService;
        public event RoomServiceClient.RoomExitEventHandler RoomExited
        {
            add { _roomService.RoomExited += value; }
            remove { _roomService.RoomExited -= value; }
        }
        public event RoomServiceClient.RoomEnterEventHandler RoomEntered
        {
            add { _roomService.RoomEntered += value; }
            remove { _roomService.RoomEntered -= value; }
        }
        public Room CurrentRoom { get => _roomService.CurrentRoom; }

        public void OnRoomEnter()
        {
            _roomService.OnRoomEnter();
        }

        public RoomAPI(RoomServiceClient roomService)
        {
            this._roomService = roomService;
        }

        public ServiceCallback<Room> QuickJoin()
        {
            return new ServiceCallback<Room>(() =>
            {
                List<Room> rooms = _roomService.RoomList().Wait();
                Room r;
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

        public ServiceCallback<bool> RoomExit(uint roomId)
        {
            return _roomService.RoomExit(roomId, _roomService.ForwarderAddress);
        }

        public Room GetRoomDetails()
        {
            return _roomService.CurrentRoom;
        }
    }
}
