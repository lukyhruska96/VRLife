using System;
using System.Collections.Generic;
using System.Net;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.Services
{
    public interface IRoomServiceClient : IServiceClient
    {
        IPEndPoint ForwarderAddress { get; }

        IRoom CurrentRoom { get; }

        event Action RoomExited;

        event Action RoomEntered;

        IServiceCallback<IRoom> RoomDetail(uint roomId);

        IServiceCallback<List<IRoom>> RoomList(string contains = "", bool notEmpty = false, bool notFull = false);

        IServiceCallback<IRoom> RoomCreate(string name, uint capacity);

        IServiceCallback<IRoom> RoomEnter(uint roomId);

        IServiceCallback<IRoom> RoomEnter(uint roomId, IPEndPoint address);

        IServiceCallback<bool> RoomExit(uint roomId);

        IServiceCallback<bool> RoomExit(uint roomId, IPEndPoint address);

        void OnRoomExit();

        void OnRoomEnter();
    }
}
