using System;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.API.OpenAPI
{
    public interface IRoomAPI
    {
        event Action RoomExited;
        event Action RoomEntered;
        IRoom CurrentRoom { get; }

        void OnRoomEnter();

        IServiceCallback<IRoom> QuickJoin();

        IServiceCallback<bool> RoomExit(uint roomId);
    }
}
