using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.RoomService;

namespace VrLifeAPI.Forwarder.Core.Services.RoomService
{
    public delegate void UserConnectEventHandler(ulong userId, uint roomId);
    public delegate void UserDisconnectEventHandler(ulong userId, uint roomId, string reason);
    public delegate void RoomCreateEventHandler(IRoom room);
    public delegate void RoomDeleteEventHandler(uint roomId);

    public interface IRoomServiceForwarder : IRoomService, IServiceForwarder
    {
        uint? RoomByUserId(ulong userId);
        ulong[] GetConnectedUsers(uint roomId);

        event UserConnectEventHandler UserConnected;

        event UserDisconnectEventHandler UserDisconnected;

        event RoomDeleteEventHandler RoomDeleted;

        event RoomCreateEventHandler RoomCreated;
    }
}
