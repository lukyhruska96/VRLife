using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Services.RoomService;

namespace VrLifeServer.Core.Services.TickRateService
{
    interface ITickRateServiceForwarder : ITickRateService, IServiceForwarder
    {
        bool IsActive(ulong userId, uint roomId);
        void AddRoom(Room room);
        void DelRoom(Room room);
    }
}
