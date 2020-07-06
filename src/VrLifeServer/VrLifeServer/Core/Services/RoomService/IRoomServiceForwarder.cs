using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.RoomService
{
    interface IRoomServiceForwarder : IRoomService, IServiceForwarder
    {
        public uint? RoomByUserId(ulong userId);

        public delegate void UserDisconnectEventHandler(ulong userId, uint roomId, string reason);
        public event UserDisconnectEventHandler UserDisconnected;
    }
}
