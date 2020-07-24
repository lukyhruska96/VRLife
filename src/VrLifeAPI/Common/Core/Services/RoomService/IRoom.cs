using System;
using System.Collections.Generic;
using System.Net;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.RoomService
{
    public interface IRoom
    {
        uint Id { get; set; }
        string Name { get; set; }
        uint Capacity { get; set; }

        // list of user IDs
        List<ulong> Players { get; }
        IPEndPoint Address { get; set; }
        uint TickRate { get; set; }
        ulong StartTime { get; set; }
        long LastActivity { get; set; }

        RoomDetail ToNetworkModel();
    }
}
