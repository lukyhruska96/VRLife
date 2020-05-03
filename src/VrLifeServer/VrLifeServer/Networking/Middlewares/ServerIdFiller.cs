using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Networking.Middlewares
{
    class ServerIdFiller : IMiddleware<MainMessage>
    {
        private uint _id;

        public ServerIdFiller()
        {
            this._id = 0;
        }

        public ServerIdFiller(uint id)
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
            msg.ServerId = this._id;
            return msg;
        }
    }
}
