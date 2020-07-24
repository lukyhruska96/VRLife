using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    public class ServerIdFiller : IServerIdFiller
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
