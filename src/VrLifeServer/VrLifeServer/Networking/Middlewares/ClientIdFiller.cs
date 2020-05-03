﻿using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Networking.Middlewares
{
    class ClientIdFiller : IMiddleware<MainMessage>
    {
        uint _id;
        public ClientIdFiller()
        {
            this._id = 0;
        }

        public ClientIdFiller(uint id)
        {
            this._id = id;
        }

        public void SetId(uint id)
        {
            this._id = id;
        }

        public MainMessage TransformInputMsg(MainMessage msg)
        {
            return msg;
        }

        public MainMessage TransformOutputMsg(MainMessage msg)
        {
            msg.ClientId = this._id;
            return msg;
        }
    }
}
