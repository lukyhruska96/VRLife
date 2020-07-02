using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.RoomService
{
    interface IRoomServiceForwarder : IRoomService, IServiceForwarder
    {
    }
}
