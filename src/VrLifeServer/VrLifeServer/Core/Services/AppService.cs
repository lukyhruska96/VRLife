﻿using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services
{
    class AppService : IService
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