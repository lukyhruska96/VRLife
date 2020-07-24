using System;
using VrLifeAPI.Client.Core.Wrappers;
using VrLifeAPI.Client.Services;

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
