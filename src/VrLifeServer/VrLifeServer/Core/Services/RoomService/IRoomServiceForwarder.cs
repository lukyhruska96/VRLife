using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.RoomService
{
    interface IRoomServiceForwarder : IRoomService, IServiceForwarder
    {
        uint? RoomByUserId(ulong userId);
        ulong[] GetConnectedUsers(uint roomId);

        delegate void UserConnectEventHandler(ulong userId, uint roomId);
        event UserConnectEventHandler UserConnected;

        delegate void UserDisconnectEventHandler(ulong userId, uint roomId, string reason);
        event UserDisconnectEventHandler UserDisconnected;

        delegate void RoomDeleteEventHandler(uint roomId);
        event RoomDeleteEventHandler RoomDeleted;

        delegate void RoomCreateEventHandler(Room room);
        event RoomCreateEventHandler RoomCreated;
    }
}
