using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.RoomService
{
    class RoomServiceProvider : IRoomService
    {
        public MainMessage HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
        }
    }
}
